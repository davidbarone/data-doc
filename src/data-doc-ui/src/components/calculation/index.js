import { h } from "preact";
import style from "./style.css";
import { useState, useEffect } from "preact/hooks";
import {
  getCalculation,
  updateCalculation,
  createCalculation,
} from "../../utils/apiFacade";
import MyInput from "../myInput/myInput";
import MyButton from "../myButton/myButton";

const Calculation = ({ projectId, entityName, calculationId }) => {
  const [calculation, setCalculation] = useState({});

  const refreshData = () => {
    // calculation
    if (calculationId > 0) {
      getCalculation(calculationId).then((c) => {
        setCalculation(c);
      });
    } else {
      setCalculation({
        projectId: parseInt(projectId, 10),
        entityName,
      });
    }
  };

  useEffect(() => {
    refreshData();
  }, [calculationId]);

  const save = () => {
    if (calculationId > 0) {
      return updateCalculation(calculationId, calculation).then(() => {});
    }
    return createCalculation(calculation).then(() => {});
  };

  return (
    <div>
      <h3>Calculation: {calculation.calculationId}</h3>
      <form>
        <MyInput
          name="calculationId"
          disabled={true}
          target={calculation}
          setTarget={setCalculation}
          label="Calculation Id"
          type="input"
        />

        <MyInput
          name="projectId"
          disabled={true}
          target={calculation}
          setTarget={setCalculation}
          label="Project Id"
          type="input"
        />

        <MyInput
          name="calculationName"
          disabled={false}
          target={calculation}
          setTarget={setCalculation}
          label="Calculation Name"
          type="input"
        />

        <MyInput
          name="calculationDesc"
          disabled={false}
          target={calculation}
          setTarget={setCalculation}
          label="Calculation Desc"
          type="input"
        />

        <MyInput
          name="calculationComment"
          disabled={false}
          target={calculation}
          setTarget={setCalculation}
          label="Calculation Comment"
          type="input"
          rows="5"
        />

        <MyInput
          name="formula"
          disabled={false}
          target={calculation}
          setTarget={setCalculation}
          label="Formula"
          type="input"
          rows="10"
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

export default Calculation;
