import { h } from "preact";
import style from "./style.css";
import { useState } from "preact/hooks";
import { Tabs } from "../../components/tabs/tabs";
import { Tab } from "../../components/tabs/tab";
import EntityGeneral from "./entityGeneral";

const Entity = ({ projectId, entityName }) => {
  const [tabIndex, setTabIndex] = useState(0);

  const setTab = (t) => {
    setTabIndex(t);
  };

  return (
    <div class={style.home}>
      <h3>Entity Information</h3>

      <Tabs activeTab={tabIndex} onChangeTab={setTab}>
        <Tab title="General">
          <EntityGeneral projectId={projectId} entityName={entityName} />
        </Tab>
        <Tab title="Attributes">To Do</Tab>
        <Tab title="Relationships">To Do</Tab>
      </Tabs>
    </div>
  );
};

export default Entity;
