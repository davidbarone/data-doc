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
    /// Provides services to manage relationships within a project
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class RelationshipController : ControllerBase
    {
        private string ConnectionString { get; set; }
        private MetadataRepository MetadataRepository { get; set; }


        /// <summary>
        /// Constructor for RelationshipController class
        /// </summary>
        /// <param name="connectionStrings">The connection string</param>
        public RelationshipController(IOptions<ConnectionStringConfig> connectionStrings)
        {
            this.ConnectionString = connectionStrings.Value.DataDoc;
            this.MetadataRepository = MetadataRepository.Connect(ConnectionString);
        }

        /// <summary>
        /// Gets a list of all relationships for a project
        /// </summary>
        /// <param name="projectId">The project id</param>
        /// <returns>A list of relationships</returns>
        [HttpGet("/Relationships/{projectId}")]
        public ActionResult<IEnumerable<RelationshipInfo>> GetAll(int projectId)
        {
            return Ok(MetadataRepository.GetRelationships(projectId));
        }

        /// <summary>
        /// Gets a single relationship by relationshipId
        /// </summary>
        /// <param name="relationshipId">The project id</param>
        /// <returns>The selected relationship</returns>
        [HttpGet("/Relationships/Single/{relationshipId}")]
        public ActionResult<RelationshipInfo> GetSingle(int relationshipId)
        {
            return Ok(MetadataRepository.GetRelationship(relationshipId));
        }

        /// <summary>
        /// Creates a new relationship
        /// </summary>
        /// <param name="relationship">The relationship to create</param>
        /// <returns>The new relationship</returns>
        [HttpPost("/Relationships")]
        public ActionResult<RelationshipInfo> CreateRelationship(RelationshipInfo relationship)
        {
            return Ok(MetadataRepository.CreateRelationship(relationship));
        }

        /// <summary>
        /// Updates an existing relationship
        /// </summary>
        /// <param name="relationshipId">The relationship id</param>
        /// <param name="relationship">The updated relationship</param>
        /// <returns>The updated relationship</returns>
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

        /// <summary>
        /// Deletes an existing relationship
        /// </summary>
        /// <param name="relationshipId">The relationship id</param>
        /// <returns>No content</returns>
        [HttpDelete("/Relationships/{relationshipId}")]
        public ActionResult DeleteRelationship(int relationshipId)
        {
            MetadataRepository.DeleteRelationship(relationshipId);
            return NoContent();
        }

        /// <summary>
        /// Automatically scans the source database for any physical relationships
        /// </summary>
        /// <param name="projectId">The project id</param>
        /// <returns>No content</returns>
        [HttpPut("/Relationships/Scan/{projectId}")]
        public ActionResult Scan(int projectId)
        {
            MetadataRepository.ScanRelationships(projectId);
            return NoContent();
        }
    }
}
