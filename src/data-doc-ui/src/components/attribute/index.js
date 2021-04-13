import { h } from "preact";
import style from "./style.css";
import { useState, useEffect } from "preact/hooks";
import {
  getAttribute,
  setAttributeConfig,
  unsetAttributeConfig,
  setAttributePrimaryKeyConfig,
  unsetAttributePrimaryKeyConfig,
  setAttributeDescConfig,
  unsetAttributeDescConfig,
} from "../../utils/apiFacade";
import MyInput from "../myInput/myInput";
import MyButton from "../myButton/myButton";
import MyDropdown from "../myDropdown/myDropdown";
import ValueGroups from "../valueGroups";

const Attribute = ({ projectId, entityName, attributeName }) => {
  const [attribute, setAttribute] = useState({});

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

  const setPrimaryKeyConfig = (isPrimaryKey) => {
    return setAttributePrimaryKeyConfig(
      projectId,
      entityName,
      attributeName,
      isPrimaryKey
    ).then(() => refreshData());
  };

  const unsetPrimaryKeyConfig = () => {
    return unsetAttributePrimaryKeyConfig(
      projectId,
      entityName,
      attributeName
    ).then(() => refreshData());
  };

  const setDescConfig = (
    descScope,
    attributeDesc,
    attributeComment,
    valueGroupId
  ) => {
    return setAttributeDescConfig(
      projectId,
      entityName,
      attributeName,
      descScope,
      attributeDesc,
      attributeComment,
      parseInt(valueGroupId, 10) || null
    ).then(() => refreshData());
  };

  const unsetDescConfig = () => {
    unsetAttributeDescConfig(
      attribute.projectId,
      attribute.entityName,
      attributeName,
      attribute.descScope
    ).then(() => refreshData());
  };

  useEffect(() => {
    refreshData();
  }, [attributeName]);

  const onSubmit = (e) => {};

  return (
    <div>
      <h3>Attribute: {attribute.attributeName}</h3>
      <form onSubmit={onSubmit}>
        <fieldset>
          <legend>Config</legend>
          <MyInput
            name="isActive"
            disabled={attribute.attributeConfigId === null}
            target={attribute}
            setTarget={setAttribute}
            label="Active"
            type="checkbox"
            onInputHook={(e) => {
              setConfig(e.target.checked);
              e.preventDefault();
            }}
          />
          <MyButton
            visible={attribute.attributeConfigId === null}
            action={(e) => {
              setConfig(true);
              e.preventDefault();
            }}
            label="Set"
          />
          <MyButton
            visible={attribute.attributeConfigId !== null}
            action={(e) => {
              unsetConfig();
              e.preventDefault();
            }}
            label="Unset"
          />
        </fieldset>

        <fieldset>
          <legend>Primary Key</legend>
          <MyInput
            name="IsPrimaryKey"
            disabled={attribute.attributePrimaryKeyConfigId === null}
            target={attribute}
            setTarget={setAttribute}
            label="Primary Key"
            type="checkbox"
            onInputHook={(e) => setPrimaryKeyConfig(e.target.checked)}
          />
          <MyButton
            visible={attribute.attributePrimaryKeyConfigId === null}
            action={(e) => {
              setPrimaryKeyConfig(attribute.isPrimaryKey);
              e.preventDefault();
            }}
            label="Set"
          />
          <MyButton
            visible={attribute.attributePrimaryKeyConfigId !== null}
            action={(e) => {
              unsetPrimaryKeyConfig();
              e.preventDefault();
            }}
            label="Unset"
          />
        </fieldset>

        <fieldset>
          <legend>Descriptions</legend>

          <span class={style.descScopeMessage}>
            The current description and comment are at: [{attribute.descScope}]
            scope.
          </span>
          <MyButton
            visible={true}
            label={`Delete Description and Comment at [${attribute.descScope}] Scope`}
            name="deleteDesc"
            action={(e) => {
              unsetDescConfig();
              e.preventDefault();
            }}
          />

          <MyDropdown
            values={["Undefined", "Local", "Project", "Global"]}
            target={attribute}
            setTarget={setAttribute}
            selectedValue={attribute.descScope}
            name="descScope"
            label="Description Scope"
          />

          <MyInput
            name="attributeDesc"
            target={attribute}
            setTarget={setAttribute}
            label="Attribute Desc"
          />

          <MyInput
            name="attributeComment"
            target={attribute}
            setTarget={setAttribute}
            label="Attribute Comment"
            type="input"
            rows="5"
          />

          <ValueGroups
            projectId={projectId}
            attribute={attribute}
            setAttribute={setAttribute}
          />

          <MyButton
            visible={true}
            label="Update"
            name="deleteDesc"
            action={(e) => {
              setDescConfig(
                attribute.descScope,
                attribute.attributeDesc,
                attribute.attributeComment,
                attribute.valueGroupId
              );
              e.preventDefault();
            }}
          />
        </fieldset>
      </form>
    </div>
  );
};

export default Attribute;
