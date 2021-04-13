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
        IEnumerable<EntityDetailsInfo> Entities { get; set; }
        IEnumerable<AttributeDetailsInfo> Attributes { get; set; }
        IEnumerable<EntityDependencyInfo> EntityDependencies { get; set; }
        IEnumerable<RelationshipScanInfo> Relationships { get; set; }

        public Documenter(MetadataRepository metadataRepository, ProjectInfo project)
        {
            this.MetadataRepository = metadataRepository;
            this.Project = project;
            this.Entities = MetadataRepository.GetEntityDetails(project.ProjectId);
            this.Attributes = MetadataRepository.GetAttributeDetails(project.ProjectId);
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
                        Top = "80px",
                        Bottom = "80px",
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
            var entities = Entities.Where(e => e.IsActive);
            var generateIndexHtml = GetIndexHtml();
            var entityHtml = String.Join("", entities.Select(e => GetEntityHtml(e)));
            var projectHtml = GetProjectHtml();

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

    div.entity, div.index, div.project {{
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

    /* The entity title */
    h2 {{
        display: block;
        background: #2468AC;
        color: #ffffff;
        border-left: 8px solid #012345;
        padding: 4px 8px;
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

{projectHtml}

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

        private string GetProjectHtml()
        {
            var sql = "";
            if (!string.IsNullOrEmpty(Project.ProjectDesc))
            {
                sql += $@"
                    <h2>Project Description</h2>
                    <div>{Project.ProjectDesc}</div>";
            }
            if (!string.IsNullOrEmpty(Project.ProjectComment))
            {
                sql += $@"
                    <h2>Project Comment</h2>
                    <div>{Project.ProjectComment}</div>";
            }
            if (!string.IsNullOrEmpty(sql))
            {
                return @$"<div class=""project"">{sql}</div>";
            }
            else
            {
                return sql;
            }
        }

        private string GetEntitiesHtml()
        {
            var entities = Entities.Where(e => e.IsActive);
            var entityHtml = String.Join("", entities.Select(e => GetEntityHtml(e)));
            return entityHtml;
        }

        private string GetEntityHtml(EntityDetailsInfo entity)
        {
            var attributesHtml = GetAttributesHtml(entity);
            var entityDataPreviewHtml = GetEntityDataPreviewHtml(entity);
            var entityDependencyDownHtml = GetEntityDependencyHtml(entity, false);
            var entityDependencyUpHtml = GetEntityDependencyHtml(entity, true);
            var entityDefinitionHtml = GetEntityDefinitionHtml(entity);
            var entityRelationsHtml = GetRelationsHtml(entity);
            var entityComment = string.IsNullOrEmpty(entity.EntityComment) ? "" : $"<h3>Additional Comments</h3><div>{entity.EntityComment}</div>";

            if (entity == null)
            {
                return "";
            }

            return $@"
                <script type='text/javascript'>
                    pagenum.push({{
                        entity: '{entity.EntityAlias}',
                        page: currpage
                    }});
                </script>
                <div class='entity' id='{entity.EntityAlias}'>
                    <h2>{entity.EntityAlias}</h2>
                    <div>{entity.EntityDesc}</div>

                    {entityComment}

                    <h3>Properties</h3>
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

                    <h3>Relations</h3>
                    {entityRelationsHtml}

                    <h3>Preview</h3>
                    {entityDataPreviewHtml}

                    <h3>Definition</h3>
                    {entityDefinitionHtml}

                    <h3>Objects on which [{entity.EntityName}] depends</h3>
                    {entityDependencyDownHtml}

                    <h3>Objects that depend on [{entity.EntityName}]</h3>
                    {entityDependencyUpHtml}

                </div>
            ";
        }

        private string GetEntityDataPreviewHtml(EntityDetailsInfo entity)
        {
            var errorMessage = "[Data preview is not available for this entity.]";
            if (!entity.ShowData)
            {
                return errorMessage;
            }

            var data = MetadataRepository.GetEntityData(Project, entity);
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

        private string GetEntityDefinitionHtml(EntityDetailsInfo entity)
        {
            if (entity.ShowDefinition)
            {
                return $@"<pre>{entity.Definition}</pre>";
            }
            else
            {
                return "[Definition not available for this entity.]";
            }
        }

        private string GetAttributesHtml(EntityDetailsInfo entity)
        {
            var attributes = Attributes
                .Where(a => a.ProjectId == entity.ProjectId && a.EntityName.Equals(entity.EntityName, StringComparison.OrdinalIgnoreCase))
                .OrderBy(a => a.Order);

            var attributeHtml = String.Join("", attributes.Select(a => GetAttributeHtml(a)));

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

        /// <summary>
        /// Gets the child relationships for an entity.
        /// </summary>
        /// <param name="entityConfig"></param>
        /// <returns></returns>
        private string GetRelationsHtml(EntityDetailsInfo entity)
        {
            var relations = string.Join(" ", Relationships
                .Where(r => r.ReferencedEntityName.Equals(entity.EntityName, StringComparison.OrdinalIgnoreCase))
                .Select(r => new
                {
                    RelationshipName = r.RelationshipName,
                    ParentEntityName = r.ParentEntityName
                })
                .Distinct()
                .Select(r => new
                {
                    Name = this.Entities.First(e => e.EntityName.Equals(r.ParentEntityName)).EntityAlias,
                    Role = r.RelationshipName
                })
                .Select(r => $"<tr><td><a href='#{r.Name}'>{r.Name}</a></td><td>{r.Role}</td></tr>"));

            if (relations.Any())
            {
                return $@"
<table>
    <thead>
        <tr>
            <th>Entity</th>
            <th>Relationship</th>
        </tr>
    </thead>
    <tbody>
        { String.Join("", relations) }
    </tbody>
</table>";
            }
            else
            {
                return "[No child relations for this entity.]";
            }
        }

        private string GetAttributeHtml(AttributeDetailsInfo attribute)
        {
            var attributeComment = !string.IsNullOrEmpty(attribute.AttributeComment) ? $"<p>{attribute.AttributeComment}</p>" : "";
            // References for the attribute
            var references = string.Join(" ", Relationships
                .Where(r => r.ParentEntityName.Equals(attribute.EntityName, StringComparison.OrdinalIgnoreCase))
                .Where(r => r.ParentAttributeName.Equals(attribute.AttributeName, StringComparison.OrdinalIgnoreCase))
                .Select(r => r.ReferencedEntityName)
                .Select(r => this.Entities.First(e => e.EntityName.Equals(r)).EntityAlias)
                .Select(r => $"<a href='#{r}'>{r}</a>"));

            Func<bool, string> getColor = (isPrimaryKey) => { return isPrimaryKey ? "style='background: wheat;'" : ""; };

            return $@"
            <tr { getColor(attribute.IsPrimaryKey) }>
                <td>{attribute.AttributeName}</td>
                <td>{attribute.DataTypeDesc}</td>
                <td>{(attribute.IsNullable ? "Yes" : "")}</td>
                <td>{references}</td>
                <td>{attribute.AttributeDesc}{attributeComment}</td>
            </tr>";
        }

        private string GetEntityDependencyHtml(EntityDetailsInfo entity, bool reverseDirection = false)
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

            var tree = new TreeNode<string>(treeMapping, entity.EntityName, null, (string a, string b) => { return a.Equals(b, StringComparison.OrdinalIgnoreCase); });
            return $@"<pre>{tree.PrettyPrint()}</pre>";
        }
    }
}