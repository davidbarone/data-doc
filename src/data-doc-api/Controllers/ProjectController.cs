using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using data_doc_api.Models;
using System.Collections.Generic;
using Dapper;
using System.Data.SqlClient;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace data_doc_api.Controllers
{
    /// <summary>
    /// Provides services to manage projects
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class ProjectController : ControllerBase
    {
        private string ConnectionString { get; set; }
        private MetadataRepository MetadataRepository { get; set; }

        /// <summary>
        /// Constructor for ProjectController class
        /// </summary>
        /// <param name="connectionStrings">The connection string</param>
        public ProjectController(IOptions<ConnectionStringConfig> connectionStrings)
        {
            this.ConnectionString = connectionStrings.Value.DataDoc;
            this.MetadataRepository = MetadataRepository.Connect(ConnectionString);
        }

        /// <summary>
        /// Gets a list of all projects
        /// </summary>
        /// <returns>The list of projects</returns>
        [HttpGet("/Projects")]
        public ActionResult<IEnumerable<ProjectInfo>> GetAll()
        {
            return Ok(MetadataRepository.GetProjects());
        }

        /// <summary>
        /// Gets a single project by id
        /// </summary>
        /// <param name="id">The project id</param>
        /// <returns>The requested project</returns>
        [HttpGet("/Projects/{id}")]
        public ActionResult<ProjectInfo> GetOne(int id)
        {
            return Ok(MetadataRepository.GetProject(id));
        }

        /// <summary>
        /// Creates a new project
        /// </summary>
        /// <param name="project">The new project</param>
        /// <returns>The new project</returns>
        [HttpPost("/Projects")]
        public ActionResult<ProjectInfo> Create(ProjectInfo project)
        {
            return Ok(MetadataRepository.CreateProject(project));
        }

        /// <summary>
        /// Updates an existing project
        /// </summary>
        /// <param name="projectId">The project id</param>
        /// <param name="project">The updated project</param>
        /// <returns>No content</returns>
        [HttpPut("/Projects/{projectId}")]
        public ActionResult Update(int projectId, [FromBody] ProjectInfo project)
        {
            MetadataRepository.UpdateProject(projectId, project);
            return NoContent();
        }

        /// <summary>
        /// Deletes an existing project
        /// </summary>
        /// <param name="id">The project id</param>
        /// <param name="purgeData">If set to true, the data will be purged prior to deleting the project</param>
        /// <returns>No content</returns>
        [HttpDelete("/Projects/{id}/{purgeData?}")]
        public ActionResult Delete(int id, bool purgeData = false)
        {
            MetadataRepository.DeleteProject(id, purgeData);
            return NoContent();
        }

        /// <summary>
        /// Scans the database for the project, and caches a list of all the object metadata
        /// </summary>
        /// <param name="id">The project id</param>
        /// <returns>No content</returns>
        [HttpPut("/Projects/Scan/{id}")]
        public ActionResult Scan(int id)
        {
            var mr = MetadataRepository.Connect(ConnectionString);
            var project = mr.GetProjects().First(c => c.ProjectId == id);

            // Scan Entities
            mr.ScanProject(project);

            return NoContent();
        }

        /// <summary>
        /// Documents the database
        /// </summary>
        /// <param name="id">The project id</param>
        /// <param name="addCredit">If set to true, then adds a 'powered by' credit on footer of document</param>
        /// <returns>A PDF file documenting the database</returns>
        [HttpGet("/Projects/Document/{id}/{addCredit?}")]
        public ActionResult Document(int id, bool addCredit = false)
        {
            var mr = MetadataRepository.Connect(ConnectionString);
            var project = mr.GetProjects().First(p => p.ProjectId == id);
            var doc = new Documenter(mr, project);
            var result = doc.Document(addCredit).Result;
            return File(result, @"application/pdf", $"{project.ProjectName}.v{project.Version}.pdf");
        }

        /// <summary>
        /// Returns a backup object containing all information for the project
        /// </summary>
        /// <param name="projectId">The project to back up</param>
        /// <returns></returns>
        [HttpGet("/Projects/Backup/{projectId}")]
        public ActionResult Backup(int projectId)
        {
            var project = MetadataRepository.GetProject(projectId);
            var data = MetadataRepository.GetBackup(projectId);
            var stringData = JsonSerializer.Serialize<BackupInfo>(data);
            // convert string to stream
            byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(stringData);
            return File(byteArray, "application/json", $"{project.ProjectName}.v{project.Version}.backup.json");
        }

        /// <summary>
        /// Restores a backup file to an existing project
        /// </summary>
        /// <param name="projectId">The project to restore over</param>
        /// <param name="file">The backup file to restore</param>
        /// <returns></returns>
        [HttpPost("/Projects/Restore/{projectId}")]
        public ActionResult Restore(int projectId, IFormFile file)
        {
            var sr = new StreamReader(file.OpenReadStream());
            var json = sr.ReadToEnd();
            var obj = JsonSerializer.Deserialize<BackupInfo>(json);
            MetadataRepository.Restore(projectId, obj);
            return Content("File restored successfully");

        }

    }
}
