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

        /// <summary>
        /// Constructor for RelationshipController class.
        /// </summary>
        /// <param name="connectionStrings">Connection string.</param>
        public RelationshipController(IOptions<ConnectionStringConfig> connectionStrings)
        {
            this.ConnectionString = connectionStrings.Value.DataDoc;
        }

        /// <summary>
        /// Scans the database for the project, and caches a list of all the object metadata.
        /// </summary>
        /// <param name="id">The project id to scan</param>
        /// <returns></returns>
        [HttpPut("/Relationships/Scan/{id}")]
        public ActionResult Scan(int id)
        {
            var mr = MetadataRepository.Connect(ConnectionString);
            var project = mr.GetProjects().First(c => c.ProjectId == id);

            // Scan Entities
            mr.DeleteRelationships(project);
            mr.ScanRelationships(project);

            return NoContent();
        }

        /// <summary>
        /// Deletes the physical relationships (foreign key constraints) for a project.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("/Relationships/{id}")]
        public ActionResult DeleteRelationships(int id)
        {
            var mr = MetadataRepository.Connect(ConnectionString);
            var project = mr.GetProjects().First(c => c.ProjectId == id);

            // Scan Entities
            mr.DeleteRelationships(project);
            return NoContent();
        }


        /// <summary>
        /// Gets a list of all relationships for a project.
        /// </summary>
        /// <returns>The list of projects.</returns>
        [HttpGet("/Relationships")]
        public ActionResult<IEnumerable<RelationshipInfo>> GetAll(int projectId)
        {
            using (var db = new SqlConnection(ConnectionString))
            {
                var relationships = db.Query<RelationshipInfo>("SELECT * FROM RELATIONSHIP WHERE ProjectId = @ProjectId", new
                {
                    ProjectId = projectId
                });
                return Ok(relationships);
            }
        }



    }
}
