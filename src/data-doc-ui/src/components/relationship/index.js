import { h } from "preact";
import style from "./style.css";
import { useState, useEffect } from "preact/hooks";
import {
  getRelationship,
  getEntities,
  updateRelationship,
} from "../../utils/apiFacade";
import MyInput from "../myInput/myInput";
import MyButton from "../myButton/myButton";
import MyDropdown from "../myDropdown/myDropdown";

const Relationship = ({ projectId, relationshipId }) => {
  const [relationship, setRelationship] = useState({});
  const [entities, setEntities] = useState([]);

  const refreshData = () => {
    // relationships
    if (relationshipId) {
      getRelationship(relationshipId).then((r) => {
        setRelationship(r);
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

  const onSubmit = (e) => {};

  const save = () => {
    return updateRelationship(relationshipId, relationship).then(() => {});
  };

  return (
    <div>
      <h3>Relationship: {relationship.relationshipName}</h3>
      <form>
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
          values={entities.map((e) => e.entityName)}
          target={relationship}
          setTarget={setRelationship}
          selectedValue={relationship.referencedEntityName}
          name="referencedEntityName"
          label="Referenced Entity"
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
