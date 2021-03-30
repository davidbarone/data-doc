import { h } from "preact";
import style from "./style.css";
import { useState, useEffect } from "preact/hooks";
import { getAttribute } from "../../utils/apiFacade";
import Field from "../../components/field/field";

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
      <h3>Attribute</h3>
      <form onSubmit={onSubmit}>
        <Field name="attributeName" target={attribute} label="Attribute Name" />
        <Field name="attributeDesc" target={attribute} label="Attribute Desc" />
        <Field
          name="attributeComment"
          target={attribute}
          label="Attribute Comment"
        />
      </form>

      <a href={getEntityUrl()}>Back to entity</a>
    </div>
  );
};

export default Attribute;
