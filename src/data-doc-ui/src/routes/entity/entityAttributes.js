import { h } from "preact";
import style from "./style.css";
import { useState, useEffect } from "preact/hooks";
import { getAttributes } from "../../utils/apiFacade";
import MyTable from "../../components/myTable/myTable";
import MySlider from "../../components/mySlider/mySlider";
import Attribute from "../../routes/attribute";
import MyButton from "../../components/myButton/myButton";

const EntityAttributes = ({ projectId, entityName }) => {
  const [attributes, setAttributes] = useState([]);
  const [selectedAttributeName, setSelectedAttributeName] = useState(null);
  const [attributeModal, setAttributeModal] = useState(false);

  const refreshData = () => {
    getAttributes(projectId, entityName).then((e) => setAttributes(e));
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
      <h3>Attributes</h3>

      <MyTable
        data={attributes}
        mapping={{
          "Attribute Name": (r) => editAttributeButton(r.attributeName),
          "Primary Key": (r) => (r.isPrimaryKey ? "Yes" : "No"),
          "Data Type": (r) => r.dataTypeDesc,
          Nullable: (r) => (r.isNullable ? "Yes" : "No"),
          Description: (r) => r.attributeDesc,
          Active: (r) => (r.isActive ? "Yes" : "No"),
        }}
      />
      <MySlider
        state={[attributeModal, setAttributeModal]}
        onClose={() => refreshData()}
      >
        <Attribute
          projectId={projectId}
          entityName={entityName}
          attributeName={selectedAttributeName}
        />
      </MySlider>
    </div>
  );
};

export default EntityAttributes;
