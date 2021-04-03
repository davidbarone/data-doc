import { h } from "preact";
import style from "./style.css";
import { useState, useEffect } from "preact/hooks";
import { getAttributes } from "../../utils/apiFacade";
import MyTable from "../../components/myTable/myTable";

const EntityAttributes = ({ projectId, entityName }) => {
  const [attributes, setAttributes] = useState([]);

  useEffect(() => {
    getAttributes(projectId, entityName).then((e) => setAttributes(e));
  }, []);

  const getAttributeUrl = (attributeName) =>
    `/attribute/${projectId}/${entityName}/${attributeName}`;

  const getAttributeLink = (attributeName) => (
    <a href={getAttributeUrl(attributeName)}>{attributeName}</a>
  );

  return (
    <div>
      <h3>Attributes</h3>
      <MyTable
        data={attributes}
        mapping={{
          "Attribute Name": (r) => getAttributeLink(r.attributeName),
          "Data Type": (r) => r.dataTypeDesc,
          Nullable: (r) => r.isNullable,
          Description: (r) => r.attributeDesc,
        }}
      />
    </div>
  );
};

export default EntityAttributes;
