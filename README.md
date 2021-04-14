# data-doc

Data-Doc is a simple metadata repository application for SQL Server databases. It is especially useful for documenting data warehouses. It automatically pulls in schema metadata using built-in dynamic management views (DMVs), and allows DBAs, BI professionals, data stewards, and other data professionals to assign business metadata.

![anaytics-notebook](https://github.com/davidbarone/data-doc/blob/main/images/data-doc-ui.png?raw=true)

## Metadata Repository

A metadata repository is simply a database that holds metadata regarding some other data (normally a database). The data normally stored in a metadata repository is normally split between:

- Business metadata
- Technical metadata
- Operational metadata

## Business Metadata

Business metadata includes things like:
- Business vocabularies, and terminology
- Business definitions of the data elements
- Rules, metrics KPIs and other higher level abstractions of the data

## Technical Metadata

Technical metadata typically includes:
- Details of physical objects, tables, entities
- Details of columns, data types, sizes, ranges
- Primary key information, nullable indicators
- Relationships, foreign keys, cardinality of joins
- Object dependencies, data lineage

## Operational Metadata

Operational metadata typically includes:
- Table sizes, disk space utilisation
- Query metrics, frequency, duration

## Projects
Data-Doc can store metadata for multiple databases. These are configured as an individual `project`. Each project defines a connection string to a source database. Once a project has been set up, the source database can be scanned to pick up the built-in metadata that SQL Server provides about tables, columns, and relationships.

## Adding Business Metadata
Once the source database has been scanned for its physical metadata (i.e. column names, data types etc), Data-Doc allows business semantics to be overlaid. Typically, business descriptions can be defined for both entities (tables, views) and attributes (columns). The following types of business metata can be added:
- Descriptions and comment at project level
- Descriptions and comment at entity level
- Descriptions and comment at attribute level
- Disabling / enabling of entities
- Disabling / enabling of attributes
- Definition of values stored within an attribute (attribute's domain).

## Documentation
The REST API provides a documentation endpoint, to document individual projects. The default output is in PDF format, and this is obtained via <a href="https://www.puppeteersharp.com/index.html">PuppeteerSharp</a> and `Headless Chrome`. The output is currently fixed in code. An example using the AdventureWorks DW2019 database can be found <a href='https://github.com/davidbarone/data-doc/blob/main/docs/AdventureWorks DW2019.pdf'>here</a>. The Data-Doc schema can be documented by itself, and the output can also be found <a href='https://github.com/davidbarone/data-doc/blob/main/docs/Data-Doc.pdf'>here</a>.

## Debugging in VSCode
VSCode workspaces are a nice way to manage multiple projects. data-doc's workspace contains the data-doc-api and data-doc-ui. Each project can be run independently. However, the workspace launch.json file can be edited to allow both projects to be launched simultaneously from the `run and debug` menu. Please refer to the data-doc.code-workspace file for details on how this was configured.

https://stackoverflow.com/questions/34835082/how-to-debug-using-npm-run-scripts-from-vscode

## Database Permissions

In order for Data-Doc to connect to its own repository, and to other databases, you'll need to set up SQL permissions. The minimal permissions can be created by tailoring the following script:

``` sql
-- Data Doc User
USE [master]
GO
CREATE LOGIN [data-doc-user] WITH PASSWORD = '<password>';  -- insert password here

-- Master database permission required
USE [Master]
GO
GRANT VIEW SERVER STATE to [data-doc-user]

-- Permissions in data-doc database
USE [data-doc]
GO
CREATE USER [data-doc-user] FOR LOGIN [data-doc-user]
EXEC sp_addrolemember N'db_owner', N'data-doc-user'     -- data-doc-user is db owner of data-doc

-- Permissions in any other database for data-doc-user to read metadata
USE [<database>]
GO
CREATE USER [data-doc-user] FOR LOGIN [data-doc-user]
GRANT VIEW DATABASE STATE to [data-doc-user]
EXEC sp_addrolemember N'db_datareader', N'data-doc-user'
GRANT VIEW DEFINITION TO [data-doc-user]
```

## ToDo
The following items are on my to-do list:
- Templated output
- Additional document output formats (MSWord, HTML)
- Page numbers on table-of-contents (current limitation with Puppeteer / Headless Chrome)
- Data preview in UI
- Additional semantics for entities (dimensions, fact tables)
- Graphical entity diagram or business matrix showing high level relationships
- Entity groupings (to allow functional areas or modules to be defined)

David Barone - Apr 2021