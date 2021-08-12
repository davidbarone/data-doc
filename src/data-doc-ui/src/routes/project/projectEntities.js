import { h } from "preact";
import style from "./style.css";
import { useState, useEffect } from "preact/hooks";
import { getEntities, scanProject } from "../../utils/apiFacade";
import MyTable from "../../components/myTable/myTable";
import MyButton from "../../components/myButton/myButton";
import MyInput from "../../components/myInput/myInput";

const ProjectEntities = ({ projectId }) => {
  const [entities, setEntities] = useState([]);
  const [includeInactive, setIncludeInactive] = useState(false);

  const toggleInactive = e => {
    let checked = e.target.checked;
    setIncludeInactive(checked);
    refreshEntities();
  };
  
  const refreshEntities = () => {
    getEntities(projectId).then((e) => {
      if (!includeInactive) {
        setEntities(e.filter(ent=>ent.isActive))
      } else {
        setEntities(e)
      }
    });
  }

  useEffect(() => {
    refreshEntities();
  }, [includeInactive]);

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

      <input type="checkbox" name="includeInactive" onClick =  {toggleInactive}></input>Include Inactive
        
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
