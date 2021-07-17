import { h } from "preact";
import style from "./style.css";
import { useState, useEffect } from "preact/hooks";
import {
  getAttributeHierarchies,
  scanAttributeHierarchies,
  deleteAttributeHierarchies,
} from "../../utils/apiFacade";
import MyButton from "../myButton/myButton";
import MyTable from "../myTable/myTable";

const Hierarchies = ({ projectId, entityName }) => {
  const [hierarchies, setHierarchies] = useState([]);

  const refreshData = () => {
    getAttributeHierarchies(projectId, entityName).then((e) => {
      setHierarchies(e);
    });
  };

  useEffect(() => {
    refreshData();
  }, []);

  return (
    <div>
      <h3>Hierarchies</h3>

      <MyButton
        action={() => {
          scanAttributeHierarchies(projectId, entityName).then(() =>
            refreshData()
          );
        }}
        label="Scan Hierarchies"
      />

      <MyButton
        action={() => {
          deleteAttributeHierarchies(projectId, entityName).then(() =>
            refreshData()
          );
        }}
        label="Delete Hierarchies"
      />

      <MyTable
        data={hierarchies}
        mapping={{
          "Parent Attribute Name": (h) => h.parentAttributeName,
          "Child Attribute Name": (h) => h.childAttributeName,
          "1:1 Relationship": (h) => (h.isOneToOneRelationship ? "Yes" : "No"),
          "Is Root": (h) => (h.isRoot ? "Yes" : "No"),
        }}
      />
    </div>
  );
};

export default Hierarchies;
