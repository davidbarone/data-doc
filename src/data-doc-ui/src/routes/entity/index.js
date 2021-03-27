import { h } from "preact";
import style from "./style.css";
import { getEntity, updateEntity } from "../../utils/apiFacade";
import { useState, useEffect } from "preact/hooks";

const Entity = ({ projectId, entityName }) => {
  const [entity, setEntity] = useState({});

  useEffect(() => {
    getEntity(projectId, entityName).then((e) => setEntity(e));
  }, []);

  const onInput = (e) => {
    const newValue = { ...entity, [e.target.name]: e.target.value };
    console.log(newValue);
    setEntity(newValue);
  };

  const onSubmit = (e) => {
    updateEntity(projectId, entityName, entity);
    e.preventDefault();
  };

  return (
    <div class={style.home}>
      <h3>Entity Information</h3>
      <form onSubmit={onSubmit}>
        <div class={style.field}>
          <label>Entity Name:</label>
          <input
            readOnly
            type="text"
            name="entityName"
            value={entity.entityName}
          />
        </div>

        <div class={style.field}>
          <label>Entity Alias:</label>
          <input
            type="text"
            name="entityAlias"
            value={entity.entityAlias}
            onInput={onInput}
          />
        </div>

        <div class={style.field}>
          <label>Entity Description:</label>
          <input
            type="text"
            name="entityDesc"
            value={entity.entityDesc}
            onInput={onInput}
          />
        </div>

        <div class={style.field}>
          <label>Entity Comment:</label>
          <textarea
            name="entityComment"
            value={entity.entityComment}
            onInput={onInput}
          />
        </div>

        <div class={style.field}>
          <label>Show Data?</label>
          <input
            type="checkbox"
            name="showData"
            checked={entity.showData}
            onInput={onInput}
          />
        </div>

        <button type="submit">Submit</button>
        <a href="/projects">Back to projects</a>
      </form>
    </div>
  );
};

export default Entity;
