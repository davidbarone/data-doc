import { h } from "preact";
import style from "./style.css";
import { useState, useEffect } from "preact/hooks";
import { getEntities } from "../../utils/apiFacade";
import MyTable from "../../components/myTable/myTable";

const ProjectEntities = ({ projectId }) => {
  const [entities, setEntities] = useState([]);

  useEffect(() => {
    getEntities(projectId).then((e) => setEntities(e));
  }, []);

  const getEntityUrl = (entity) => `/entity/${projectId}/${entity.entityName}`;
  const getEntityLink = (entity) => (
    <a href={getEntityUrl(entity)}>{entity.entityName}</a>
  );

  return (
    <div>
      <h3>Entities</h3>
      <MyTable
        data={entities}
        mapping={{
          "Entity Name": (r) => getEntityLink(r),
          "Entity Alias": (r) => r.entityAlias,
          "Entity Type": (r) => r.entityType,
          "Entity Description": (r) => r.entityDesc,
        }}
      />
    </div>
  );
};

export default ProjectEntities;
