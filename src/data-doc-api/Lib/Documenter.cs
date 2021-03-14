using data_doc_api.Models;
using System.Linq;
using PuppeteerSharp;
using System;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;
using data_doc_api.Lib;

namespace data_doc_api
{
    public class Documenter
    {
        MetadataRepository MetadataRepository { get; set; }
        ProjectInfo Project { get; set; }
        IEnumerable<EntityInfo> Entities { get; set; }
        IEnumerable<EntityConfigInfo> EntitiesConfig { get; set; }
        IEnumerable<AttributeInfo> Attributes { get; set; }
        IEnumerable<AttributeConfigInfo> AttributesConfig { get; set; }
        IEnumerable<EntityDependencyInfo> EntityDependencies { get; set; }
        IEnumerable<RelationshipInfo> Relationships { get; set; }

        public Documenter(MetadataRepository metadataRepository, ProjectInfo project)
        {
            this.MetadataRepository = metadataRepository;
            this.Project = project;
            this.Entities = MetadataRepository.GetEntities(Project.ProjectId);
            this.EntitiesConfig = MetadataRepository.GetEntitiesConfig(Project.ProjectId);
            this.Attributes = MetadataRepository.GetAttributes(project);
            this.AttributesConfig = MetadataRepository.GetAttributesConfig(project);
            this.EntityDependencies = metadataRepository.GetEntityDependencies(project);
            this.Relationships = metadataRepository.GetRelationships(project);
        }

        public async Task<byte[]> Document(bool poweredByLink = false)
        {
            Func<string> poweredByHtml = () => { return poweredByLink ? "<div>Powered by <a href='https://github.com/davidbarone/data-doc'>Data-Doc</a></div>" : ""; };
            await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision);
            using (var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                //Headless = true
            }))
            {
                var page = await browser.NewPageAsync();
                //await page.GoToAsync("https://api.dbarone.com/resources/name/AnalyticsNotebook.Docs.html");
                var html = GetHtml(Project);
                var tmpFile = Path.GetTempFileName();
                await page.SetContentAsync(html);
                await page.PdfAsync(tmpFile, new PdfOptions
                {

                    DisplayHeaderFooter = true,
                    HeaderTemplate = $@"
                    <div
                        style='
                            -webkit-print-color-adjust: exact;
                            font-size: 10px;
                            width: 100%;
                            padding: 4px 20px;
                            text-align: right;
                            border-bottom: 1px solid #999;
                        '>
                        Metadata Repository for: {Project.ProjectName}
                    </div>",
                    FooterTemplate = $@"
                    <div
                        style='
                            -webkit-print-color-adjust: exact;
                            font-size: 8px;
                            width: 100%;
                            padding: 4px 20px;
                            text-align: center;
                            border-top: 1px solid #999'>
                            <div style='width: 100%;'>Page <span class='pageNumber
                        ' /></div>
                        {poweredByHtml()}
                    </div>",
                    MarginOptions = new PuppeteerSharp.Media.MarginOptions
                    {
                        Top = "100px",
                        Bottom = "100px",
                        Left = "100px",
                        Right = "100px"
                    }
                });

                // Read tmpFile, and return
                var file = new FileStream(tmpFile, FileMode.Open);
                var mem = new MemoryStream();
                file.CopyTo(mem);
                return mem.ToArray();
            }
        }

        private string GetHtml(ProjectInfo project)
        {
            var entitiesConfig = EntitiesConfig.Where(entitiesConfig => entitiesConfig.IsActive);
            var generateIndexHtml = GetIndexHtml();
            var entityHtml = String.Join("", entitiesConfig.Select(e => GetEntityHtml(e)));

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
        border-collapse: collapse;
        border: 1px solid #aaa;
    }}

    tr:nth-child(odd) {{
        background: #e0f0ff;
    }}

    th, td {{
        padding: 2px 8px;
        border: 1px solid white;
    }}

    th {{
        background: #ccc;
        font-weight: 600;
        padding: 8px;
    }}

    div.entity, div.index {{
        page-break-after: always;
    }}

    div.cover {{
        page-break-after: always;
        padding: 300px 50px;
    }}

    div.title {{
        font-weight: 600;
        font-size: 48px;
    }}

    div.subtitle {{
        font-weight: 600;
        font-size: 36px;
    }}

</style>

<script type='text/javascript'>
    // Used for table of contents
    var currpage = 0;
    var pagenum = [];
</script>
</head>

<body>

<!-- Cover Page -->

<div class=cover>
    <div class='title'>
        Metadata Repository
        <div class='subtitle'>{project.ProjectName}</div>
    </div>
    <div>Version: {project.ScanVersion} (scan), {project.ConfigVersion} (config)</div>
    <div>Updated: {project.ScanUpdatedDt} (scan), {project.ConfigUpdatedDt} (config)</div>
</div>

<!-- Index (generated at end) -->
<div id=index class=index></div>

{entityHtml}

{generateIndexHtml}

</body>
</html>
            ";
        }

        private string GetIndexHtml()
        {
            return $@"
                <script type='text/javascript'>
                    var count = document.getElementsByClassName('counter').length;
                    var indexHtml = `<h2>Index</h2>` + pagenum.map(pn => `<div><a href='#${{pn.entity}}'>${{pn.entity}}</a></div>`).join(' ');
                    document.getElementById('index').innerHTML = indexHtml;
                </script>
            ";
        }

        private string GetEntitiesHtml()
        {
            var entitiesConfig = EntitiesConfig.Where(entities => entities.IsActive);
            var entityHtml = String.Join("", entitiesConfig.Select(e => GetEntityHtml(e)));
            return entityHtml;
        }

        private string GetEntityHtml(EntityConfigInfo entityConfig)
        {
            var entity = Entities.FirstOrDefault(entity => entity.ProjectId == entityConfig.ProjectId && entity.EntityName == entityConfig.EntityName);
            var attributesHtml = GetAttributesHtml(entityConfig);
            var entityDataPreviewHtml = GetEntityDataPreviewHtml(entityConfig);
            var entityDependencyDownHtml = GetEntityDependencyHtml(entityConfig, false);
            var entityDependencyUpHtml = GetEntityDependencyHtml(entityConfig, true);

            if (entity == null)
            {
                return "";
            }

            return $@"
                <script type='text/javascript'>
                    pagenum.push({{
                        entity: '{entityConfig.EntityAlias}',
                        page: currpage
                    }});
                </script>
                <div class='entity' id='{entityConfig.EntityAlias}'>
                    <h2>Entity Alias: {entityConfig.EntityAlias}</h2>
                    <div>{entityConfig.EntityDesc}</div>
                    
                    <h3>Entity Properties</h3>
                    <table>
                        <thead>
                            <tr>
                                <th>Entity Name</th>
                                <th>Entity Type</th>
                                <th>Created Date</th>
                                <th>Modified Date</th>
                                <th>Updated Date</th>
                                <th>Row Count</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td>{entity.EntityName}</td>
                                <td>{entity.EntityType}</td>
                                <td>{entity.CreatedDt}</td>
                                <td>{entity.ModifiedDt}</td>
                                <td>{entity.UpdatedDt}</td>
                                <td>{entity.RowCount}</td>
                            </tr>
                        </tbody>
                    </table>

                    <h3>Attributes</h3>
                    {attributesHtml}

                    <h3>Data Preview</h3>
                    {entityDataPreviewHtml}

                    <h3>Dependencies Down (Objects on which [{entity.EntityName}] depends)</h3>
                    {entityDependencyDownHtml}

                    <h3>Dependencies Up (Objects that depend on [{entity.EntityName}])</h3>
                    {entityDependencyUpHtml}

                </div>
            ";
        }

        private string GetEntityDataPreviewHtml(EntityConfigInfo entityConfig)
        {
            var errorMessage = "Data preview is not available for this entity.";
            var entityInfo = Entities.FirstOrDefault(e => e.ProjectId == entityConfig.ProjectId && e.EntityName == entityConfig.EntityName);
            if (!entityConfig.ShowData || entityInfo == null)
            {
                return errorMessage;
            }

            var data = MetadataRepository.GetEntityData(Project, entityInfo);
            if (!data.Any())
            {
                return errorMessage;
            }

            var firstRow = true;
            var rowHtml = "";
            var headerHtml = "";
            List<string> headers = new List<string>();
            foreach (var row in data)
            {
                var rowAsIDict = (IDictionary<string, object>)row;
                if (firstRow)
                {
                    foreach (var key in rowAsIDict.Keys)
                    {
                        headers.Add(key);
                    }
                    firstRow = false;
                }

                headerHtml = $@"
                        <tr>
                            {String.Join("", headers.Select(h => $"<th>{h}</th>"))}
                        </tr>";

                rowHtml += "<tr>" + String.Join("", headers.Select(h => $"<td>{rowAsIDict[h]}</td>")) + "</tr>";
            }

            return $@"
                <table>
                    <thead>
                        {headerHtml}
                    </thead>
                    <tbody>
                        {rowHtml}
                    </tbody>
                </table>
            ";
        }

        private int GetAttributeConfigOrder(AttributeConfigInfo attributeConfig)
        {
            var attribute = Attributes
                .FirstOrDefault(attributes =>
                    attributes.ProjectId == attributeConfig.ProjectId &&
                    attributes.EntityName == attributeConfig.EntityName &&
                    attributes.AttributeName == attributeConfig.AttributeName);
            return attribute != null ? attribute.Order : 0;
        }

        private string GetAttributesHtml(EntityConfigInfo entityConfig)
        {
            var attributesConfig = AttributesConfig
                .Where(a => a.ProjectId == entityConfig.ProjectId && a.EntityName == entityConfig.EntityName)
                .OrderBy(a => GetAttributeConfigOrder(a));

            var attributeHtml = String.Join("", attributesConfig.Select(a => GetAttributeHtml(a)));

            return $@"
                <table>
                    <thead>
                        <tr>
                            <th>Attribute Name</th>
                            <th>Data Type</th>
                            <th>Nulls</th>
                            <th>References</th>
                            <th>Description</th>
                        </tr>
                    </thead>
                    <tbody>
                        {attributeHtml}
                    </tbody>
                </table>
            ";
        }

        private string GetAttributeHtml(AttributeConfigInfo attributeConfig)
        {
            var attribute = Attributes.FirstOrDefault(a => a.ProjectId == attributeConfig.ProjectId && a.EntityName == attributeConfig.EntityName && a.AttributeName == attributeConfig.AttributeName);
            if (attribute == null)
            {
                return "";
            }

            // References for the attribute
            var references = string.Join(" ", Relationships
                .Where(r => r.ParentEntityName.Equals(attribute.EntityName))
                .Where(r => r.ParentAttributeName.Equals(attribute.AttributeName))
                .Select(r => r.ReferencedEntityName)
                .Select(r => this.EntitiesConfig.First(e => e.EntityName.Equals(r)).EntityAlias)
                .Select(r => $"<a href='#{r}'>{r}</a>"));

            Func<bool, string> getColor = (isPrimaryKey) => { return isPrimaryKey ? "style='background: wheat;'" : ""; };

            return $@"
            <tr { getColor(attribute.IsPrimaryKey) }>
                <td>{attribute.AttributeName}</td>
                <td>{attribute.DataTypeDesc()}</td>
                <td>{(attribute.IsNullable ? "Yes" : "No")}</td>
                <td>{references}</td>
                <td>{attributeConfig.AttributeDesc}</td>
            </tr>";
        }

        private string GetEntityDependencyHtml(EntityConfigInfo entityConfig, bool reverseDirection = false)
        {
            IEnumerable<ParentChild<string>> treeMapping = null;

            // False = dependency down (objects used by this object)
            // True = dependency up (objects that this object is used in)
            if (reverseDirection == false)
            {
                treeMapping = EntityDependencies.Select(ed => new ParentChild<string>(ed.ParentEntityName, ed.ChildEntityName));
            }
            else
            {
                treeMapping = EntityDependencies.Select(ed => new ParentChild<string>(ed.ChildEntityName, ed.ParentEntityName));
            }

            var tree = new TreeNode<string>(treeMapping, entityConfig.EntityName, null, (string a, string b) => { return a.Equals(b, StringComparison.OrdinalIgnoreCase); });
            return $@"<pre>{tree.PrettyPrint()}</pre>";
        }
    }
}