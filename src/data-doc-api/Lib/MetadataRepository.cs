using System.Data.SqlClient;
using Dapper;
using System.Collections.Generic;
using data_doc_api.Models;
using System.Linq;
using System;

namespace data_doc_api
{
    public class MetadataRepository
    {
        private string ConnectionString { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="connectionString"></param>
		public MetadataRepository(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        /// <summary>
        /// Factory method to create a new MetadataRepository instance.
        /// </summary>
        /// <param name="connectionString">The connection string</param>
        /// <returns></returns>
		public static MetadataRepository Connect(string connectionString)
        {
            return new MetadataRepository(connectionString);
        }

        #region Projects

        public IEnumerable<ProjectInfo> GetProjects()
        {
            using (var db = new SqlConnection(ConnectionString))
            {
                var projects = db.Query<ProjectInfo>("SELECT * FROM PROJECT WHERE ISACTIVE = 1");
                return projects;
            }
        }

        public ProjectInfo GetProject(int id)
        {
            using (var db = new SqlConnection(ConnectionString))
            {
                var project = db.Query<ProjectInfo>(
                    "SELECT * FROM PROJECT WHERE ProjectId = @ProjectId",
                    new
                    {
                        ProjectId = id
                    }).FirstOrDefault();

                if (project == null)
                {
                    throw new Exception("Task not found.");
                }

                return project;
            }
        }

        public ProjectInfo CreateProject(ProjectInfo project)
        {
            using (var db = new SqlConnection(ConnectionString))
            {
                var now = DateTime.Now;

                var newProject = db.Query<ProjectInfo>(@"
INSERT INTO Project (
    ProjectName, ProjectDesc, ConnectionString, ScanVersion, ScanUpdatedDt, ConfigVersion, ConfigUpdatedDt, IsActive)
SELECT
    @ProjectName, @ProjectDesc, @ConnectionString, @ScanVersion, @ScanUpdatedDt, @ConfigVersion, @ConfigUpdatedDt, @IsActive;
SELECT * FROM Project WHERE ProjectId = SCOPE_IDENTITY();", new
                {
                    ProjectName = project.ProjectName,
                    ProjectDesc = project.ProjectDesc,
                    ConnectionString = project.ConnectionString,
                    ScanVersion = 0,
                    ScanUpdatedDt = now,
                    ConfigVersion = 0,
                    ConfigUpdatedDt = now,
                    IsActive = 1
                });
                return newProject.First();
            }
        }

        public void UpdateProject(int id, ProjectInfo project)
        {
            if (id != project.ProjectId)
            {
                throw new Exception("Invalid project id");
            }

            using (var db = new SqlConnection(ConnectionString))
            {
                var newProject = db.Execute(@"
UPDATE
    Project
SET
    ProjectName = @ProjectName,
    ProjectDesc = @ProjectDesc,
    ConnectionString = @ConnectionString,
    ScanVersion = @ScanVersion,
    ScanUpdatedDt = @ScanUpdatedDt,
    ConfigVersion = @ConfigVersion,
    ConfigUpdatedDt = @ConfigUpdatedDt,
    IsActive = @IsActive
WHERE
    ProjectId = @ProjectId;", new
                {
                    ProjectId = project.ProjectId,
                    ProjectName = project.ProjectName,
                    ProjectDesc = project.ProjectDesc,
                    ConnectionString = project.ConnectionString,
                    ScanVersion = project.ScanVersion,
                    ScanUpdatedDt = project.ScanUpdatedDt,
                    ConfigVersion = project.ConfigVersion,
                    ConfigUpdatedDt = project.ConfigUpdatedDt,
                    IsActive = project.IsActive
                });
            }
        }

        public void DeleteProject(int id)
        {
            using (var db = new SqlConnection(ConnectionString))
            {
                var newTask = db.Query<ProjectInfo>(@"
DELETE FROM
    Project
WHERE
    ProjectId = @ProjectId;", new
                {
                    ProjectId = id
                });
            }
        }

        #endregion

        #region Entities

        public IEnumerable<EntityInfo> GetEntities(int projectId)
        {
            using (var db = new SqlConnection(ConnectionString))
            {
                db.Open();
                return db.Query<EntityInfo>("SELECT * FROM Entity WHERE ProjectId = @ProjectId", new { ProjectId = projectId });
            }
        }

        public IEnumerable<EntityConfigInfo> GetEntitiesConfig(int projectId)
        {
            using (var db = new SqlConnection(ConnectionString))
            {
                var sql = @"
SELECT
	EC.EntityConfigId,
    COALESCE(E.ProjectId, EC.ProjectId) ProjectId,
	COALESCE(E.EntityName, EC.EntityName) EntityName,
	COALESCE(EC.EntityAlias, E.EntityName) EntityAlias,
	COALESCE(EC.EntityDesc, '[No definition available for entity.]') EntityDesc,
	COALESCE(EC.ShowData, CAST(0 AS BIT)) ShowData,
	COALESCE(EC.ShowDefinition, CAST(0 AS BIT)) ShowDefinition,
	COALESCE(EC.IsActive, CASE WHEN E.EntityName = '<Global>' THEN CAST(0 AS BIT) ELSE CAST(1 AS BIT) END) IsActive
FROM
	Entity E
LEFT OUTER JOIN
	EntityConfig EC
ON
	E.ProjectId = EC.ProjectId AND
	E.EntityName = EC.EntityName
WHERE
	E.ProjectId  = @ProjectId";
                return db.Query<EntityConfigInfo>(sql, new { ProjectId = projectId });
            }
        }

        public EntityConfigInfo SetEntityConfig(EntityConfigInfo entityConfig)
        {
            using (var db = new SqlConnection(ConnectionString))
            {
                var sql = @"
                    DECLARE @EntityConfigId INT;
                    SELECT @EntityConfigId = EntityConfigId FROM EntityConfig WHERE ProjectId = @ProjectId AND EntityName = @EntityName;
                    IF @EntityConfigId IS NOT NULL
                    BEGIN
                        DELETE FROM EntityConfig WHERE EntityConfigId = @EntityConfigId;
                    END
                    INSERT INTO
                        EntityConfig
                            (ProjectId, EntityName, EntityAlias, EntityDesc, ShowData, ShowDefinition, IsActive)
                        SELECT
                            @ProjectId,
                            @EntityName,
                            @EntityAlias,
                            @EntityDesc,
                            @ShowData,
                            @ShowDefinition,
                            @IsActive;
                    SELECT * FROM EntityConfig WHERE EntityConfigId = SCOPE_IDENTITY();";
                return db.Query<EntityConfigInfo>(sql, new
                {
                    ProjectId = entityConfig.ProjectId,
                    EntityName = entityConfig.EntityName,
                    EntityAlias = entityConfig.EntityAlias,
                    EntityDesc = entityConfig.EntityDesc,
                    ShowData = entityConfig.ShowData,
                    ShowDefinition = entityConfig.ShowDefinition,
                    IsActive = entityConfig.IsActive
                }).First();
            }
        }

        public void UnsetEntityConfig(int id)
        {
            using (var db = new SqlConnection(ConnectionString))
            {
                var sql = @"DELETE FROM EntityConfig WHERE EntityConfigId = @EntityConfigId";
                db.Execute(sql, new { EntityConfigId = id });
            }
        }

        private IEnumerable<EntityInfo> ScanEntities(ProjectInfo project)
        {
            using (var db = new SqlConnection(project.ConnectionString))
            {
                var entities = db.Query<EntityInfo>(SqlGetEntities);
                // Add projectName
                foreach (var e in entities)
                {
                    e.ProjectId = project.ProjectId;
                }
                return entities;
            }
        }

        private void SaveEntities(ProjectInfo project, IEnumerable<EntityInfo> entities)
        {
            using (var db = new SqlConnection(ConnectionString))
            {
                db.Open();
                db.Execute("DELETE FROM Entity WHERE ProjectId = @ProjectId", new { ProjectId = project.ProjectId });
                var dt = entities.ToDataTable();
                db.BulkCopy(dt, "Entity");
                var entitiesConfig = db.Query<EntityConfigInfo>(SqlGetMissingEntityConfig, new { ProjectId = project.ProjectId });
                dt = entitiesConfig.ToDataTable();
                db.BulkCopy(dt, "EntityConfig");
            }
        }

        #endregion

        #region Attributes

        public IEnumerable<AttributeInfo> GetAttributes(int projectId)
        {
            using (var db = new SqlConnection(ConnectionString))
            {
                db.Open();
                return db.Query<AttributeInfo>("SELECT * FROM Attribute WHERE ProjectId = @ProjectId", new { ProjectId = projectId });
            }
        }

        public IEnumerable<AttributeConfigScopedInfo> GetAttributesConfig(int projectId)
        {
            using (var db = new SqlConnection(ConnectionString))
            {
                var sql = @"
SELECT
	COALESCE(AC.AttributeConfigId,ACProject.AttributeConfigId, ACGlobal.AttributeConfigId) AttributeConfigId,
	COALESCE(AC.ProjectId, A.ProjectId) ProjectId,
	COALESCE(AC.EntityName, A.EntityName) EntityName,
	COALESCE(AC.AttributeName, A.AttributeName) AttributeName,
	COALESCE(AC.AttributeDesc, ACProject.AttributeDesc, ACGlobal.AttributeDesc, '') AttributeDesc,
	COALESCE(AC.IsActive, ACProject.IsActive, ACGlobal.IsActive, 1) IsActive,
	CASE
		WHEN AC.AttributeConfigId IS NOT NULL THEN 'Local'
		WHEN ACProject.AttributeConfigId IS NOT NULL THEN 'Project'
		WHEN ACGlobal.AttributeConfigId IS NOT NULL THEN 'Global'
		ELSE 'Undefined'
	END Scope
FROM
	Attribute A
LEFT OUTER JOIN
	AttributeConfig AC
ON
	A.ProjectId = AC.ProjectId AND
	A.EntityName = AC.EntityName AND
	A.AttributeName = AC.AttributeName
LEFT OUTER JOIN
	(SELECT * FROM AttributeConfig WHERE EntityName = '<Global>') ACProject
ON
	A.ProjectId = ACProject.ProjectId AND
	A.AttributeName = ACProject.AttributeName
LEFT OUTER JOIN
	(SELECT * FROM AttributeConfig WHERE EntityName = '<Global>' AND ProjectId = -1) ACGlobal
ON
	A.AttributeName = ACGlobal.AttributeName
WHERE
	A.ProjectId = @ProjectId";
                return db.Query<AttributeConfigScopedInfo>(sql, new { ProjectId = projectId });
            }
        }

        public AttributeConfigInfo SetAttributeConfig(AttributeConfigInfo attributeConfig)
        {
            using (var db = new SqlConnection(ConnectionString))
            {
                var sql = @"
                    DECLARE @AttributeConfigId INT;
                    SELECT @AttributeConfigId = AttributeConfigId FROM AttributeConfig WHERE ProjectId = @ProjectId AND EntityName = @EntityName AND AttributeName = @AttributeName;
                    IF @AttributeConfigId IS NOT NULL
                    BEGIN
                        DELETE FROM AttributeConfig WHERE AttributeConfigId = @AttributeConfigId;
                    END
                    INSERT INTO
                        AttributeConfig
                            (ProjectId, EntityName, AttributeName, AttributeDesc, IsActive)
                        SELECT
                            @ProjectId,
                            @EntityName,
                            @AttributeName,
                            @AttributeDesc,
                            @IsActive;
                    SELECT * FROM AttributeConfig WHERE AttributeConfigId = SCOPE_IDENTITY();";
                return db.Query<AttributeConfigInfo>(sql, new
                {
                    ProjectId = attributeConfig.ProjectId,
                    EntityName = attributeConfig.EntityName,
                    AttributeName = attributeConfig.AttributeName,
                    AttributeDesc = attributeConfig.AttributeDesc,
                    IsActive = attributeConfig.IsActive
                }).First();
            }
        }

        public void UnsetAttributeConfig(int id)
        {
            using (var db = new SqlConnection(ConnectionString))
            {
                var sql = @"DELETE FROM AttributeConfig WHERE AttributeConfigId = @AttributeConfigId";
                db.Execute(sql, new { AttributeConfigId = id });
            }
        }

        #endregion

        #region Scanning

        public void ScanProject(ProjectInfo project)
        {
            var entities = ScanEntities(project);
            SaveEntities(project, entities);
            var attributes = ScanAttributes(project);
            SaveAttributes(project, attributes);
            var dependencies = ScanDependencies(project);
            SaveDependencies(project, dependencies);
        }

        public void ScanRelationships(ProjectInfo project)
        {
            var relationships = ScanRelationshipsEx(project);
            SaveRelationships(project, relationships);
        }

        private IEnumerable<RelationshipInfo> ScanRelationshipsEx(ProjectInfo project)
        {
            using (var db = new SqlConnection(project.ConnectionString))
            {
                var relationships = db.Query<RelationshipInfo>(SqlGetEntityRelationships);
                // Add projectName
                foreach (var r in relationships)
                {
                    r.ProjectId = project.ProjectId;
                }
                return relationships;
            }
        }

        #endregion

        private void SaveRelationships(ProjectInfo project, IEnumerable<RelationshipInfo> relationships)
        {
            using (var db = new SqlConnection(ConnectionString))
            {
                db.Open();
                DeleteRelationships(project);
                var dt = relationships.ToDataTable();
                db.BulkCopy(dt, "Relationship");
            }
        }

        public void DeleteRelationships(ProjectInfo project)
        {
            using (var db = new SqlConnection(ConnectionString))
            {
                db.Open();
                db.Execute("DELETE FROM Relationship WHERE ProjectId = @ProjectId", new { ProjectId = project.ProjectId });
            }
        }


        private IEnumerable<AttributeInfo> ScanAttributes(ProjectInfo project)
        {
            using (var db = new SqlConnection(project.ConnectionString))
            {
                var attributes = db.Query<AttributeInfo>(SqlGetAttributes);
                // Add projectName
                foreach (var a in attributes)
                {
                    a.ProjectId = project.ProjectId;
                }
                return attributes;
            }
        }

        private void SaveAttributes(ProjectInfo project, IEnumerable<AttributeInfo> attributes)
        {
            using (var db = new SqlConnection(ConnectionString))
            {
                //Logger.Log(LogType.INFORMATION, string.Format("Getting server objects for database: {0}.", database));
                db.Open();
                db.Execute("DELETE FROM Attribute WHERE ProjectId = @ProjectId", new { ProjectId = project.ProjectId });
                var dt = attributes.ToDataTable();
                db.BulkCopy(dt, "Attribute");
                var attributesConfig = db.Query<AttributeConfigInfo>(SqlGetMissingAttributeConfig, new { ProjectId = project.ProjectId });
                dt = attributesConfig.ToDataTable();
                db.BulkCopy(dt, "AttributeConfig");
            }
        }

        public IEnumerable<dynamic> GetEntityData(ProjectInfo project, EntityInfo entity)
        {
            using (var db = new SqlConnection(project.ConnectionString))
            {
                var data = db.Query($"SELECT * FROM {entity.EntityName}");
                return data;
            }
        }

        private IEnumerable<EntityDependencyInfo> ScanDependencies(ProjectInfo project)
        {
            using (var db = new SqlConnection(project.ConnectionString))
            {
                var dependencies = db.Query<EntityDependencyInfo>(SqlGetEntityDependencies);
                // Add projectName
                foreach (var d in dependencies)
                {
                    d.ProjectId = project.ProjectId;
                }
                return dependencies;
            }
        }

        private void SaveDependencies(ProjectInfo project, IEnumerable<EntityDependencyInfo> dependencies)
        {
            using (var db = new SqlConnection(ConnectionString))
            {
                db.Open();
                db.Execute("DELETE FROM EntityDependency WHERE ProjectId = @ProjectId", new { ProjectId = project.ProjectId });
                var dt = dependencies.ToDataTable();
                db.BulkCopy(dt, "EntityDependency");
            }
        }

        public IEnumerable<EntityDependencyInfo> GetEntityDependencies(ProjectInfo project)
        {
            using (var db = new SqlConnection(ConnectionString))
            {
                db.Open();
                return db.Query<EntityDependencyInfo>("SELECT * FROM EntityDependency WHERE ProjectId = @ProjectId", new { ProjectId = project.ProjectId });
            }
        }

        public IEnumerable<RelationshipInfo> GetRelationships(ProjectInfo project)
        {
            using (var db = new SqlConnection(ConnectionString))
            {
                var data = db.Query<RelationshipInfo>($"SELECT * FROM Relationship");
                return data;
            }
        }

        #region SqlTemplates

        private string SqlGetEntityRelationships = @"
SELECT
    fk.name 'RelationshipName',
    SCHEMA_NAME(tp.schema_id) + '.' + tp.name ParentEntityName,
    cp.name ParentAttributeName,
	--cp.column_id,
    SCHEMA_NAME(tr.schema_id) + '.' + tr.name ReferencedEntityName,
    cr.name ReferencedAttributeName
	--cr.column_id
FROM 
    sys.foreign_keys fk
INNER JOIN 
    sys.tables tp ON fk.parent_object_id = tp.object_id
INNER JOIN 
    sys.tables tr ON fk.referenced_object_id = tr.object_id
INNER JOIN 
    sys.foreign_key_columns fkc ON fkc.constraint_object_id = fk.object_id
INNER JOIN 
    sys.columns cp ON fkc.parent_column_id = cp.column_id AND fkc.parent_object_id = cp.object_id
INNER JOIN 
    sys.columns cr ON fkc.referenced_column_id = cr.column_id AND fkc.referenced_object_id = cr.object_id
ORDER BY
    tp.name, cp.column_id";
        private string SqlGetEntityDependencies = @"
WITH cte
AS
(
	SELECT DISTINCT
		OBJECT_SCHEMA_NAME(REFERENCING_ID) + '.' + OBJECT_NAME(REFERENCING_ID) PARENT_ENTITY_NAME,
		DEP.REFERENCING_ID PARENT_ID,
		COALESCE(DEP.REFERENCED_SCHEMA_NAME, OBJECT_SCHEMA_NAME(DEP.REFERENCED_ID)) + '.' + DEP.REFERENCED_ENTITY_NAME CHILD_ENTITY_NAME,
		DEP.REFERENCED_ID CHILD_ID
	FROM
		sys.sql_expression_dependencies DEP
)
SELECT
	PARENT_ENTITY_NAME ParentEntityName,
	CHILD_ENTITY_NAME ChildEntityName
FROM
	cte
WHERE
	CHILD_ENTITY_NAME IS NOT NULL AND PARENT_ENTITY_NAME IS NOT NULL";

        private string SqlGetMissingAttributeConfig = @"
WITH cteMissingAttributes
AS
(
	SELECT
		ProjectId,
		EntityName,
		AttributeName
	FROM
		Attribute
	WHERE
		ProjectId = @ProjectId

	EXCEPT

	SELECT
		ProjectId,
		EntityName,
		AttributeName
	FROM
		AttributeConfig
	WHERE
		ProjectId = @ProjectId
)

SELECT
	ProjectId,
	EntityName,
	AttributeName,
	'' AttributeDesc,
	CAST(1 AS BIT) IsActive
FROM
	cteMissingAttributes
WHERE
	ProjectId = @ProjectId";

        private string SqlGetMissingEntityConfig = @"
WITH cteMissingEntities
AS
(
	SELECT
		ProjectId,
		EntityName
	FROM
		Entity
	WHERE
		ProjectId = @ProjectId

	EXCEPT

	SELECT
		ProjectId,
		EntityName
	FROM
		EntityConfig
	WHERE
		ProjectId = @ProjectId
)

SELECT
	ProjectId,
	EntityName,
	EntityName EntityAlias,
	'[No description currently available for this entity]' EntityDesc,
	CAST(1 AS BIT) IsActive,
	CAST(0 AS BIT) ShowData,
    CAST(0 AS BIT) ShowDefinition
FROM
	cteMissingEntities
WHERE
	ProjectId = @ProjectId";

        private string SqlGetAttributes = @"
WITH ctePK
AS
(
	SELECT
		SCHEMA_NAME(TAB.SCHEMA_ID) as SCHEMA_NAME, 
		PK.[NAME] as PK_NAME,
		IC.INDEX_COLUMN_ID as PK_COLUMN_ID,
		COL.[NAME] as COLUMN_NAME, 
		TAB.[NAME] as TABLE_NAME
	FROM
		SYS.TABLES TAB
	INNER JOIN
		SYS.INDEXES PK
	ON
		TAB.OBJECT_ID = PK.OBJECT_ID AND
		PK.IS_PRIMARY_KEY = 1
	INNER JOIN
		SYS.INDEX_COLUMNS IC
	ON
		IC.OBJECT_ID = PK.OBJECT_ID AND
		IC.INDEX_ID = PK.INDEX_ID
	INNER JOIN
		SYS.COLUMNS COL
	ON
		PK.OBJECT_ID = COL.OBJECT_ID AND
		COL.COLUMN_ID = IC.COLUMN_ID
)
, cteCols
AS
(
	SELECT
		DB_NAME() DATABASE_NAME,
		SCHEMA_NAME(O.SCHEMA_ID) SCHEMA_NAME,
		OBJECT_NAME(C.OBJECT_ID) OBJECT_NAME,
		C.COLUMN_ID,
		C.NAME COLUMN_NAME,
		T.NAME DATA_TYPE,
		C.MAX_LENGTH MAXIMUM_LENGTH,
		C.PRECISION PRECISION,
		C.SCALE SCALE,
		C.IS_NULLABLE
		FROM
			SYS.COLUMNS C
		INNER JOIN
			SYS.TYPES T
		ON
			C.USER_TYPE_ID = T.USER_TYPE_ID
		INNER JOIN
			SYS.OBJECTS O
		ON
			C.OBJECT_ID = O.OBJECT_ID
)
  , cteFinal
   AS
(
    SELECT
	    C.DATABASE_NAME COLLATE Latin1_General_CI_AS DATABASE_NAME,
	    C.SCHEMA_NAME COLLATE Latin1_General_CI_AS SCHEMA_NAME,
	    C.OBJECT_NAME COLLATE Latin1_General_CI_AS OBJECT_NAME,
	    C.COLUMN_ID,
	    C.COLUMN_NAME COLLATE Latin1_General_CI_AS COLUMN_NAME,
	    C.DATA_TYPE COLLATE Latin1_General_CI_AS DATA_TYPE,
	    C.MAXIMUM_LENGTH,
	    C.PRECISION,
	    C.SCALE,
	    CASE WHEN PK.PK_COLUMN_ID IS NOT NULL THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END PRIMARY_KEY,
		C.IS_NULLABLE
    FROM
	    cteCols C
    LEFT OUTER JOIN
	    ctePK PK
    ON
	    C.SCHEMA_NAME = PK.SCHEMA_NAME AND
	    C.OBJECT_NAME = PK.TABLE_NAME AND
	    C.COLUMN_NAME = PK.COLUMN_NAME
	WHERE
		C.SCHEMA_NAME NOT IN ('SYS')

	UNION ALL

    -- COLUMNS FOR PROCEDURES (NOTE THAT THE PROCEDURE MUST RETURN THE SAME SHAPE RESULTS. WE DO NOT ALLOW
    -- PROCEDURES TO RETURN DIFFERENT SHAPES BASED ON THE PARAMETER(S) PASSED IN).
    SELECT
		DB_NAME() COLLATE Latin1_General_CI_AS DATABASE_NAME,
	    SCHEMA_NAME(O.SCHEMA_ID) COLLATE Latin1_General_CI_AS SCHEMA_NAME,
	    OBJECT_NAME(O.OBJECT_ID) COLLATE Latin1_General_CI_AS OBJECT_NAME,
	    COLS.COLUMN_ORDINAL COLUMN_ID,
	    COLS.NAME COLLATE Latin1_General_CI_AS COLUMN_NAME,
	    T.NAME COLLATE Latin1_General_CI_AS DATA_TYPE,
	    COLS.MAX_LENGTH MAXIMUM_LENGTH,
		COLS.PRECISION PRECISION,
		COLS.SCALE SCALE,
	    CAST(0 AS BIT) PRIMARY_KEY,
		COLS.IS_NULLABLE
    FROM
	    SYS.OBJECTS O
    CROSS APPLY
	    SYS.DM_EXEC_DESCRIBE_FIRST_RESULT_SET_FOR_OBJECT(O.OBJECT_ID,NULL) COLS
    INNER JOIN
	    SYS.TYPES T
    ON
	    COLS.SYSTEM_TYPE_ID = T.USER_TYPE_ID
    WHERE
	    O.TYPE = 'P' AND
		SCHEMA_NAME(O.SCHEMA_ID) NOT IN ('SYS')
)

SELECT
	SCHEMA_NAME + '.' + OBJECT_NAME EntityName,
	COLUMN_NAME AttributeName,
	COLUMN_ID [Order],
	PRIMARY_KEY IsPrimaryKey,
	DATA_TYPE DataType,
	MAXIMUM_LENGTH DataLength,
	PRECISION [Precision],
	SCALE Scale,
	IS_NULLABLE IsNullable
FROM
	cteFinal";

        private string SqlGetEntities = @"
-------------------------------------
-- Get Entities
-------------------------------------
WITH cte
AS
(
	SELECT
		SCHEMA_NAME(SCHEMA_ID) + '.' + O.NAME EntityName,
		CASE
			WHEN O.[TYPE] = 'U' THEN 'Table'
			WHEN O.[TYPE] = 'IF' THEN 'Function'
			WHEN O.[TYPE] = 'TF' THEN 'Function'
			WHEN O.[TYPE] = 'SN' THEN 'Synonym'
			WHEN O.[TYPE] = 'P' THEN 'Procedure'
			WHEN O.[TYPE] = 'V' THEN 'View'
			ELSE 'Unknown'
		END EntityType,
		(SELECT SUM(ROW_COUNT) FROM SYS.DM_DB_PARTITION_STATS PS WITH (NOLOCK) WHERE PS.OBJECT_ID = O.OBJECT_ID AND (INDEX_ID=0 or INDEX_ID=1)) [RowCount],
		O.CREATE_DATE CreatedDt,
		O.MODIFY_DATE ModifiedDt,
		STAT.LAST_USER_UPDATE UpdatedDt,
		OBJECT_DEFINITION(O.OBJECT_ID) Definition
	FROM
		SYS.OBJECTS O WITH (NOLOCK) 
	LEFT OUTER JOIN (
		SELECT
			DATABASE_ID,
			OBJECT_ID,
			MAX(LAST_USER_UPDATE) LAST_USER_UPDATE
		FROM
			SYS.DM_DB_INDEX_USAGE_STATS WITH (NOLOCK) 
		GROUP BY 
			DATABASE_ID,
			OBJECT_ID
		) STAT
	ON
		STAT.DATABASE_ID = DB_ID() AND
		STAT.OBJECT_ID = O.OBJECT_ID
	WHERE
		SCHEMA_NAME(SCHEMA_ID) NOT IN ('SYS')
)
SELECT * FROM cte WHERE EntityType <> 'Unknown'
UNION ALL
-- Global entity
SELECT '<Global>', '<Global>', NULL, NULL, NULL, NULL, NULL";

        #endregion

    }
}