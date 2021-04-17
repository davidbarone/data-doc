import { h } from "preact";
import style from "./style.css";
import { useState, useEffect } from "preact/hooks";
import {
  deleteRelationship,
  getRelationships,
  scanRelationships,
} from "../../utils/apiFacade";
import MyTable from "../myTable/myTable";
import MySlider from "../mySlider/mySlider";
import MyButton from "../myButton/myButton";
import Relationship from "../../components/relationship";

const Relationships = ({ projectId, entityName }) => {
  const [relationships, setRelationships] = useState([]);
  const [selectedRelationship, setSelectedRelationship] = useState(null);
  const [slider, setSlider] = useState(false);

  const refreshData = () => {
    getRelationships(projectId).then((e) => {
      if (entityName) {
        e = e.filter(
          (data) =>
            data.parentEntityName === entityName ||
            data.referencedEntityName === entityName
        );
      }
      setRelationships(e);
    });
  };

  useEffect(() => {
    refreshData();
  }, []);

  const actionButtons = (relationship) => {
    return [
      <MyButton
        action={() => {
          setSelectedRelationship(relationship);
          setSlider(true);
        }}
        label={"Edit"}
      />,
      <MyButton
        action={() => {
          deleteRelationship(relationship.relationshipId).then(() =>
            refreshData()
          );
        }}
        label={"Delete"}
      />,
    ];
  };

  return (
    <div>
      <h3>Relationships</h3>

      <MyButton
        action={() => {
          scanRelationships(projectId).then(() => refreshData());
        }}
        label="Scan Relationships"
      />

      <MyButton
        action={() => {
          // Set up dummy / empty relationship
          setSelectedRelationship(null);
          setSlider(true);
        }}
        label="Add"
      />

      <MyTable
        data={relationships}
        mapping={{
          Actions: (r) => actionButtons(r),
          "Relationship Name": (r) => r.relationshipName,
          "Parent Entity": (r) => r.parentEntityName,
          "Referenced Entity": (r) => r.referencedEntityName,
          "Relationship Type": (r) => (r.isScanned ? "Scanned" : "Manual"),
        }}
      />
      <MySlider state={[slider, setSlider]} onClose={() => refreshData()}>
        <Relationship
          projectId={projectId}
          relationshipId={
            selectedRelationship ? selectedRelationship.relationshipId : -1
          }
        />
      </MySlider>
    </div>
  );
};

export default Relationships;
