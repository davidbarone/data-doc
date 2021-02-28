using System.Data.SqlClient;
using Dapper;
using System.Collections.Generic;
using DataDoc.Models;

namespace DataDoc
{
    public class MetadataRepository
    {
        private string ConnectionString { get; set; }

        public MetadataRepository(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        public static MetadataRepository Connect(string connectionString)
        {
            return new MetadataRepository(connectionString);
        }

        public IEnumerable<ProjectInfo> GetProjects()
        {
            using (var db = new SqlConnection(ConnectionString))
            {
                db.Open();
                db.ChangeDatabase("MetadataRepository");
                return db.Query<ProjectInfo>("SELECT * FROM PROJECT");
            }
        }

        public IEnumerable<EntityInfo> GetEntities(ProjectInfo project)
        {
            using (var db = new SqlConnection(ConnectionString))
            {
                db.Open();
                db.ChangeDatabase("MetadataRepository");
                return db.Query<EntityInfo>("SELECT * FROM Entity WHERE ProjectName = @ProjectName", new { ProjectName = project.ProjectName });
            }
        }

        public IEnumerable<AttributeInfo> GetAttributes(ProjectInfo project)
        {
            using (var db = new SqlConnection(ConnectionString))
            {
                db.Open();
                db.ChangeDatabase("MetadataRepository");
                return db.Query<AttributeInfo>("SELECT * FROM Attribute WHERE ProjectName = @ProjectName", new { ProjectName = project.ProjectName });
            }
        }

        public void CreateProject(string projectName, string connectionString)
        {
            using (var db = new SqlConnection(ConnectionString))
            {
                db.Open();
                db.ChangeDatabase("MetadataRepository");
                db.Execute(
                    "INSERT INTO PROJECT (ProjectName, ConnectionString, Version, LastUpdated) VALUES (@ProjectName, @ConnectionString, 0, GETDATE())",
                    new
                    {
                        ProjectName = projectName,
                        ConnectionString = connectionString
                    });
            }
        }

        public void ScanProject(ProjectInfo project)
        {
            var entities = ScanEntities(project);
            SaveEntities(project, entities);
            var attributes = ScanAttributes(project);
            SaveAttributes(project, attributes);
        }

        private IEnumerable<EntityInfo> ScanEntities(ProjectInfo project)
        {
            using (var db = new SqlConnection(project.ConnectionString))
            {
                var entities = db.Query<EntityInfo>(SqlGetEntities);
                // Add projectName
                foreach (var e in entities)
                {
                    e.ProjectName = project.ProjectName;
                }
                return entities;
            }
        }

        private void SaveEntities(ProjectInfo project, IEnumerable<EntityInfo> entities)
        {
            using (var db = new SqlConnection(ConnectionString))
            {
                //Logger.Log(LogType.INFORMATION, string.Format("Getting server objects for database: {0}.", database));
                db.Open();
                db.ChangeDatabase("MetadataRepository");
                db.Execute("DELETE FROM Entity WHERE ProjectName = @ProjectName", new { ProjectName = project.ProjectName });
                var dt = entities.ToDataTable();
                db.BulkCopy(dt, "Entity");
                db.Execute(SqlInsertMissingEntityPublications, new { ProjectName = project.ProjectName });
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
                    a.ProjectName = project.ProjectName;
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
                db.ChangeDatabase("MetadataRepository");
                db.Execute("DELETE FROM Attribute WHERE ProjectName = @ProjectName", new { ProjectName = project.ProjectName });
                var dt = attributes.ToDataTable();
                db.BulkCopy(dt, "Attribute");
            }
        }

        #region SqlTemplates

        private string SqlInsertMissingEntityPublications = @"
WITH cteMissingEntities
AS
(
	SELECT
		ProjectName,
		EntityName
	FROM
		Entity
	WHERE
		ProjectName = @ProjectName

	EXCEPT

	SELECT
		ProjectName,
		EntityName
	FROM
		EntityPublication
	WHERE
		ProjectName = @ProjectName
)

SELECT
	ProjectName,
	EntityName,
	EntityName EntityAlias,
	'The ' + EntityName + ' entity is used for ...' EntityDescription,
	CAST(1 AS BIT) IsPublished
FROM
	cteMissingEntities
WHERE
	ProjectName = @ProjectName";

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
			C.SCALE SCALE
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
	    C.DATABASE_NAME,
	    C.SCHEMA_NAME,
	    C.OBJECT_NAME,
	    C.COLUMN_ID,
	    C.COLUMN_NAME,
	    C.DATA_TYPE,
	    C.MAXIMUM_LENGTH,
	    C.PRECISION,
	    C.SCALE,
	    CASE WHEN PK.PK_COLUMN_ID IS NOT NULL THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END PRIMARY_KEY
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
		DB_NAME() DATABASE_NAME,
	    SCHEMA_NAME(O.SCHEMA_ID) SCHEMA_NAME,
	    OBJECT_NAME(O.OBJECT_ID) OBJECT_NAME,
	    COLS.COLUMN_ORDINAL COLUMN_ID,
	    COLS.NAME COLUMN_NAME,
	    T.NAME DATA_TYPE,
	    COLS.MAX_LENGTH MAXIMUM_LENGTH,
		COLS.PRECISION PRECISION,
		COLS.SCALE SCALE,
	    CAST(0 AS BIT) PRIMARY_KEY
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
	CASE DATA_TYPE
		WHEN 'varchar' THEN 'String'
		WHEN 'nvarchar' THEN 'String'
		WHEN 'char' THEN 'String'
		WHEN 'nchar' THEN 'String'
		WHEN 'int' THEN 'Integer64'
		WHEN 'smallint' THEN 'Integer16'
		WHEN 'tinyint' THEN 'Integer8'
		WHEN 'bigint' THEN 'Integer64'
		WHEN 'float' THEN 'Float32'
		WHEN 'real' THEN 'Float32'
		WHEN 'decimal' THEN 'Decimal'
		WHEN 'numeric' THEN 'Decimal'
		WHEN 'datetime' THEN 'DateTime'
		WHEN 'datetime2' THEN 'DateTime'
		WHEN 'date' THEN 'Date'
		WHEN 'time' THEN 'Time'
		WHEN 'bit' THEN 'Boolean'
		WHEN 'uniqueidentifier' THEN 'Guid'
		ELSE 'String'
	END DataType,
	MAXIMUM_LENGTH DataLength,
	PRECISION [Precision],
	SCALE Scale
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
SELECT * FROM cte WHERE EntityType <> 'Unknown'";

        #endregion

    }
}