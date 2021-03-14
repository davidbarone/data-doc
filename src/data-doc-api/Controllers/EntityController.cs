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
    /// Provides services to manage entities (tables, objects) within a project.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class EntityController : ControllerBase
    {
        private string ConnectionString { get; set; }
        private MetadataRepository MetadataRepository { get; set; }

        /// <summary>
        /// Constructor for ProjectController class.
        /// </summary>
        /// <param name="connectionStrings">Connection string.</param>
        public EntityController(IOptions<ConnectionStringConfig> connectionStrings)
        {
            this.ConnectionString = connectionStrings.Value.DataDoc;
            this.MetadataRepository = MetadataRepository.Connect(ConnectionString);
        }

        /// <summary>
        /// Gets a list of all entities for a project.
        /// </summary>
        /// <returns>The list of entities for the selected project.</returns>
        [HttpGet("/Entities")]
        public ActionResult<IEnumerable<EntityInfo>> GetEntities(int projectId)
        {
            return Ok(MetadataRepository.GetEntities(projectId));
        }

        /// <summary>
        /// Gets a list of all entities configuration for a project.
        /// </summary>
        /// <returns>The list of entity configuration for the selected project.</returns>
        [HttpGet("/Entities/Config")]
        public ActionResult<IEnumerable<EntityInfo>> GetEntitiesConfig(int projectId)
        {
            return Ok(MetadataRepository.GetEntitiesConfig(projectId));
        }

        /// <summary>
        /// Sets the configuration for an entity.
        /// </summary>
        /// <param name="entityConfig">EntityConfig object containing the configuration.</param>
        /// <returns></returns>
        [HttpPut("/Entities/Config")]
        public ActionResult<EntityDependencyInfo> SetEntityConfig(EntityConfigInfo entityConfig)
        {
            var result = MetadataRepository.SetEntityConfig(entityConfig);
            return Ok(result);
        }

        /// <summary>
        /// Unsets the configuration for an entity.
        /// </summary>
        /// <param name="id">Unique id of the configuration record to delete.</param>
        /// <returns></returns>
        [HttpDelete("/Entities/Config")]
        public ActionResult<EntityDependencyInfo> UnsetEntityConfig(int id)
        {
            MetadataRepository.UnsetEntityConfig(id);
            return NoContent();
        }
    }
}
