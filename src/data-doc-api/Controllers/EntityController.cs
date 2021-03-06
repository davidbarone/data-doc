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

        /// <summary>
        /// Constructor for ProjectController class.
        /// </summary>
        /// <param name="connectionStrings">Connection string.</param>
        public EntityController(IOptions<ConnectionStringConfig> connectionStrings)
        {
            this.ConnectionString = connectionStrings.Value.DataDoc;
        }

        /// <summary>
        /// Gets a list of all entities for a project.
        /// </summary>
        /// <returns>The list of projects.</returns>
        [HttpGet("/Entities")]
        public ActionResult<IEnumerable<ProjectInfo>> GetAll(int projectId)
        {
            using (var db = new SqlConnection(ConnectionString))
            {
                var projects = db.Query<ProjectInfo>("SELECT * FROM PROJECT WHERE ISACTIVE = 1");
                return Ok(projects);
            }
        }



    }
}
