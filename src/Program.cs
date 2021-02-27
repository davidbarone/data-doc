using System;
using PuppeteerSharp;
using System.Threading.Tasks;
using System.Linq;

namespace DataDoc
{
    class Program
    {
        static void Main(string[] args)
        {
            DoWork().Wait();
        }

        static void SetupData()
        {
            var cs = @"Integrated Security=SSPI;Data Source=localhost\SQLEXPRESS;";
            var mr = MetadataRepository.Connect(cs);

            // Create Project
            //mr.CreateProject("ReactCrudDemo", @"Provider=SQLNCLI11.1;Integrated Security=SSPI;Data Source=localhost\SQLEXPRESS;Initial Catalog=react-crud-demo;");

            var project = mr.GetProjects().First(c => c.ProjectName == "ReactCrudDemo");

            // Scan Entities
            mr.ScanProject(project);

        }

        async static Task<string> DoWork()
        {

            await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision);
            using (var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true
            }))
            {
                var page = await browser.NewPageAsync();
                await page.GoToAsync("https://api.dbarone.com/resources/name/AnalyticsNotebook.Docs.html");
                await page.PdfAsync("./out.pdf", new PdfOptions
                {

                    DisplayHeaderFooter = true,
                    HeaderTemplate = "<div>THIS IS A HEADER</div>",
                    MarginOptions = new PuppeteerSharp.Media.MarginOptions
                    {
                        Top = "100px",
                        Bottom = "100px",
                        Left = "100px",
                        Right = "100px"
                    }
                });
            }

            return "OK";
        }
    }
}
