using System.Linq;
using Microsoft.AspNetCore.Mvc;


namespace data_doc_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DataDocController : ControllerBase
    {

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
