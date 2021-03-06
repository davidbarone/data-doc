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

        /// <summary>
        /// Constructor for ProjectController class.
        /// </summary>
        /// <param name="connectionStrings">Connection string.</param>
        public ProjectController(IOptions<ConnectionStringConfig> connectionStrings)
        {
            this.ConnectionString = connectionStrings.Value.DataDoc;
        }

        /// <summary>
        /// Gets a list of all active projects.
        /// </summary>
        /// <returns>The list of projects.</returns>
        [HttpGet("/Projects")]
        public ActionResult<IEnumerable<ProjectInfo>> GetAll()
        {
            using (var db = new SqlConnection(ConnectionString))
            {
                var projects = db.Query<ProjectInfo>("SELECT * FROM PROJECT WHERE ISACTIVE = 1");
                return Ok(projects);
            }
        }

        /// <summary>
        /// Gets a single project by id.
        /// </summary>
        /// <param name="id">The project id</param>
        /// <returns></returns>
        [HttpGet("/Projects/{id}")]
        public ActionResult<ProjectInfo> GetOne(int id)
        {
            using (var db = new SqlConnection(ConnectionString))
            {
                var project = db.Query<ProjectInfo>(
                    "SELECT * FROM PROJECT WHERE ProjectId = @ProjectId",
                    new
                    {
                        ProjectId = id
                    }).FirstOrDefault();

                if (project == null)
                {
                    throw new Exception("Task not found.");
                }

                return Ok(project);
            }
        }

        /// <summary>
        /// Creates a new project.
        /// </summary>
        /// <param name="project">The new project</param>
        /// <returns></returns>
        [HttpPost("/Projects")]
        public ActionResult<ProjectInfo> Create(ProjectInfo project)
        {
            using (var db = new SqlConnection(ConnectionString))
            {
                var now = DateTime.Now;

                var newProject = db.Query<ProjectInfo>(@"
INSERT INTO Project (
    ProjectName, ProjectDesc, ConnectionString, ScanVersion, ScanUpdatedDt, ConfigVersion, ConfigUpdatedDt, IsActive)
SELECT
    @ProjectName, @ProjectDesc, @ConnectionString, @ScanVersion, @ScanUpdatedDt, @ConfigVersion, @ConfigUpdatedDt, @IsActive;
SELECT * FROM Project WHERE ProjectId = SCOPE_IDENTITY();", new
                {
                    ProjectName = project.ProjectName,
                    ProjectDesc = project.ProjectDesc,
                    ConnectionString = project.ConnectionString,
                    ScanVersion = 0,
                    ScanUpdatedDt = now,
                    ConfigVersion = 0,
                    ConfigUpdatedDt = now,
                    IsActive = 1
                });
                return Ok(newProject);
            }
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
            if (id != project.ProjectId)
            {
                throw new Exception("Invalid project id");
            }

            using (var db = new SqlConnection(ConnectionString))
            {
                var newProject = db.Execute(@"
UPDATE
    Project
SET
    ProjectName = @ProjectName,
    ProjectDesc = @ProjectDesc,
    ConnectionString = @ConnectionString,
    ScanVersion = @ScanVersion,
    ScanUpdatedDt = @ScanUpdatedDt,
    ConfigVersion = @ConfigVersion,
    ConfigUpdatedDt = @ConfigUpdatedDt,
    IsActive = @IsActive
WHERE
    ProjectId = @ProjectId;", new
                {
                    ProjectId = project.ProjectId,
                    ProjectName = project.ProjectName,
                    ProjectDesc = project.ProjectDesc,
                    ConnectionString = project.ConnectionString,
                    ScanVersion = project.ScanVersion,
                    ScanUpdatedDt = project.ScanUpdatedDt,
                    ConfigVersion = project.ConfigVersion,
                    ConfigUpdatedDt = project.ConfigUpdatedDt,
                    IsActive = project.IsActive
                });
                return NoContent();
            }
        }

        /// <summary>
        /// Deletes an existing project.
        /// </summary>
        /// <param name="id">The id of the project id delete</param>
        /// <returns>No content</returns>
        [HttpDelete("/Projects/{id}")]
        public ActionResult Delete(int id)
        {
            using (var db = new SqlConnection(ConnectionString))
            {
                var newTask = db.Query<ProjectInfo>(@"
DELETE FROM
    Project
WHERE
    ProjectId = @ProjectId;", new
                {
                    ProjectId = id
                });
                return Ok();
            }
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
        /// <param name="projectName">The project to document.</param>
        /// <returns></returns>
        [HttpGet("Document")]
        public ActionResult Document(string projectName)
        {
            //DoWork().Wait();
            var cs = @"Integrated Security=SSPI;Data Source=localhost\SQLEXPRESS;";
            var mr = MetadataRepository.Connect(cs);
            var project = mr.GetProjects().First(p => p.ProjectName == "ReactCrudDemo");
            var doc = new Documenter(mr);
            doc.Document(project).Wait();
            return NoContent();
        }
    }
}
