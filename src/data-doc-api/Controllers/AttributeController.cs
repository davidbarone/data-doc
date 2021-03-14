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
    public class AttributeController : ControllerBase
    {
        private string ConnectionString { get; set; }
        private MetadataRepository MetadataRepository { get; set; }

        /// <summary>
        /// Constructor for ProjectController class.
        /// </summary>
        /// <param name="connectionStrings">Connection string.</param>
        public AttributeController(IOptions<ConnectionStringConfig> connectionStrings)
        {
            this.ConnectionString = connectionStrings.Value.DataDoc;
            this.MetadataRepository = MetadataRepository.Connect(ConnectionString);
        }

        /// <summary>
        /// Gets a list of all attributes for a project.
        /// </summary>
        /// <returns>The list of entities for the selected project.</returns>
        [HttpGet("/Attributes")]
        public ActionResult<IEnumerable<AttributeInfo>> GetAttributes(int projectId)
        {
            return Ok(MetadataRepository.GetAttributes(projectId));
        }

        /// <summary>
        /// Gets a list of all attribute configuration for a project.
        /// </summary>
        /// <returns>The list of attribute configuration for the selected project.</returns>
        [HttpGet("/Attributes/Config")]
        public ActionResult<IEnumerable<AttributeConfigScopedInfo>> GetAttributesConfig(int projectId)
        {
            return Ok(MetadataRepository.GetAttributesConfig(projectId));
        }

        /// <summary>
        /// Sets the configuration for an attribute.
        /// </summary>
        /// <param name="entityConfig">AttributeConfig object containing the configuration.</param>
        /// <returns></returns>
        [HttpPut("/Attributes/Config")]
        public ActionResult<AttributeConfigInfo> SetAttributeConfig(AttributeConfigInfo attributeConfig)
        {
            var result = MetadataRepository.SetAttributeConfig(attributeConfig);
            return Ok(result);
        }

        /// <summary>
        /// Unsets the configuration for an attribute.
        /// </summary>
        /// <param name="id">Unique id of the configuration record to delete.</param>
        /// <returns></returns>
        [HttpDelete("/Attributes/Config")]
        public ActionResult UnsetAttributeConfig(int id)
        {
            MetadataRepository.UnsetEntityConfig(id);
            return NoContent();
        }
    }
}
