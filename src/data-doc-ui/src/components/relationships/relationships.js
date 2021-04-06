import { h } from "preact";
import style from "./style.css";
import { useState, useEffect } from "preact/hooks";
import { getRelationships } from "../../utils/apiFacade";
import MyTable from "../myTable/myTable";
import MySlider from "../mySlider/mySlider";
import MyButton from "../myButton/myButton";

const Relationships = ({ projectId }) => {
  const [relationships, setRelationships] = useState([]);
  const [selectedAttributeName, setSelectedAttributeName] = useState(null);
  const [attributeModal, setAttributeModal] = useState(false);

  const refreshData = () => {
    getRelationships(projectId).then((e) => setRelationships(e));
  };

  useEffect(() => {
    refreshData();
  }, []);

  const editAttributeButton = (attributeName) => (
    <MyButton
      action={() => {
        setSelectedAttributeName(attributeName);
        setAttributeModal(true);
      }}
      label={attributeName}
    />
  );

  return (
    <div>
      <h3>Relationships</h3>

      <MyTable
        data={relationships}
        mapping={{
          "Relationship Id": (r) => r.relationshipId,
          "Relationship Name": (r) => r.relationshipName,
          "Parent Entity": (r) => r.parentEntityName,
          "Referenced Entity": (r) => r.referencedEntityName,
        }}
      />
      <MySlider
        state={[attributeModal, setAttributeModal]}
        onClose={() => refreshData()}
      >
        To Do
      </MySlider>
    </div>
  );
};

export default Relationships;
