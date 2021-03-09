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
    /// Provides services to manage projects.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class ProjectController : ControllerBase
    {
        private string ConnectionString { get; set; }
        private MetadataRepository MetadataRepository { get; set; }

        /// <summary>
        /// Constructor for ProjectController class.
        /// </summary>
        /// <param name="connectionStrings">Connection string.</param>
        public ProjectController(IOptions<ConnectionStringConfig> connectionStrings)
        {
            this.ConnectionString = connectionStrings.Value.DataDoc;
            this.MetadataRepository = MetadataRepository.Connect(ConnectionString);
        }

        /// <summary>
        /// Gets a list of all active projects.
        /// </summary>
        /// <returns>The list of projects.</returns>
        [HttpGet("/Projects")]
        public ActionResult<IEnumerable<ProjectInfo>> GetAll()
        {
            return Ok(MetadataRepository.GetProjects());
        }

        /// <summary>
        /// Gets a single project by id.
        /// </summary>
        /// <param name="id">The project id</param>
        /// <returns></returns>
        [HttpGet("/Projects/{id}")]
        public ActionResult<ProjectInfo> GetOne(int id)
        {
            return Ok(MetadataRepository.GetProject(id));
        }

        /// <summary>
        /// Creates a new project.
        /// </summary>
        /// <param name="project">The new project</param>
        /// <returns></returns>
        [HttpPost("/Projects")]
        public ActionResult<ProjectInfo> Create(ProjectInfo project)
        {
            return Ok(MetadataRepository.CreateProject(project));
        }

        /// <summary>
        /// Updates an existing project.
        /// </summary>
        /// <param name="id">The project id to update</param>
        /// <param name="project">The updated project</param>
        /// <returns></returns>
        [HttpPut("/Projects/{id}")]
        public ActionResult Update(int id, [FromBody] ProjectInfo project)
        {
            MetadataRepository.UpdateProject(id, project);
            return NoContent();
        }

        /// <summary>
        /// Deletes an existing project.
        /// </summary>
        /// <param name="id">The id of the project id delete</param>
        /// <returns>No content</returns>
        [HttpDelete("/Projects/{id}")]
        public ActionResult Delete(int id)
        {
            MetadataRepository.DeleteProject(id);
            return NoContent();
        }

        /// <summary>
        /// Scans the database for the project, and caches a list of all the object metadata.
        /// </summary>
        /// <param name="id">The project id to scan</param>
        /// <returns></returns>
        [HttpPut("/Scan/{id}")]
        public ActionResult Scan(int id)
        {
            var mr = MetadataRepository.Connect(ConnectionString);
            var project = mr.GetProjects().First(c => c.ProjectId == id);

            // Scan Entities
            mr.ScanProject(project);

            return NoContent();
        }

        /// <summary>
        /// Documents the database.
        /// </summary>
        /// <param name="id">The id of the project to document.</param>
        /// <returns></returns>
        [HttpGet("/Document/{id}")]
        public ActionResult Document(int id)
        {
            var mr = MetadataRepository.Connect(ConnectionString);
            var project = mr.GetProjects().First(p => p.ProjectId == id);
            var doc = new Documenter(mr, project);
            var result = doc.Document().Result;
            return File(result, @"application/pdf", $"{project.ProjectName}.pdf");
        }
    }
}
