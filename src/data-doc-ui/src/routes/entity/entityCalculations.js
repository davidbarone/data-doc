import { h } from "preact";
import style from "./style.css";
import { useState, useEffect } from "preact/hooks";
import { deleteCalculation, getCalculations } from "../../utils/apiFacade";
import MyTable from "../../components/myTable/myTable";
import MySlider from "../../components/mySlider/mySlider";
import Calculation from "../../components/calculation";
import MyButton from "../../components/myButton/myButton";

const EntityCalculations = ({ projectId, entityName }) => {
  const [calculations, setCalculations] = useState([]);
  const [selectedCalculationId, setSelectedCalculationId] = useState(0);
  const [slider, setSlider] = useState(false);

  const refreshData = () => {
    getCalculations(projectId, entityName).then((e) => setCalculations(e));
  };

  useEffect(() => {
    refreshData();
  }, []);

  const actionButtons = (calculation) => {
    return [
      <MyButton
        action={() => {
          setSelectedCalculationId(calculation.calculationId);
          setSlider(true);
        }}
        label={"Edit"}
      />,
      <MyButton
        action={() => {
          deleteCalculation(calculation.calculationId).then(() =>
            refreshData()
          );
        }}
        label={"Delete"}
      />,
    ];
  };

  return (
    <div>
      <h3>Calculations</h3>

      <MyButton
        action={() => {
          // Set up dummy / empty calculation
          setSelectedCalculationId(0);
          setSlider(true);
        }}
        label="Add"
      />

      <MyTable
        data={calculations}
        mapping={{
          Actions: (c) => actionButtons(c),
          "Calculation Id": (c) => c.calculationId,
          "Calculation Name": (c) => c.calculationName,
          "Calculation Description": (c) => c.calculationDesc,
        }}
      />
      <MySlider state={[slider, setSlider]} onClose={() => refreshData()}>
        <Calculation
          projectId={projectId}
          entityName={entityName}
          calculationId={selectedCalculationId}
        />
      </MySlider>
    </div>
  );
};

export default EntityCalculations;
