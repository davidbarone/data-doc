import { h } from "preact";
import style from "./style.css";
import { useState, useEffect } from "preact/hooks";
import {
  getRelationship,
  getEntities,
  getAttributes,
  updateRelationship,
  createRelationship,
} from "../../utils/apiFacade";
import MyInput from "../myInput/myInput";
import MyButton from "../myButton/myButton";
import MyDropdown from "../myDropdown/myDropdown";

const Relationship = ({ projectId, relationshipId }) => {
  const [relationship, setRelationship] = useState({});
  const [entities, setEntities] = useState([]);
  const [parentAttributes, setParentAttributes] = useState([]);
  const [referencedAttributes, setReferencedAttributes] = useState([]);

  const refreshData = () => {
    // relationships
    if (relationshipId >= 0) {
      getRelationship(relationshipId).then((r) => {
        setRelationship(r);
      });
    } else {
      setRelationship({
        projectId: parseInt(projectId, 10),
      });
    }

    // Entities
    getEntities(projectId).then((e) => {
      setEntities(e);
    });
  };

  useEffect(() => {
    refreshData();
  }, [relationshipId]);

  useEffect(() => {
    getAttributes(projectId, relationship.parentEntityName).then((a) =>
      setParentAttributes(a)
    );
  }, [relationship.parentEntityName, projectId]);

  useEffect(() => {
    getAttributes(projectId, relationship.referencedEntityName).then((a) =>
      setReferencedAttributes(a)
    );
  }, [relationship.referencedEntityName, projectId]);

  const save = () => {
    if (relationshipId >= 0) {
      return updateRelationship(relationshipId, relationship).then(() => {});
    }
    return createRelationship(relationship).then(() => {});
  };

  return (
    <div>
      <h3>Relationship: {relationship.relationshipName}</h3>
      <form>
        <MyInput
          name="relationshipId"
          disabled={true}
          target={relationship}
          setTarget={setRelationship}
          label="Relationship Id"
          type="input"
        />

        <MyInput
          name="projectId"
          disabled={true}
          target={relationship}
          setTarget={setRelationship}
          label="Project Id"
          type="input"
        />

        <MyInput
          name="relationshipName"
          disabled={false}
          target={relationship}
          setTarget={setRelationship}
          label="Relationship Name"
          type="input"
        />

        <MyDropdown
          values={entities.map((e) => e.entityName)}
          target={relationship}
          setTarget={setRelationship}
          selectedValue={relationship.parentEntityName}
          name="parentEntityName"
          label="Parent Entity"
        />

        <MyDropdown
          values={parentAttributes.map((a) => a.attributeName)}
          multiple
          size="5"
          target={relationship}
          setTarget={setRelationship}
          selectedValue={relationship.parentAttributeNames}
          name="parentAttributeNames"
          label="Parent Attributes"
        />

        <MyDropdown
          values={entities.map((e) => e.entityName)}
          target={relationship}
          setTarget={setRelationship}
          selectedValue={relationship.referencedEntityName}
          name="referencedEntityName"
          label="Referenced Entity"
        />

        <MyDropdown
          values={referencedAttributes.map((a) => a.attributeName)}
          multiple
          size="5"
          target={relationship}
          setTarget={setRelationship}
          selectedValue={relationship.referencedAttributeNames}
          name="referencedAttributeNames"
          label="Referenced Attributes"
        />

        <MyButton
          visible={true}
          label="Update"
          name="update"
          action={(e) => {
            save();
            e.preventDefault();
          }}
        />
      </form>
    </div>
  );
};

export default Relationship;
