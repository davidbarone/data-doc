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
import Btn from "../../components/btn/btn";
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
    return unsetAttributeDescConfig(
      projectId,
      entityName,
      attributeName
    ).then(() => refreshData());
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
        <MyInput
          name="attributeName"
          target={attribute}
          label="Attribute Name"
          disabled
        />

        <fieldset>
          <legend>Config</legend>
          <MyInput
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
          <MyInput
            name="IsPrimaryKey"
            disabled={attribute.attributePrimaryKeyConfigId === null}
            target={attribute}
            label="Primary Key"
            type="checkbox"
            onInputHook={(e) => setPrimaryKeyConfig(e.target.checked)}
          />
          <Btn
            visible={attribute.attributePrimaryKeyConfigId === null}
            action={() => {
              setPrimaryKeyConfig(attribute.isPrimaryKey);
              return false;
            }}
            label="Set"
          />
          <Btn
            visible={attribute.attributePrimaryKeyConfigId !== null}
            action={() => {
              unsetPrimaryKeyConfig();
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
          <Btn
            visible={true}
            label={`Delete current description and comment at [${attribute.descScope}] scope`}
            name="deleteDesc"
            action={() => unsetDescConfig()}
          />

          <MyDropdown
            values={["Undefined", "Local", "Project", "Global"]}
            selectedValue={attribute.descScope}
            name="DescScope"
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

          <Btn
            visible={true}
            label="Save"
            name="deleteDesc"
            action={() => {
              setDescConfig(
                attribute.attributeDesc,
                attribute.attributeComment
              );
            }}
          />
        </fieldset>
      </form>

      <a href={getEntityUrl()}>Back to entity</a>
    </div>
  );
};

export default Attribute;
