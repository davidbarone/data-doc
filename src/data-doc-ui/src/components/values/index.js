import { h } from "preact";
import style from "./style.css";
import { useState, useEffect } from "preact/hooks";
import {
  getValues,
  createValue,
  updateValue,
  deleteValue,
  scanValues,
} from "../../utils/apiFacade";
import MyTable from "../myTable/myTable";
import MyButton from "../myButton/myButton";

const Values = ({ valueGroupId, attribute }) => {
  const [values, setValues] = useState([]);

  const handleScanValues = () => {
    scanValues(valueGroupId, attribute).then(() => {
      getData(valueGroupId);
    });
  };

  const handleUpdateValue = (e, valueId) => {
    if (valueId) {
      let valueToChange = values.filter((v) => v.valueId === valueId)[0];
      valueToChange.value = e.target.value;
      updateValue(valueId, valueToChange).then(() => {
        getData(valueGroupId);
      });
    } else {
      let valueToCreate = {
        valueGroupId,
        value: e.target.value,
        desc: "",
      };
      createValue(valueToCreate).then(() => {
        getData(valueGroupId);
      });
    }
  };

  const handleUpdateDesc = (e, valueId) => {
    if (valueId) {
      let valueToChange = values.filter((v) => v.valueId === valueId)[0];
      valueToChange.desc = e.target.value;
      updateValue(valueId, valueToChange).then(() => {
        getData(valueGroupId);
      });
    } else {
      let valueToCreate = {
        valueGroupId,
        value: e.target.value,
        desc: "",
      };
      createValue(valueToCreate).then(() => {
        getData(valueGroupId);
      });
    }
  };

  const handleDeleteValue = (valueId) => {
    deleteValue(valueId).then(() => {
      getData(valueGroupId);
    });
  };

  const getData = (valueGroupId) => {
    return getValues(valueGroupId ?? 0).then((e) => {
      e.push({
        valueId: null,
        valueGroupId,
        value: "[new value]",
        desc: "[new description]",
      });
      setValues(e);
    });
  };

  useEffect(() => {
    getData(valueGroupId);
  }, [valueGroupId]);

  return (
    <div>
      <MyButton
        label="Scan"
        visible={valueGroupId !== null}
        action={(e) => {
          handleScanValues();
          e.preventDefault();
        }}
      />

      <MyTable
        visible={valueGroupId !== null}
        data={values}
        mapping={{
          "Value Id": (v) => v.valueId,
          Value: (v) => (
            <input
              type="text"
              name={`${v.valueId}_Value`}
              value={v.value}
              onBlur={(e) => handleUpdateValue(e, v.valueId)}
            />
          ),
          Description: (v) => (
            <textarea
              name={`${v.valueId}_Desc`}
              rows="1"
              onBlur={(e) => handleUpdateDesc(e, v.valueId)}
            />
          ),
          Actions: (v) => (
            <MyButton
              label="Delete"
              action={(e) => {
                handleDeleteValue(v.valueId);
                e.preventDefault();
              }}
            />
          ),
        }}
      />
    </div>
  );
};

export default Values;
