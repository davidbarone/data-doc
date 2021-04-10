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
    ProjectName, ProjectDesc, ProjectComment, ConnectionString, ScanVersion, ScanUpdatedDt, ConfigVersion, ConfigUpdatedDt, IsActive)
SELECT
    @ProjectName, @ProjectDesc, @ProjectComment, @ConnectionString, @ScanVersion, @ScanUpdatedDt, @ConfigVersion, @ConfigUpdatedDt, @IsActive;
SELECT * FROM Project WHERE ProjectId = SCOPE_IDENTITY();", new
                {
                    ProjectName = project.ProjectName,
                    ProjectDesc = project.ProjectDesc,
                    ProjectComment = project.ProjectComment,
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
    ProjectComment = @ProjectComment,
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
                    ProjectComment = project.ProjectComment,
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

        /// <summary>
        /// Gets the full entity details for a project
        /// </summary>
        /// <param name="projectId">The project id</param>
        /// <returns></returns>
        public IEnumerable<EntityDetailsInfo> GetEntityDetails(int projectId)
        {
            using (var db = new SqlConnection(ConnectionString))
            {
                return db.Query<EntityDetailsInfo>("SELECT * FROM EntityDetails WHERE ProjectId = @ProjectId", new { ProjectId = projectId });
            }
        }

        /// <summary>
        /// Gets the full entity detail for a single entity
        /// </summary>
        /// <param name="projectId">The project id</param>
        /// <param name="entityName">The entity name</param>
        /// <returns></returns>
        public EntityDetailsInfo GetEntityDetail(int projectId, string entityName)
        {
            using (var db = new SqlConnection(ConnectionString))
            {
                return db.Query<EntityDetailsInfo>("SELECT * FROM EntityDetails WHERE ProjectId = @ProjectId AND EntityName = @EntityName", new
                {
                    ProjectId = projectId,
                    EntityName = entityName
                }).FirstOrDefault();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="entityName"></param>
        /// <param name="payload"></param>
        /// <returns></returns>
        public EntityDetailsInfo SetEntityConfig(int projectId, string entityName, EntityConfigPayloadInfo payload)
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
                            (ProjectId, EntityName, EntityAlias, EntityDesc, EntityComment, ShowData, ShowDefinition, IsActive)
                        SELECT
                            @ProjectId,
                            @EntityName,
                            @EntityAlias,
                            @EntityDesc,
                            @EntityComment,
                            @ShowData,
                            @ShowDefinition,
                            @IsActive;";
                db.Execute(sql, new
                {
                    ProjectId = projectId,
                    EntityName = entityName,
                    EntityAlias = payload.EntityAlias,
                    EntityDesc = payload.EntityDesc,
                    EntityComment = payload.EntityComment,
                    ShowData = payload.ShowData,
                    ShowDefinition = payload.ShowDefinition,
                    IsActive = payload.IsActive
                });
                return this.GetEntityDetail(projectId, entityName);
            }
        }

        /// <summary>
        /// Clears the entity configuration
        /// </summary>
        /// <param name="projectId">The project id</param>
        /// <param name="entityName">The entity name</param>
        public EntityDetailsInfo UnsetEntityConfig(int projectId, string entityName)
        {
            using (var db = new SqlConnection(ConnectionString))
            {
                var sql = @"DELETE FROM EntityConfig WHERE ProjectId = @ProjectId AND EntityName = @EntityName";
                db.Execute(sql, new
                {
                    ProjectId = projectId,
                    EntityName = entityName
                });
                return this.GetEntityDetail(projectId, entityName);
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
            }
        }

        #endregion

        #region Attributes

        /// <summary>
        /// Gets raw scanned attribute data for a project.
        /// </summary>
        /// <param name="projectId">The project id.</param>
        /// <returns></returns>
        public IEnumerable<AttributeInfo> GetAttributes(int projectId)
        {
            using (var db = new SqlConnection(ConnectionString))
            {
                db.Open();
                return db.Query<AttributeInfo>("SELECT * FROM Attribute WHERE ProjectId = @ProjectId", new { ProjectId = projectId });
            }
        }

        /// <summary>
        /// Gets the full attribute details for a project.
        /// </summary>
        /// <param name="projectId">The project id</param>
        /// <returns>The attribute details.</returns>
        public IEnumerable<AttributeDetailsInfo> GetAttributeDetails(int projectId)
        {
            using (var db = new SqlConnection(ConnectionString))
            {
                var sql = @"SELECT * FROM AttributeDetails WHERE ProjectId = @ProjectId";
                return db.Query<AttributeDetailsInfo>(sql, new { ProjectId = projectId });
            }
        }

        /// <summary>
        /// Gets an single attribute detail
        /// </summary>
        /// <param name="projectId">The project id</param>
        /// <param name="entityName">The entity name</param>
        /// <param name="attributeName">The attribute name</param>
        /// <returns></returns>
        public AttributeDetailsInfo GetAttributeDetails(int projectId, string entityName, string attributeName)
        {
            using (var db = new SqlConnection(ConnectionString))
            {
                var sql = @"SELECT * FROM AttributeDetails WHERE ProjectId = @ProjectId";
                return db.Query<AttributeDetailsInfo>(sql, new
                {
                    ProjectId = projectId,
                    EntityName = entityName,
                    AttributeName = attributeName
                }).FirstOrDefault();
            }
        }

        /// <summary>
        /// Clears any custom override of the primary key status of an attribute, leaving it to whatever
        /// the scanned value is.
        /// </summary>
        /// <param name="projectId">The project id</param>
        /// <param name="entityName">The entity name</param>
        /// <param name="attributeName">The attribute name</param>
        /// <returns></returns>
        public AttributeDetailsInfo UnsetAttributePrimaryKey(int projectId, string entityName, string attributeName)
        {
            using (var db = new SqlConnection(ConnectionString))
            {
                var sql = @"
                    DECLARE @AttributePrimaryKeyConfigId INT;

                    SELECT @AttributePrimaryKeyConfigId = AttributePrimaryKeyConfigId
                    FROM
                        AttributePrimaryKeyConfig
                    WHERE
                        ProjectId = @ProjectId AND
                        EntityName = @EntityName AND
                        AttributeName = @AttributeName;

                    IF @AttributePrimaryKeyConfigId IS NOT NULL
                    BEGIN
                        DELETE FROM AttributePrimaryKeyConfig WHERE AttributePrimaryKeyConfigId = @AttributePrimaryKeyConfigId;
                    END";
                db.Execute(sql, new
                {
                    ProjectId = projectId,
                    EntityName = entityName,
                    AttributeName = attributeName
                });
                return this.GetAttributeDetails(projectId, entityName, attributeName);
            }
        }

        public AttributeDetailsInfo SetAttributePrimaryKey(int projectId, string entityName, string attributeName, bool isPrimaryKey)
        {
            using (var db = new SqlConnection(ConnectionString))
            {
                var sql = @"
                    DECLARE @AttributePrimaryKeyConfigId INT;

                    SELECT @AttributePrimaryKeyConfigId = AttributePrimaryKeyConfigId
                    FROM
                        AttributePrimaryKeyConfig
                    WHERE
                        ProjectId = @ProjectId AND
                        EntityName = @EntityName AND
                        AttributeName = @AttributeName;

                    IF @AttributePrimaryKeyConfigId IS NOT NULL
                    BEGIN
                        DELETE FROM AttributePrimaryKeyConfig WHERE AttributePrimaryKeyConfigId = @AttributePrimaryKeyConfigId;
                    END

                    INSERT INTO
                        AttributePrimaryKeyConfig (ProjectId, EntityName, AttributeName, IsPrimaryKey)
                    SELECT
                        @ProjectId, @EntityName, @AttributeName, @IsPrimaryKey;";
                db.Execute(sql, new
                {
                    ProjectId = projectId,
                    EntityName = entityName,
                    AttributeName = attributeName,
                    IsPrimaryKey = isPrimaryKey
                });
                return this.GetAttributeDetails(projectId, entityName, attributeName);
            }
        }

        public AttributeDetailsInfo SetAttributeConfig(int projectId, string entityName, string attributeName, AttributeConfigPayloadInfo payload)
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
                            (ProjectId, EntityName, AttributeName, IsActive)
                        SELECT
                            @ProjectId,
                            @EntityName,
                            @AttributeName,
                            @IsActive;";
                db.Execute(sql, new
                {
                    ProjectId = projectId,
                    EntityName = entityName,
                    AttributeName = attributeName,
                    IsActive = payload.IsActive
                });
                return this.GetAttributeDetails(projectId, entityName, attributeName);
            }
        }

        public AttributeDetailsInfo UnsetAttributeConfig(int projectId, string entityName, string attributeName)
        {
            using (var db = new SqlConnection(ConnectionString))
            {
                var sql = @"DELETE FROM AttributeConfig WHERE ProjectId = @ProjectId AND EntityName = @EntityName AND AttributeName = @AttributeName";
                db.Execute(sql, new
                {
                    ProjectId = projectId,
                    EntityName = entityName,
                    AttributeName = attributeName
                });
                return this.GetAttributeDetails(projectId, entityName, attributeName);
            }
        }

        /// <summary>
        /// Clears any attribute description configuration
        /// </summary>
        /// <param name="projectId">The project id</param>
        /// <param name="entityName">The entity name</param>
        /// <param name="attributeName">The attribute name</param>
        /// <returns></returns>
        public AttributeDetailsInfo UnsetAttributeDesc(int projectId, string entityName, string attributeName)
        {
            using (var db = new SqlConnection(ConnectionString))
            {
                var sql = @"
                    DECLARE @AttributeDescConfigId INT;

                    SELECT @AttributeDescConfigId = AttributeDescConfigId
                    FROM
                        AttributeDescConfig
                    WHERE
                        ProjectId = @ProjectId AND
                        EntityName = @EntityName AND
                        AttributeName = @AttributeName;

                    IF @AttributeDescConfigId IS NOT NULL
                    BEGIN
                        DELETE FROM AttributeDescConfig WHERE AttributeDescConfigId = @AttributeDescConfigId;
                    END";
                db.Execute(sql, new
                {
                    ProjectId = projectId,
                    EntityName = entityName,
                    AttributeName = attributeName
                });
                return this.GetAttributeDetails(projectId, entityName, attributeName);
            }
        }

        public AttributeDetailsInfo SetAttributeDesc(int projectId, string entityName, string attributeName, string attributeDesc, string attributeComment, int? valueGroupId)
        {
            using (var db = new SqlConnection(ConnectionString))
            {
                var sql = @"
                    DECLARE @AttributeDescConfigId INT;

                    SELECT @AttributeDescConfigId = AttributeDescConfigId
                    FROM
                        AttributeDescConfig
                    WHERE
                        ProjectId = @ProjectId AND
                        EntityName = @EntityName AND
                        AttributeName = @AttributeName;

                    IF @AttributeDescConfigId IS NOT NULL
                    BEGIN
                        DELETE FROM AttributeDescConfig WHERE AttributeDescConfigId = @AttributeDescConfigId;
                    END

                    INSERT INTO
                        AttributeDescConfig (ProjectId, EntityName, AttributeName, AttributeDesc, AttributeComment, ValueGroupId)
                    SELECT
                        @ProjectId, @EntityName, @AttributeName, @AttributeDesc, @AttributeComment, @ValueGroupId;";
                db.Execute(sql, new
                {
                    ProjectId = projectId,
                    EntityName = entityName,
                    AttributeName = attributeName,
                    AttributeDesc = attributeDesc,
                    AttributeComment = attributeComment,
                    ValueGroupId = valueGroupId
                });
                return this.GetAttributeDetails(projectId, entityName, attributeName);
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
                db.Open();
                db.Execute("DELETE FROM Attribute WHERE ProjectId = @ProjectId", new { ProjectId = project.ProjectId });
                var dt = attributes.ToDataTable();
                db.BulkCopy(dt, "Attribute");
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

        #endregion

        #region Relationships

        public void ScanRelationships(int projectId)
        {
            var relationships = ScanRelationshipsEx(projectId);
            SaveRelationships(relationships);
        }

        private IEnumerable<RelationshipScanInfo> ScanRelationshipsEx(int projectId)
        {
            var project = this.GetProject(projectId);

            using (var db = new SqlConnection(project.ConnectionString))
            {
                var relationships = db.Query<RelationshipScanInfo>(SqlGetEntityRelationships);
                // Add projectName
                foreach (var r in relationships)
                {
                    r.ProjectId = project.ProjectId;
                }
                return relationships;
            }
        }

        /// <summary>
        /// Saves a manually created relationship.
        /// </summary>
        /// <param name="relationship">The new relationship to save.</param>
        /// <returns></returns>
        public RelationshipInfo CreateRelationship(RelationshipInfo relationship)
        {
            using (var db = new SqlConnection(ConnectionString))
            {
                if (relationship.ReferencedAttributeNames.Count() < 1 &&
                    relationship.ReferencedAttributeNames.Count() != relationship.ParentAttributeNames.Count())
                {
                    throw new Exception("Relationship attributes not valid.");
                }

                var sql = @"
INSERT INTO Relationship
    (ProjectId, RelationshipName, ParentEntityName, ReferencedEntityName, IsScanned)
SELECT
    @ProjectId, @RelationshipName, @ParentEntityName, @ReferencedEntityName, @IsScanned;
SELECT
    *
FROM
    Relationship
WHERE
    RelationshipId = SCOPE_IDENTITY();";
                var newRel = db.Query<RelationshipInfo>(sql, new
                {
                    ProjectId = relationship.ProjectId,
                    RelationshipName = relationship.RelationshipName,
                    ParentEntityName = relationship.ParentEntityName,
                    ReferencedEntityName = relationship.ReferencedEntityName,
                    IsScanned = false
                }).First();

                // attributes
                for (int i = 0; i < relationship.ParentAttributeNames.Count(); i++)
                {
                    sql = @"
INSERT INTO RelationshipAttribute
    (RelationshipId, ParentAttributeName, ReferencedAttributeName)
SELECT
    @RelationshipId, @ParentAttributeName, @ReferencedAttributeName";
                    db.Execute(sql, new
                    {
                        RelationshipId = newRel.RelationshipId,
                        ParentAttributeName = relationship.ParentAttributeNames[i],
                        ReferencedAttributeName = relationship.ReferencedAttributeNames[i]
                    });
                }

                return this.GetRelationship(newRel.RelationshipId.Value);
            }
        }

        private void SaveRelationships(IEnumerable<RelationshipScanInfo> relationships)
        {
            using (var db = new SqlConnection(ConnectionString))
            {
                db.Open();

                // Get unique header records
                var headers = relationships.Select(r => new
                {
                    ProjectId = r.ProjectId,
                    RelationshipName = r.RelationshipName,
                    ParentEntityName = r.ParentEntityName,
                    ReferencedEntityName = r.ReferencedEntityName,
                    IsScanned = true
                }).Distinct();

                foreach (var item in headers)
                {
                    // Insert header + get id
                    var sql = @"
INSERT INTO Relationship
    (ProjectId, RelationshipName, ParentEntityName, ReferencedEntityName, IsScanned)
SELECT
    @ProjectId, @RelationshipName, @ParentEntityName, @ReferencedEntityName, @IsScanned;
SELECT
    SCOPE_IDENTITY();";

                    var newId = db.Query<int>(sql, new
                    {
                        ProjectId = item.ProjectId,
                        RelationshipName = item.RelationshipName,
                        ParentEntityName = item.ParentEntityName,
                        ReferencedEntityName = item.ReferencedEntityName,
                        IsScanned = item.IsScanned
                    });

                    // Do items / attributes
                    var attr = relationships
                        .Where(r => r.ProjectId == item.ProjectId)
                        .Where(r => r.RelationshipName == item.RelationshipName)
                        .Where(r => r.ParentEntityName == item.ParentEntityName)
                        .Where(r => r.ReferencedEntityName == item.ReferencedEntityName);

                    foreach (var attrItem in attr)
                    {
                        var sqlAttr = @"
INSERT INTO RelationshipAttribute
    (RelationshipId, ParentAttributeName, ReferencedAttributeName)
SELECT
    @RelationshipId, @ParentAttributeName, @ReferencedAttributeName";
                        db.Execute(sqlAttr, new
                        {
                            RelationshipId = newId,
                            ParentAttributeName = attrItem.ParentAttributeName,
                            ReferencedAttributeName = attrItem.ReferencedAttributeName
                        });
                    }
                }
            }
        }

        /// <summary>
        /// Deletes a relationship.
        /// </summary>
        /// <param name="id">The RelationshipId.</param>
        public void DeleteRelationship(int id)
        {
            using (var db = new SqlConnection(ConnectionString))
            {
                db.Execute(@"
DELETE FROM RelationshipAttribute WHERE RelationshipId = @RelationshipId;
DELETE FROM Relationship WHERE RelationshipId = @RelationshipId;",
                    new { RelationshipId = id });
            }
        }

        public IEnumerable<RelationshipScanInfo> GetRelationships(ProjectInfo project)
        {
            using (var db = new SqlConnection(ConnectionString))
            {
                var sql = @"
SELECT
	R.ProjectId,
	R.RelationshipName,
	R.ParentEntityName,
	R.ReferencedEntityName,
	RA.ParentAttributeName,
	RA.ReferencedAttributeName
FROM
	Relationship R
INNER JOIN
	RelationshipAttribute RA
ON
	R.RelationshipId = RA.RelationshipId
WHERE
    R.ProjectId = @ProjectId
";
                var data = db.Query<RelationshipScanInfo>(sql, new { ProjectId = project.ProjectId });
                return data;
            }
        }

        public IEnumerable<RelationshipInfo> GetRelationships(int projectId)
        {
            using (var db = new SqlConnection(ConnectionString))
            {
                var relationships = db.Query<RelationshipInfo>("SELECT * FROM RELATIONSHIP WHERE ProjectId = @ProjectId", new
                {
                    ProjectId = projectId
                });

                foreach (var relationship in relationships)
                {
                    // Get attributes
                    var attributes = db.Query<RelationshipAttributeInfo>("SELECT * FROM RelationshipAttribute WHERE RelationshipId = @RelationshipId", new
                    {
                        RelationshipId = relationship.RelationshipId
                    });

                    relationship.ParentAttributeNames = attributes.Select(a => a.ParentAttributeName).ToList();
                    relationship.ReferencedAttributeNames = attributes.Select(a => a.ReferencedAttributeName).ToList();
                }
                return relationships;
            }
        }

        public RelationshipInfo GetRelationship(int relationshipId)
        {
            using (var db = new SqlConnection(ConnectionString))
            {
                var relationships = db.Query<RelationshipInfo>("SELECT * FROM Relationship WHERE RelationshipId = @RelationshipId", new
                {
                    RelationshipId = relationshipId
                });

                foreach (var relationship in relationships)
                {
                    // Get attributes
                    var attributes = db.Query<RelationshipAttributeInfo>("SELECT * FROM RelationshipAttribute WHERE RelationshipId = @RelationshipId", new
                    {
                        RelationshipId = relationship.RelationshipId
                    });

                    relationship.ParentAttributeNames = attributes.Select(a => a.ParentAttributeName).ToList();
                    relationship.ReferencedAttributeNames = attributes.Select(a => a.ReferencedAttributeName).ToList();
                }
                return relationships.First();
            }
        }

        #endregion

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

        #region Values & Value Groups

        /// <summary>
        /// Gets value groups for a project
        /// </summary>
        /// <param name="projectId">The project id.</param>
        /// <returns></returns>
        public IEnumerable<ValueGroupInfo> GetValueGroups(int projectId)
        {
            using (var db = new SqlConnection(ConnectionString))
            {
                db.Open();
                return db.Query<ValueGroupInfo>("SELECT * FROM ValueGroup WHERE ProjectId = @ProjectId", new { ProjectId = projectId });
            }
        }

        /// <summary>
        /// Gets a single value groups
        /// </summary>
        /// <param name="valueGroupId">The value group id.</param>
        /// <returns></returns>
        public ValueGroupInfo GetValueGroup(int valueGroupId)
        {
            using (var db = new SqlConnection(ConnectionString))
            {
                db.Open();
                return db.Query<ValueGroupInfo>("SELECT * FROM ValueGroup WHERE ValueGroupId = @ValueGroupId", new { ValueGroupId = valueGroupId }).First();
            }
        }


        public ValueGroupInfo CreateValueGroup(ValueGroupInfo valueGroup)
        {
            using (var db = new SqlConnection(ConnectionString))
            {
                var now = DateTime.Now;

                var newValueGroup = db.Query<ValueGroupInfo>(@"
INSERT INTO ValueGroup (
    ProjectId, ValueGroupName )
SELECT
    @ProjectId, @ValueGroupName;
SELECT * FROM ValueGroup WHERE ValueGroupId = SCOPE_IDENTITY();", new
                {
                    ProjectId = valueGroup.ProjectId,
                    ValueGroupName = valueGroup.ValueGroupName
                });
                return newValueGroup.First();
            }
        }

        public void UpdateValueGroup(int id, ValueGroupInfo valueGroup)
        {
            if (id != valueGroup.ValueGroupId)
            {
                throw new Exception("Invalid value group id");
            }

            using (var db = new SqlConnection(ConnectionString))
            {
                var newProject = db.Execute(@"
UPDATE
    ValueGroup
SET
    ProjectId = @ProjectId
    ValueGroupName = @ValueGroupName
WHERE
    ValueGroupId = @ValueGroupId;", new
                {
                    ValueGroupId = valueGroup.ValueGroupId,
                    ProjectId = valueGroup.ProjectId,
                    ValueGroupName = valueGroup.ValueGroupName
                });
            }
        }

        public void DeleteValueGroup(int valueGroupId)
        {
            using (var db = new SqlConnection(ConnectionString))
            {
                var newTask = db.Query<ProjectInfo>(@"
DELETE FROM
    ValueGroup
WHERE
    ValueGroupId = @ValueGroupId;", new
                {
                    ValueGroupId = valueGroupId
                });
            }
        }

        #endregion

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
SELECT '*', '*', NULL, NULL, NULL, NULL, NULL";

        #endregion

    }
}