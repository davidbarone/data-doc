import { h } from "preact";
import style from "./style.css";
import { useState, useEffect } from "preact/hooks";
import { getProject, updateProject } from "../../utils/apiFacade";
import { MyTabs } from "../../components/myTabs/myTabs";
import { MyTab } from "../../components/myTabs/myTab";
import ProjectGeneral from "./projectGeneral";
import ProjectEntities from "./projectEntities";

const Project = ({ projectId, index }) => {
  const [project, setProject] = useState({});
  const [tabIndex, setTabIndex] = useState(index ? parseInt(index, 10) : 0);

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

      <MyTabs activeTab={tabIndex} onChangeTab={setTab}>
        <MyTab title="General">
          <ProjectGeneral projectId={projectId} />
        </MyTab>
        <MyTab title="Entities">
          <ProjectEntities projectId={projectId} />
        </MyTab>
      </MyTabs>
    </div>
  );
};

export default Project;
