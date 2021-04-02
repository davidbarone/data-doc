import { h } from "preact";
import style from "./style.css";
import { useState, useEffect } from "preact/hooks";
import {
  getAttribute,
  setAttributeConfig,
  unsetAttributeConfig,
} from "../../utils/apiFacade";
import Field from "../../components/field/field";
import Btn from "../../components/btn/btn";

const Attribute = ({ projectId, entityName, attributeName }) => {
  const [attribute, setAttribute] = useState({});
  const [isActive, setIsActive] = useState(null); // true/false/null

  const refreshData = () => {
    return getAttribute(projectId, entityName, attributeName).then((e) => {
      setAttribute(e);
      console.log(e);
    });
  };

  const setConfig = (isActive) => {
    return setAttributeConfig(
      projectId,
      entityName,
      attributeName,
      isActive
    ).then(() => refreshData());
  };

  const unsetConfig = () => {
    return unsetAttributeConfig(projectId, entityName, attributeName).then(() =>
      refreshData()
    );
  };

  useEffect(() => {
    refreshData();
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
          disabled
        />

        <fieldset>
          <legend>Config</legend>
          <Field
            name="isActive"
            disabled={attribute.attributeConfigId === null}
            target={attribute}
            label="Active"
            type="checkbox"
            onInputHook={(e) => setConfig(e.target.checked)}
          />
          <Btn
            visible={attribute.attributeConfigId === null}
            action={() => {
              setConfig(true);
              return false;
            }}
            label="Set"
          />
          <Btn
            visible={attribute.attributeConfigId !== null}
            action={() => {
              unsetConfig();
            }}
            label="Unset"
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
