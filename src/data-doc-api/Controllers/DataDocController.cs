using System.Linq;
using Microsoft.AspNetCore.Mvc;


namespace data_doc_api.Controllers
{
    /// <summary>
    /// Provides services to document SQL Server databases.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class DataDocController : ControllerBase
    {
        /// <summary>
        /// Scans the database for the project, and caches a list of all the object metadata.
        /// </summary>
        /// <param name="projectName"></param>
        /// <returns></returns>
        [HttpPost("/Scan/{projectName}")]
        public ActionResult Scan(string projectName)
        {
            var cs = @"Integrated Security=SSPI;Data Source=localhost\SQLEXPRESS;";
            var mr = MetadataRepository.Connect(cs);
            var project = mr.GetProjects().First(c => c.ProjectName == "ReactCrudDemo");

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
