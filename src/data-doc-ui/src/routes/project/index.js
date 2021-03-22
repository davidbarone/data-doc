import { h } from "preact";
import style from "./style.css";
import { useState, useEffect } from "preact/hooks";
import { getProject, updateProject } from "../../utils/apiFacade";
import { Tabs } from "../../components/tabs/tabs";
import { Tab } from "../../components/tabs/tab";
import ProjectGeneral from "./projectGeneral";

const Project = ({ projectId }) => {
  const [project, setProject] = useState({});
  const [tabIndex, setTabIndex] = useState(-1);

  useEffect(() => {
    getProject(projectId).then((p) => setProject(p));
  }, []);

  const onInput = (e) => {
    const newValue = { ...project, [e.target.name]: e.target.value };
    setProject(newValue);
  };

  const onSubmit = (e) => {
    updateProject(project.projectId, project);
    e.preventDefault();
  };

  const setTab = (t) => {
    setTabIndex(t);
  };

  return (
    <div class={style.home}>
      <h1>Project: {project.projectId}</h1>

      <Tabs activeTab={tabIndex} onChangeTab={setTab}>
        <Tab title="General">
          <ProjectGeneral projectId={projectId} />
        </Tab>
        <Tab title="Entities">Item 2</Tab>
      </Tabs>
    </div>
  );
};

export default Project;
