import { h } from "preact";
import style from "./style.css";
import { useState, useEffect } from "preact/hooks";
import { getEntities, scanProject } from "../../utils/apiFacade";
import MyTable from "../../components/myTable/myTable";
import MyButton from "../../components/myButton/myButton";

const ProjectEntities = ({ projectId }) => {
  const [entities, setEntities] = useState([]);

  useEffect(() => {
    getEntities(projectId).then((e) => setEntities(e));
  }, []);

  const getEntityUrl = (entity) =>
    `/entity/${projectId}/${entity.entityName}/0`;
  const getEntityLink = (entity) => (
    <a href={getEntityUrl(entity)}>{entity.entityName}</a>
  );

  return (
    <div>
      <h3>Entities</h3>
      <MyButton
        action={(e) => {
          scanProject(projectId)
            .then(() => getEntities(projectId))
            .then((e) => setEntities(e));
          e.preventDefault();
        }}
        label="Scan"
      />
      <MyTable
        data={entities}
        mapping={{
          "Entity Name": (r) => getEntityLink(r),
          "Entity Alias": (r) => r.entityAlias,
          "Entity Type": (r) => r.entityType,
          "Entity Description": (r) => r.entityDesc,
          "Show Data": (r) => (r.showData ? "Yes" : "No"),
          "Show Definition": (r) => (r.showDefinition ? "Yes" : "No"),
          "Row Count": (r) => r.rowCount,
          Active: (r) => (r.isActive ? "Yes" : "No"),
        }}
      />
    </div>
  );
};

export default ProjectEntities;
