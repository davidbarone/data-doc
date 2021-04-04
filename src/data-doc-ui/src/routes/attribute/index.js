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
import MyInput from "../../components/myInput/myInput";
import MyButton from "../../components/myButton/myButton";
import MyDropdown from "../../components/myDropdown/myDropdown";

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

  const setDescConfig = (descScope, attributeDesc, attributeComment) => {
    let eName = entityName;
    let pId = projectId;

    if (descScope === "Project" || descScope === "Global") {
      eName = "*";
    }

    if (descScope === "Global") {
      pId = -1;
    }

    return setAttributeDescConfig(
      pId,
      eName,
      attributeName,
      attributeDesc,
      attributeComment
    ).then(() => refreshData());
  };

  const unsetDescConfig = () => {
    let eName = attribute.entityName;
    let pId = attribute.projectId;

    if (attribute.descScope === "Project" || attribute.descScope === "Global") {
      eName = "*";
    }

    if (attribute.descScope === "Global") {
      pId = -1;
    }
    return unsetAttributeDescConfig(pId, eName, attributeName).then(() =>
      refreshData()
    );
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
            action={() => unsetDescConfig()}
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
            rows="10"
          />

          <MyButton
            visible={true}
            label="Update"
            name="deleteDesc"
            action={(e) => {
              setDescConfig(
                attribute.descScope,
                attribute.attributeDesc,
                attribute.attributeComment
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
