import { h } from "preact";
import style from "./style.css";
import { useState, useEffect } from "preact/hooks";
import {
  getAttribute,
  searchAttributes,
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
import MyTable from "../myTable/myTable";

const Attribute = ({ projectId, entityName, attributeName }) => {
  const [attribute, setAttribute] = useState({});
  const [search, setSearch] = useState([]);

  const refreshData = () => {
    return getAttribute(projectId, entityName, attributeName)
      .then((e) => {
        setAttribute(e);
        console.log(e);
      })
      .then(() => searchAttr(projectId, attributeName));
  };

  const searchAttr = (projectId, attributeName) => {
    if (attributeName) {
      return searchAttributes(projectId, attributeName).then((e) => {
        setSearch(e);
      });
    }
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
    projectId,
    entityName,
    attributeName,
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
      valueGroupId
    ).then(() => refreshData());
  };

  const unsetDescConfig = (projectId, entityName, attributeName, descScope) => {
    return unsetAttributeDescConfig(
      projectId,
      entityName,
      attributeName,
      descScope
    ).then(() => refreshData());
  };

  const getSearchButtons = (attributeDetail) => {
    return [
      setsearchProjectGlobalButton(
        attributeDetail.projectId,
        attributeDetail.entityName,
        attributeDetail.attributeName,
        attributeDetail.attributeDesc,
        attributeDetail.attributeComment,
        attributeDetail.valueGroupId,
        attributeDetail.descScope
      ),
      unsetSearchAttrButton(
        attributeDetail.projectId,
        attributeDetail.entityName,
        attributeDetail.attributeName,
        attributeDetail.descScope
      ),
    ];
  };

  const setsearchProjectGlobalButton = (
    projectId,
    entityName,
    attributeName,
    attributeDesc,
    attributeComment,
    valueGroupId,
    descScope
  ) => {
    return (
      <MyButton
        visible={descScope === "Local" || descScope === "Project"}
        title="Make this description the project or global level description."
        label={descScope === "Local" ? "Project" : "Global"}
        name="setDescSearch"
        action={(e) => {
          if (
            confirm(
              "Are you sure you want to set this description as project / global level description?"
            )
          ) {
            setDescConfig(
              projectId,
              entityName,
              attributeName,
              descScope === "Local" ? "Project" : "Global",
              attributeDesc,
              attributeComment,
              valueGroupId
            ).then(() => {
              unsetDescConfig(projectId, entityName, attributeName, descScope);
            });
          }
          e.preventDefault();
        }}
      />
    );
  };

  const unsetSearchAttrButton = (
    projectId,
    entityName,
    attributeName,
    descScope
  ) => {
    return (
      <MyButton
        visible={descScope !== "Undefined"}
        title="Delete this description."
        label={"Delete"}
        name="deleteDescSearch"
        action={(e) => {
          if (confirm("Are you sure you want to remove this description?")) {
            unsetDescConfig(projectId, entityName, attributeName, descScope);
          }
          e.preventDefault();
        }}
      />
    );
  };

  useEffect(() => {
    refreshData();
  }, [attributeName]);

  const onSubmit = () => {};

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
            This attribute name is used in the following places:
          </span>

          <MyTable
            data={search}
            mapping={{
              "Entity Name": (s) => s.entityName,
              "Attribute Name": (s) => s.attributeName,
              "Attribute Desc": (s) => s.attributeDesc,
              "Description Scope": (s) => s.descScope,
              "Value Group Id": (s) => s.valueGroupId,
              Actions: (s) => getSearchButtons(s),
            }}
          />

          <span class={style.descScopeMessage}>
            The current description and comment are at: [{attribute.descScope}]
            scope.
          </span>

          <MyButton
            visible={true}
            label={`Delete Description and Comment at [${attribute.descScope}] Scope`}
            name="deleteDesc"
            action={(e) => {
              unsetDescConfig(
                attribute.projectId,
                attribute.entityName,
                attributeName,
                attribute.descScope
              );
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
                projectId,
                entityName,
                attributeName,
                attribute.descScope,
                attribute.attributeDesc,
                attribute.attributeComment,
                parseInt(attribute.valueGroupId, 10) || null
              ).then(() => refreshData());
              e.preventDefault();
            }}
          />
        </fieldset>
      </form>
    </div>
  );
};

export default Attribute;
