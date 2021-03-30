import { h } from "preact";
import style from "./style.css";
import { useState, useEffect } from "preact/hooks";
import { getAttributes } from "../../utils/apiFacade";

const EntityAttributes = ({ projectId, entityName }) => {
  const [attributes, setAttributes] = useState([]);

  useEffect(() => {
    getAttributes(projectId, entityName).then((e) => setAttributes(e));
  }, []);

  const getAttributeUrl = (attributeName) =>
    `/attribute/${projectId}/${entityName}/${attributeName}`;

  return (
    <div>
      <h3>Attributes</h3>
      <table>
        <thead>
          <tr>
            <th>Attribute Name</th>
            <th>Data Type</th>
            <th>Data Length</th>
            <th>Precision</th>
            <th>Scale</th>
            <th>Nullable?</th>
            <th>Description</th>
            <th>Comment</th>
          </tr>
        </thead>
        <tbody>
          {attributes.map((attribute) => (
            <tr>
              <td>
                <a href={getAttributeUrl(attribute.attributeName)}>
                  {attribute.attributeName}
                </a>
              </td>
              <td>{attribute.dataType}</td>
              <td>{attribute.dataLength}</td>
              <td>{attribute.precision}</td>
              <td>{attribute.scale}</td>
              <td>{attribute.isNullable}</td>
              <td>{attribute.attributeDesc}</td>
              <td>{attribute.attributeComment}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default EntityAttributes;
