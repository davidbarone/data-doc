import { h } from "preact";
import style from "./style.css";
import { useState, useEffect } from "preact/hooks";
import { getEntity, updateEntity } from "../../utils/apiFacade";
import MyInput from "../../components/myInput/myInput";
import MyButton from "../../components/myButton/myButton";

const EntityGeneral = ({ projectId, entityName }) => {
  const [entity, setEntity] = useState({});

  useEffect(() => {
    getEntity(projectId, entityName).then((e) => setEntity(e));
  }, []);

  const onInput = (e) => {
    const val =
      e.target.type === "checkbox" ? e.target.checked : e.target.value;
    const newValue = { ...entity, [e.target.name]: val };
    console.log(newValue);
    setEntity(newValue);
  };

  const submit = (e) => {
    updateEntity(projectId, entityName, entity);
    e.preventDefault();
  };

  return (
    <div>
      <h3>General Information</h3>

      <form>
        <MyInput
          name="entityName"
          target={entity}
          setTarget={setEntity}
          label="Entity Name"
          type="input"
          disabled
        />

        <MyInput
          name="entityAlias"
          target={entity}
          setTarget={setEntity}
          label="Entity Alias"
          type="input"
        />

        <MyInput
          name="entityDesc"
          target={entity}
          setTarget={setEntity}
          label="Entity Description"
          type="input"
        />

        <MyInput
          name="entityComment"
          target={entity}
          setTarget={setEntity}
          label="Entity Comment"
          type="input"
          rows="10"
        />

        <MyInput
          name="showData"
          target={entity}
          setTarget={setEntity}
          label="Show Data"
          type="checkbox"
        />

        <MyInput
          name="showDefinition"
          target={entity}
          setTarget={setEntity}
          label="Show Definition"
          type="checkbox"
        />

        <MyInput
          name="isActive"
          target={entity}
          setTarget={setEntity}
          label="Active"
          type="checkbox"
        />

        <MyButton action={(e) => submit(e)} label="Update" />
        <a href={`/project/${projectId}/1`}>Back to entities</a>
      </form>
    </div>
  );
};

export default EntityGeneral;
