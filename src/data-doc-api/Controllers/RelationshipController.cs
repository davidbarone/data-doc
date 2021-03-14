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
    /// Provides services to manage relationships within a project.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class RelationshipController : ControllerBase
    {
        private string ConnectionString { get; set; }
        private MetadataRepository MetadataRepository { get; set; }


        /// <summary>
        /// Constructor for RelationshipController class.
        /// </summary>
        /// <param name="connectionStrings">Connection string.</param>
        public RelationshipController(IOptions<ConnectionStringConfig> connectionStrings)
        {
            this.ConnectionString = connectionStrings.Value.DataDoc;
            this.MetadataRepository = MetadataRepository.Connect(ConnectionString);
        }

        /// <summary>
        /// Automatically scans the source database for any physical relationships.
        /// </summary>
        /// <param name="id">The project id to scan</param>
        /// <returns></returns>
        [HttpPut("/Relationships/Scan/{id}")]
        public ActionResult Scan(int id)
        {
            MetadataRepository.ScanRelationships(id);
            return NoContent();
        }

        /// <summary>
        /// Deletes a relationship.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("/Relationships/{id}")]
        public ActionResult DeleteRelationship(int id)
        {
            MetadataRepository.DeleteRelationship(id);
            return NoContent();
        }

        /// <summary>
        /// Gets a list of all relationships for a project.
        /// </summary>
        /// <returns>The list of projects.</returns>
        [HttpGet("/Relationships")]
        public ActionResult<IEnumerable<RelationshipInfo>> GetAll(int projectId)
        {
            return Ok(MetadataRepository.GetRelationships(projectId));
        }

        [HttpPost("/Relationships")]
        public ActionResult<RelationshipInfo> CreateRelationship(RelationshipInfo relationship)
        {
            return Ok(MetadataRepository.CreateRelationship(relationship));
        }

        [HttpPut("/Relationships/{relationshipId}")]
        public ActionResult<RelationshipInfo> UpdateRelationship(int relationshipId, [FromBody] RelationshipInfo relationship)
        {
            if (relationship.RelationshipId != relationshipId)
            {
                throw new Exception("Invalid RelationshipId");
            }
            MetadataRepository.DeleteRelationship(relationshipId);
            return Ok(MetadataRepository.CreateRelationship(relationship));
        }
    }
}
