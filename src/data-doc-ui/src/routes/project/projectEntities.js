import { h } from "preact";
import style from "./style.css";
import { useState, useEffect } from "preact/hooks";
import { getEntities } from "../../utils/apiFacade";

const ProjectEntities = ({ projectId }) => {
  const [entities, setEntities] = useState([]);

  useEffect(() => {
    getEntities(projectId).then((e) => setEntities(e));
  }, []);

  const getEntityUrl = (entity) => `/entity/${projectId}/${entity.entityName}`;

  return (
    <div>
      <h3>Entities</h3>
      <table>
        <thead>
          <tr>
            <th>Entity Name</th>
            <th>Entity Alias</th>
            <th>Entity Type</th>
            <th>Entity Description</th>
          </tr>
        </thead>
        <tbody>
          {entities.map((entity) => (
            <tr>
              <td>
                <a href={getEntityUrl(entity)}>{entity.entityName}</a>
              </td>
              <td>{entity.entityName}</td>
              <td>{entity.entityAlias}</td>
              <td>{entity.entityType}</td>
              <td>{entity.entityDesc}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default ProjectEntities;
