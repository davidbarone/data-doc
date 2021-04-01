import { h } from "preact";
import style from "./style.css";
import { useState, useEffect } from "preact/hooks";
import { getAttribute } from "../../utils/apiFacade";
import Field from "../../components/field/field";
import Btn from "../../components/btn/btn";

const Attribute = ({ projectId, entityName, attributeName }) => {
  const [attribute, setAttribute] = useState({});

  useEffect(() => {
    getAttribute(projectId, entityName, attributeName).then((e) =>
      setAttribute(e)
    );
  }, []);

  const onSubmit = (e) => {};

  const getEntityUrl = () => `/entity/${projectId}/${entityName}/1`;

  return (
    <div class={style.home}>
      <h3>Attribute: {attribute.attributeName}</h3>
      <form onSubmit={onSubmit}>
        <Field
          name="attributeName"
          target={attribute}
          label="Attribute Name"
          readOnly
        />

        <fieldset>
          <legend>Active?</legend>
          <Field
            name="isActive"
            readOnly
            target={attribute}
            label="Is Active?"
            type="checkbox"
          />
          <Btn
            visible={true}
            action={() => {
              alert("test");
            }}
            label="Edit"
          />
          <Btn
            visible={true}
            action={() => {
              alert("test");
            }}
            label="Save"
          />

          <Btn
            visible={true}
            action={() => {
              alert("test");
            }}
            label="Reset"
          />
        </fieldset>

        <fieldset>
          <legend>Primary Key</legend>
        </fieldset>

        <fieldset>
          <legend>Descriptions</legend>

          <Field
            name="attributeDesc"
            target={attribute}
            label="Attribute Desc"
          />

          <Field
            name="attributeComment"
            target={attribute}
            label="Attribute Comment"
            type="input"
            rows="5"
          />
        </fieldset>
      </form>

      <a href={getEntityUrl()}>Back to entity</a>
    </div>
  );
};

export default Attribute;
