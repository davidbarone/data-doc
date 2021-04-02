import { h } from "preact";
import style from "./style.css";
import { useState } from "preact/hooks";
import { MyTabs } from "../../components/myTabs/myTabs";
import { MyTab } from "../../components/myTabs/myTab";
import EntityGeneral from "./entityGeneral";
import EntityAttributes from "./entityAttributes";

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
        <MyTab title="Relationships">To Do</MyTab>
      </MyTabs>
    </div>
  );
};

export default Entity;
