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
    /// Provides metadata services for individual values for an attribute.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class ValueController : ControllerBase
    {
        private string ConnectionString { get; set; }
        private MetadataRepository MetadataRepository { get; set; }

        /// <summary>
        /// Constructor for the ValueController class
        /// </summary>
        /// <param name="connectionStrings">The connection string</param>
        public ValueController(IOptions<ConnectionStringConfig> connectionStrings)
        {
            this.ConnectionString = connectionStrings.Value.DataDoc;
            this.MetadataRepository = MetadataRepository.Connect(ConnectionString);
        }

        #region Values

        /// <summary>
        /// Scans the source database for distinct non-null values and adds to the values table
        /// </summary>
        /// <param name="valueGroupId"></param>
        /// <param name="projectId"></param>
        /// <param name="entityName"></param>
        /// <param name="attributeName"></param>
        /// <returns>No content</returns>
        [HttpPut("/Values/{valueGroupId}/{projectId}/{entityName}/{attributeName}")]
        public ActionResult ScanValues(int valueGroupId, int projectId, string entityName, string attributeName)
        {
            var attribute = MetadataRepository.GetAttributeDetails(projectId, entityName, attributeName);
            MetadataRepository.ScanValues(valueGroupId, attribute);
            return NoContent();
        }

        /// <summary>
        /// Gets a list of all values for a value group
        /// </summary>
        /// <param name="valueGroupId">The value group id</param>
        /// <returns>The list of values</returns>
        [HttpGet("/Values/{valueGroupId}")]
        public ActionResult<IEnumerable<ValueInfo>> Get(int valueGroupId)
        {
            return Ok(MetadataRepository.GetValues(valueGroupId));
        }

        /// <summary>
        /// Gets a single value by id
        /// </summary>
        /// <param name="valueId">The value id</param>
        /// <returns></returns>
        [HttpGet("/Values/Single/{valueId}")]
        public ActionResult<ValueInfo> GetOne(int valueId)
        {
            return Ok(MetadataRepository.GetValue(valueId));
        }

        /// <summary>
        /// Creates a new value
        /// </summary>
        /// <param name="value"></param>
        /// <returns>The new value</returns>
        [HttpPost("/Values")]
        public ActionResult<ValueGroupInfo> CreateValue([FromBody] ValueInfo value)
        {
            return Ok(MetadataRepository.CreateValue(value));
        }

        /// <summary>
        /// Updates an existing value
        /// </summary>
        /// <param name="valueId">The value id</param>
        /// <param name="value">The updated value</param>
        /// <returns>The updated value</returns>
        [HttpPut("/Values/{valueId}")]
        public ActionResult UpdateValue(int valueId, [FromBody] ValueInfo value)
        {
            MetadataRepository.UpdateValue(valueId, value);
            return NoContent();
        }

        /// <summary>
        /// Deletes an existing value
        /// </summary>
        /// <param name="valueId">The value id</param>
        /// <returns>No content</returns>
        [HttpDelete("/Values/{valueId}")]
        public ActionResult DeleteValue(int valueId)
        {
            MetadataRepository.DeleteValue(valueId);
            return NoContent();
        }

        #endregion

    }
}
