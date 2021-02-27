using DataDoc.Models;
using System.Linq;
using PuppeteerSharp;
using System;
using System.Threading.Tasks;

namespace DataDoc
{
    public class Documenter
    {
        MetadataRepository MetadataRepository { get; set; }
        public Documenter(MetadataRepository metadataRepository)
        {
            this.MetadataRepository = metadataRepository;
        }


        public async Task Document(ProjectInfo project)
        {

            await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision);
            using (var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true
            }))
            {
                var page = await browser.NewPageAsync();
                //await page.GoToAsync("https://api.dbarone.com/resources/name/AnalyticsNotebook.Docs.html");
                var html = GetHtml(project);
                var outputFile = "./out.pdf";
                await page.SetContentAsync(html);
                await page.PdfAsync(outputFile, new PdfOptions
                {

                    DisplayHeaderFooter = true,
                    HeaderTemplate = "<div style='font-size: 20px;'>Data Document Report</div>",
                    FooterTemplate = "<div style='font-size: 12px;'>Powered by Data-Doc</div>",
                    MarginOptions = new PuppeteerSharp.Media.MarginOptions
                    {
                        Top = "100px",
                        Bottom = "100px",
                        Left = "100px",
                        Right = "100px"
                    }
                });
            }
        }

        private string GetHtml(ProjectInfo project)
        {
            var entities = MetadataRepository.GetEntities(project);
            var entityHtml = String.Join("", entities.Select(e => GetEntityHtml(project, e)));

            return $@"
<!doctype html>
<html lang=""en"">
<head>
<style>
    html {{
        font-family: Arial;
        -webkit-print-color-adjust: exact;
    }}

    table {{
        background: #555555;
        border-collapse: collapse;
        border: 1px solid black;
    }}

    header {{
        font-size: 20px;
    }}

    footer {{
        font-size: 12px;
    }}

    th, td {{
        border: 1px solid black;
    }}

    tr th {{
        background-color: #888888;
    }}

    div.entity {{
        page-break-after: always;
    }}
</style>
</head>

<body>
{entityHtml}
</body>
</html>
            ";
        }

        private string GetEntityHtml(ProjectInfo project, EntityInfo entity)
        {
            var attributes = MetadataRepository
                .GetAttributes(project)
                .Where(a => a.EntityName == entity.EntityName)
                .OrderBy(a => a.Order);

            var attributeHtml = String.Join("", attributes.Select(a => $@"
            <tr>
                <td>{a.AttributeName}</td>
                <td>{a.Order}</td>
                <td>{a.IsPrimaryKey}</td>
                <td>{a.DataType}</td>
                <td>{a.DataLength}</td>
                <td>{a.Precision}</td>
                <td>{a.Scale}</td>
            </tr>"));

            return $@"
            <div class='entity'>
                <label>Entity:</label><span>{entity.EntityName}</span>
                <h3>Attributes</h3>
                <table>
                    <thead>
                        <tr>
                            <th>AttributeName</th>
                            <th>Order</th>
                            <th>IsPrimaryKey</th>
                            <th>DataType</th>
                            <th>DataLength</th>
                            <th>Precision</th>
                            <th>Scale</th>
                        </tr>
                    </thead>
                    <tbody>
                        {attributeHtml}
                    </tbody>
                </table>
            </div>
            ";
        }
    }
}