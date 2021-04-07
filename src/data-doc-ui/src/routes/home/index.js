import { h } from "preact";
import style from "./style.css";

const Home = () => (
  <div class={style.home}>
    <h1>Data-Doc</h1>
    <p>
      Data-Doc is a simple metadata repository application for SQL Server
      databases. It is especially useful for documenting data warehouses. It
      automatically pulls in schema metadata using built-in dynamic management
      views (DMVs), and allows DBAs, BI professionals, data stewards, and other
      data professionals to assign business metadata.
    </p>
    <h3>Metadata Repository</h3>
    <p>
      A metadata repository is simply a database that holds metadata regarding
      some other data (normally a database). The data normally stored in a
      metadata repository is normally split between: Business metadata Technical
      metadata Operational metadata
    </p>
    <h3>Business Metadata</h3>
    <p>
      Business metadata includes things like:
      <ol>
        <li>Business vocabularies, and terminology</li>
        <li>Business definitions of the data elements</li>
        <li>
          Rules, metrics KPIs and other higher level abstractions of the data
        </li>
      </ol>
    </p>
    <h3>Technical Metadata</h3>
    <p>
      Technical metadata typically includes:
      <ol>
        <li>Details of physical objects, tables, entities</li>
        <li>Details of columns, data types, sizes, ranges</li>
        <li>Primary key information, nullable indicators</li>
        <li>Relationships, foreign keys, cardinality of joins</li>
        <li> Object dependencies, data lineage</li>
      </ol>
    </p>
    <h3>Operational Metadata</h3>
    <p>
      Operational metadata typically includes:
      <ol>
        <li>Table sizes, disk space utilisation</li>
        <li>Query metrics, frequency, duration</li>
      </ol>
    </p>
    <h3>Projects</h3>
    <p>
      Data-Doc can store metadata for multiple databases. These are configured
      as an individual project. Each project defines a connection string to a
      source database. Once a project has been set up, the source database can
      be scanned to pick up the built-in metadata that SQL Server provides about
      tables, columns, and relationships.
    </p>
  </div>
);

export default Home;
