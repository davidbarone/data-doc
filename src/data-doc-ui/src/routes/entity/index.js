import { h } from "preact";
import style from "./style.css";
import { useState } from "preact/hooks";
import { MyTabs } from "../../components/myTabs/myTabs";
import { MyTab } from "../../components/myTabs/myTab";
import EntityGeneral from "./entityGeneral";
import EntityAttributes from "./entityAttributes";
import Relationships from "../../components/relationships/relationships";
import Hierarchies from "../../components/hierarchies/hierarchies";
import EntityCalculations from "./entityCalculations";

const Entity = ({ projectId, entityName, index }) => {
  const [tabIndex, setTabIndex] = useState(index ? parseInt(index, 10) : 0);

  const setTab = (t) => {
    setTabIndex(t);
  };

  return (
    <div class={style.home}>
      <h3>Entity Information</h3>

      <MyTabs activeTab={tabIndex} onChangeTab={setTab}>
        <MyTab title="General">
          <EntityGeneral projectId={projectId} entityName={entityName} />
        </MyTab>
        <MyTab title="Attributes">
          <EntityAttributes projectId={projectId} entityName={entityName} />
        </MyTab>
        <MyTab title="Calculations">
          <EntityCalculations projectId={projectId} entityName={entityName} />
        </MyTab>
        <MyTab title="Relationships">
          <Relationships projectId={projectId} entityName={entityName} />
        </MyTab>
        <MyTab title="Hierarchies">
          <Hierarchies projectId={projectId} entityName={entityName} />
        </MyTab>
      </MyTabs>
    </div>
  );
};

export default Entity;
