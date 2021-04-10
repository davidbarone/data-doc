import { h } from "preact";
import style from "./style.css";
import { useState, useEffect } from "preact/hooks";
import { getValueGroups, createValueGroup } from "../../utils/apiFacade";
import MyDropdown from "../myDropdown/myDropdown";
import MyButton from "../myButton/myButton";

const ValueGroups = ({ projectId, attribute, setAttribute }) => {
  const [valueGroups, setValueGroups] = useState([]);

  const refreshData = () => {
    return getValueGroups(projectId).then((e) => {
      setValueGroups(e);
    });
  };

  const addValueGroup = () => {
    let valueGroupName = window.prompt("Enter value group name:");
    if (valueGroupName !== null && valueGroupName !== "") {
      createValueGroup({
        projectId: parseInt(projectId, 10),
        valueGroupName,
      }).then(() => refreshData());
    }
  };

  useEffect(() => {
    refreshData();
  });

  return (
    <div>
      <div class={style.valueGroupFrame}>
        <MyDropdown
          name="valueGroupId"
          values={valueGroups.map((vg) => vg.valueGroupId)}
          selectedValue={attribute.valueGroupId}
          texts={valueGroups.map((vg) => vg.valueGroupName)}
          target={attribute}
          setTarget={setAttribute}
          nullText="[Select value group...]"
          label="Value Group"
        />
      </div>
      <div class={style.valueGroupFrame}>
        <MyButton
          label="Add"
          visible={attribute.valueGroupId === null}
          action={(e) => {
            addValueGroup(e);
            e.preventDefault();
          }}
        />
        <MyButton label="Delete" visible={attribute.valueGroupId !== null} />
      </div>
    </div>
  );
};

export default ValueGroups;
