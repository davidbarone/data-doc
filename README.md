# data-doc

Data-Doc is a simple metadata repository application for SQL Server databases. It is especially useful for documenting data warehouses.

## Metadata Repository

A metadata repository is simply a database that holds metadata regarding some data (normally another database). The data normally stored in a metadata repository is normally split between:

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

## Data-Doc Documentation output
The Data-Doc output is in PDF format. An example using the AdventureWorks DW2019 database can be found <a href='https://github.com/davidbarone/data-doc/blob/main/docs/AdventureWorks DW2019.pdf'>here</a>.

## Publishing Notes

dotnet publish -r win-x64 -c Release --self-contained
dotnet publish -r win-x64 -c Release /p:PublishSingleFile=true


SELECT
    fk.name 'FK Name',
    SCHEMA_NAME(tp.schema_id) + '.' + tp.name ParentEntityName,
    cp.name, cp.column_id,
    SCHEMA_NAME(tr.schema_id) + '.' + tr.name ReferencedEntityName,
    cr.name, cr.column_id
FROM 
    sys.foreign_keys fk
INNER JOIN 
    sys.tables tp ON fk.parent_object_id = tp.object_id
INNER JOIN 
    sys.tables tr ON fk.referenced_object_id = tr.object_id
INNER JOIN 
    sys.foreign_key_columns fkc ON fkc.constraint_object_id = fk.object_id
INNER JOIN 
    sys.columns cp ON fkc.parent_column_id = cp.column_id AND fkc.parent_object_id = cp.object_id
INNER JOIN 
    sys.columns cr ON fkc.referenced_column_id = cr.column_id AND fkc.referenced_object_id = cr.object_id
ORDER BY
    tp.name, cp.column_id