using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using data_doc_api.Models;
using System.Collections.Generic;
using Dapper;
using System.Data.SqlClient;
using System;

namespace data_doc_api.Controllers
{
    /// <summary>
    /// Provides services to manage entities (tables, views etc.) within a project
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class EntityController : ControllerBase
    {
        private string ConnectionString { get; set; }
        private MetadataRepository MetadataRepository { get; set; }

        /// <summary>
        /// Constructor for EntityController class.
        /// </summary>
        /// <param name="connectionStrings">Connection string.</param>
        public EntityController(IOptions<ConnectionStringConfig> connectionStrings)
        {
            this.ConnectionString = connectionStrings.Value.DataDoc;
            this.MetadataRepository = MetadataRepository.Connect(ConnectionString);
        }

        /// <summary>
        /// Gets a list of entity details for a project
        /// </summary>
        /// <param name="projectId">The project id</param>
        /// <returns>The list of entities</returns>
        [HttpGet("/Entities/{projectId}")]
        public ActionResult<IEnumerable<EntityDetailsInfo>> GetEntities(int projectId)
        {
            return Ok(MetadataRepository.GetEntityDetails(projectId));
        }

        /// <summary>
        /// Gets the details for a single entity
        /// </summary>
        /// <param name="projectId">The project id</param>
        /// <param name="entityName">The entity name</param>
        /// <returns>The selected entity</returns>
        [HttpGet("/Entities/{projectId}/{entityName}")]
        public ActionResult<EntityDetailsInfo> GetEntity(int projectId, string entityName)
        {
            return Ok(MetadataRepository.GetEntityDetail(projectId, entityName));
        }

        /// <summary>
        /// Sets the configuration for an entity
        /// </summary>
        /// <param name="projectId">The project id</param>
        /// <param name="entityName">The entity name</param>
        /// <param name="payload">The configuration payload</param>
        /// <returns>The updated entity</returns>
        [HttpPatch("/Entities/{projectId}/{entityName}")]
        public ActionResult<EntityDetailsInfo> SetEntityConfig(int projectId, string entityName, [FromBody] EntityConfigPayloadInfo payload)
        {
            var result = MetadataRepository.SetEntityConfig(projectId, entityName, payload);
            return Ok(result);
        }

        /// <summary>
        /// Unsets the configuration for an entity
        /// </summary>
        /// <param name="projectId">The project id</param>
        /// <param name="entityName">The entity name</param>
        /// <returns>The updated entity</returns>
        [HttpDelete("/Entities/{projectId}/{entityName}")]
        public ActionResult UnsetEntityConfig(int projectId, string entityName)
        {
            var result = MetadataRepository.UnsetEntityConfig(projectId, entityName);
            return Ok(result);
        }

        /// <summary>
        /// Scans an entity to detect attribute hierarchies
        /// </summary>
        /// <param name="projectId">The project id</param>
        /// <param name="entityName">The entity name</param>
        /// <returns></returns>
        [HttpPut("/Entities/Hierarchies/{projectId}/{entityName}")]
        public ActionResult ScanHierarchies(int projectId, string entityName)
        {
            MetadataRepository.ScanAttributeHierarchies(projectId, entityName);
            return NoContent();
        }

        /// <summary>
        /// Scans an entity to detect attribute hierarchies
        /// </summary>
        /// <param name="projectId">The project id</param>
        /// <param name="entityName">The entity name</param>
        /// <returns></returns>
        [HttpDelete("/Entities/Hierarchies/{projectId}/{entityName}")]
        public ActionResult DeleteHierarchies(int projectId, string entityName)
        {
            MetadataRepository.DeleteAttributeHierarchies(projectId, entityName);
            return NoContent();
        }

        /// <summary>
        /// Scans an entity to detect attribute hierarchies
        /// </summary>
        /// <param name="projectId">The project id</param>
        /// <param name="entityName">The entity name</param>
        /// <returns></returns>
        [HttpGet("/Entities/Hierarchies/{projectId}/{entityName}")]
        public ActionResult<IEnumerable<AttributeHierarchyInfo>> GetHierarchies(int projectId, string entityName)
        {
            return Ok(MetadataRepository.GetAttributeHierarchies(projectId, entityName));
        }
    }
}
