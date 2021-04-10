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
    /// Provides metadata services for individual values within an entity.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class ValueController : ControllerBase
    {
        private string ConnectionString { get; set; }
        private MetadataRepository MetadataRepository { get; set; }

        /// <summary>
        /// Constructor for ProjectController class
        /// </summary>
        /// <param name="connectionStrings">The connection string</param>
        public ValueController(IOptions<ConnectionStringConfig> connectionStrings)
        {
            this.ConnectionString = connectionStrings.Value.DataDoc;
            this.MetadataRepository = MetadataRepository.Connect(ConnectionString);
        }

        /// <summary>
        /// Gets a list of all value groups for a project
        /// </summary>
        /// <param name="projectId">The project id</param>
        /// <returns>The list of projects</returns>
        [HttpGet("/ValueGroups/{projectId}")]
        public ActionResult<IEnumerable<ValueGroupInfo>> Get(int projectId)
        {
            return Ok(MetadataRepository.GetValueGroups(projectId));
        }

        /// <summary>
        /// Gets a single value group by id
        /// </summary>
        /// <param name="valueGroupId">The project id</param>
        /// <returns></returns>
        [HttpGet("/ValueGroups/Single/{valueGroupId}")]
        public ActionResult<ValueGroupInfo> GetOne(int valueGroupId)
        {
            return Ok(MetadataRepository.GetValueGroup(valueGroupId));
        }

        /// <summary>
        /// Creates a new value group
        /// </summary>
        /// <param name="valueGroup">The new value group</param>
        /// <returns></returns>
        [HttpPost("/ValueGroups")]
        public ActionResult<ValueGroupInfo> Create(ValueGroupInfo valueGroup)
        {
            return Ok(MetadataRepository.CreateValueGroup(valueGroup));
        }

        /// <summary>
        /// Updates an existing value group
        /// </summary>
        /// <param name="valueGroupId">The value group id</param>
        /// <param name="valueGroup">The updated value group</param>
        /// <returns></returns>
        [HttpPut("/ValueGroups/{valueGroupId}")]
        public ActionResult Update(int valueGroupId, [FromBody] ValueGroupInfo valueGroup)
        {
            MetadataRepository.UpdateValueGroup(valueGroupId, valueGroup);
            return NoContent();
        }

        /// <summary>
        /// Deletes an existing value group
        /// </summary>
        /// <param name="valueGroupId">The project id</param>
        /// <returns>No content</returns>
        [HttpDelete("/ValueGroups/{valueGroupId}")]
        public ActionResult Delete(int valueGroupId)
        {
            MetadataRepository.DeleteValueGroup(valueGroupId);
            return NoContent();
        }
    }
}
