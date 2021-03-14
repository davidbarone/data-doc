# data-doc

Data-Doc is a simple metadata repository application for SQL Server databases. It is especially useful for documenting data warehouses.

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
Data-Doc can store metadata for multiple databases. These are configured as individual `projects`. Each project defines a connection string to a source database. Once a project has been set up, the source database can be scanned to pick up the built-in metadata that SQL Server provides about tables, columns, and relationships.

## Adding Business Metadata
Once the source database has been scanned for its physical metadata (i.e. column names, data types etc), Data-Doc allows business semantics to be overlaid. Typically, business descriptions can be defined for both entities (tables, views) and attributes (columns).

## Documentation
The REST API provides a documentation endpoint, to document individual projects. The default output is in PDF format, and this is obtained via <a href="https://www.puppeteersharp.com/index.html">PuppeteerSharp</a> and `Headless Chrome`. The output is currently fixed in code. An example using the AdventureWorks DW2019 database can be found <a href='https://github.com/davidbarone/data-doc/blob/main/docs/AdventureWorks DW2019.pdf'>here</a>.

## ToDo
The following items are on my to-do list:
- Templated output
- Additional document output formats (MSWord, HTML)
- Page numbers on table-of-contents (current limitation with Puppeteer / Headless Chrome)
- UI (currently you must use Swagger interface to manipulate the data. A full single page application is planned next)

David Barone - Mar 2021