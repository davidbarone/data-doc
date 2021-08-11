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
    /// Provides services to manage attributes within a project
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class AttributeController : ControllerBase
    {
        private string ConnectionString { get; set; }
        private MetadataRepository MetadataRepository { get; set; }

        /// <summary>
        /// Constructor for AttributeController class
        /// </summary>
        /// <param name="connectionStrings">Connection string.</param>
        public AttributeController(IOptions<ConnectionStringConfig> connectionStrings)
        {
            this.ConnectionString = connectionStrings.Value.DataDoc;
            this.MetadataRepository = MetadataRepository.Connect(ConnectionString);
        }

        /// <summary>
        /// Gets a list of all attribute details for a project
        /// </summary>
        /// <param name="projectId">The project id</param>
        /// <returns>The list of attribute details for the selected project</returns>
        [HttpGet("/Attributes/{projectId}")]
        public ActionResult<IEnumerable<AttributeDetailsInfo>> GetAttributeDetails(int projectId)
        {
            return Ok(MetadataRepository.GetAttributeDetails(projectId));
        }

        /// <summary>
        /// Gets a list of all attribute details for an entity in a project
        /// </summary>
        /// <param name="projectId">The project id</param>
        /// <param name="entityName">The entity name</param>
        /// <returns>The list of attribute details for the selected project and entity</returns>
        [HttpGet("/Attributes/{projectId}/{entityName}")]
        public ActionResult<IEnumerable<AttributeDetailsInfo>> GetAttributeDetails(int projectId, string entityName)
        {
            return Ok(
                MetadataRepository.GetAttributeDetails(projectId)
                .Where(e => e.EntityName.Equals(entityName, StringComparison.OrdinalIgnoreCase)));
        }

        /// <summary>
        /// Gets a list of all attribute details for a project matching a name
        /// </summary>
        /// <param name="projectId">The project id</param>
        /// <param name="attributeName">The attribute name</param>
        /// <returns>The list of attribute details for the selected project and entity</returns>
        [HttpGet("/Attributes/search/{projectId}/{attributeName}")]
        public ActionResult<IEnumerable<AttributeDetailsInfo>> SearchAttributeDetails(int projectId, string attributeName)
        {
            return Ok(
                MetadataRepository.GetAttributeDetails(projectId)
                .Where(e => e.AttributeName.Equals(attributeName, StringComparison.OrdinalIgnoreCase)));
        }

        /// <summary>
        /// Gets a single attribute for an entity in a project
        /// </summary>
        /// <param name="projectId">The project id</param>
        /// <param name="entityName">The entity name</param>
        /// <param name="attributeName">The attribute name</param>
        /// <returns>The selected attribute details</returns>
        [HttpGet("/Attributes/{projectId}/{entityName}/{attributeName}")]
        public ActionResult<AttributeDetailsInfo> GetAttributeDetails(int projectId, string entityName, string attributeName)
        {
            return Ok(
                MetadataRepository.GetAttributeDetails(projectId)
                .Where(e => e.EntityName.Equals(entityName, StringComparison.OrdinalIgnoreCase))
                .Where(e => e.AttributeName.Equals(attributeName, StringComparison.OrdinalIgnoreCase))
                .FirstOrDefault());
        }

        /// <summary>
        /// Sets the configuration for an attribute
        /// </summary>
        /// <param name="projectId">The project id</param>
        /// <param name="entityName">The entity name</param>
        /// <param name="attributeName">The attribute name</param>
        /// <param name="payload">The configuration for the attribute</param>
        /// <returns>The updated attribute details</returns>
        [HttpPatch("/Attributes/Config/{projectId}/{entityName}/{attributeName}")]
        public ActionResult<AttributeDetailsInfo> SetAttributeConfig(int projectId, string entityName, string attributeName, [FromBody] AttributeConfigPayloadInfo payload)
        {
            var result = MetadataRepository.SetAttributeConfig(projectId, entityName, attributeName, payload);
            return Ok(result);
        }

        /// <summary>
        /// Clears the configuration for an attribute
        /// </summary>
        /// <param name="projectId">The project id</param>
        /// <param name="entityName">The entity name</param>
        /// <param name="attributeName">The attribute name</param>
        /// <returns>The updated attribute details</returns>
        [HttpDelete("/Attributes/Config/{projectId}/{entityName}/{attributeName}")]
        public ActionResult<AttributeDetailsInfo> UnsetAttributeConfig(int projectId, string entityName, string attributeName)
        {
            var result = MetadataRepository.UnsetAttributeConfig(projectId, entityName, attributeName);
            return Ok(result);
        }

        /// <summary>
        /// Sets attribute's description and comment
        /// </summary>
        /// <param name="projectId">The project id</param>
        /// <param name="entityName">The entity name</param>
        /// <param name="attributeName">The attribute name</param>
        /// <param name="scope">The scope of the description. Either 'Local', 'Project', or 'Global'. Default is 'Local'</param>
        /// <param name="payload">The payload</param>
        /// <returns>The updated attribute details</returns>
        [HttpPatch("/Attributes/Desc/{projectId}/{entityName}/{attributeName}/{scope}")]
        public ActionResult<AttributeDetailsInfo> SetAttributeDescConfig(int projectId, string entityName, string attributeName, DescriptionScope scope, [FromBody] AttributeDescConfigPayloadInfo payload)
        {
            var result = MetadataRepository.SetAttributeDesc(projectId, entityName, attributeName, scope, payload.AttributeDesc, payload.AttributeComment, payload.ValueGroupId);
            return Ok(result);
        }

        /// <summary>
        /// Clears the attribute's description and comment
        /// </summary>
        /// <param name="projectId">The project id</param>
        /// <param name="entityName">The entity name</param>
        /// <param name="attributeName">The attribute name</param>
        /// <param name="scope">The scope of the description. Either 'Local', 'Project', or 'Global'. Default is 'Local'</param>
        /// <returns>The updated attribute details</returns>
        [HttpDelete("/Attributes/Desc/{projectId}/{entityName}/{attributeName}/{scope}")]
        public ActionResult<AttributeDetailsInfo> UnsetAttributeDescConfig(int projectId, string entityName, string attributeName, DescriptionScope scope)
        {
            var result = MetadataRepository.UnsetAttributeDesc(projectId, entityName, attributeName, scope);
            return Ok(result);
        }

        /// <summary>
        /// Sets attribute's primary key status
        /// </summary>
        /// <param name="projectId">The project id</param>
        /// <param name="entityName">The entity name</param>
        /// <param name="attributeName">The attribute name</param>
        /// <param name="payload">The payload</param>
        /// <returns>The updated attribute details</returns>
        [HttpPatch("/Attributes/PrimaryKey/{projectId}/{entityName}/{attributeName}")]
        public ActionResult<AttributeDetailsInfo> SetAttributePrimaryKeyConfig(int projectId, string entityName, string attributeName, [FromBody] AttributePrimaryKeyConfigPayloadInfo payload)
        {
            var result = MetadataRepository.SetAttributePrimaryKey(projectId, entityName, attributeName, payload.IsPrimaryKey);
            return Ok(result);
        }

        /// <summary>
        /// Clears the attribute's primary key status
        /// </summary>
        /// <param name="projectId">The project id</param>
        /// <param name="entityName">The entity name</param>
        /// <param name="attributeName">The attribute name</param>
        /// <returns>The updated attribute details</returns>
        [HttpDelete("/Attributes/PrimaryKey/{projectId}/{entityName}/{attributeName}")]
        public ActionResult<AttributeDetailsInfo> UnsetAttributePrimaryKeyConfig(int projectId, string entityName, string attributeName)
        {
            var result = MetadataRepository.UnsetAttributePrimaryKey(projectId, entityName, attributeName);
            return Ok(result);
        }
    }
}
