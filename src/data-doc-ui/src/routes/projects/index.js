import { h } from "preact";
import style from "./style.css";
import { useState, useEffect } from "preact/hooks";
import {
  getProjects,
  createProject,
  deleteProject,
} from "../../utils/apiFacade";
import MyTable from "../../components/myTable/myTable";
import MyButton from "../../components/myButton/myButton";

const Projects = () => {
  const [projects, setProjects] = useState([]);

  useEffect(() => {
    getProjects().then((p) => setProjects(p));
  }, []);

  const getProjectLink = (p) => (
    <a href={`/project/${p.projectId}`} projectId={p.projectId}>
      {p.projectId}
    </a>
  );

  const deleteButton = (projectId) => {
    return (
      <MyButton
        label="Delete"
        action={(e) => {
          if (
            confirm(`Are you sure you wish to delete project #${projectId}?`)
          ) {
            deleteProject(projectId)
              .then(() => getProjects())
              .then((p) => setProjects(p));
          }
        }}
      />
    );
  };

  const handleNew = (e) => {
    let project = {
      projectId: 0,
      projectName: "",
      projectDesc: "",
      projectComment: "",
      connectionString: "",
      scanVersion: 0,
      scanUpdatedDt: new Date(),
      configVerion: 0,
      configUpdatedDt: new Date(),
      isActive: true,
    };
    let projectId = null;
    createProject(project).then((data) => {
      projectId = data.projectId;
      window.location = `/project/${projectId}`;
    });
    e.preventDefault();
  };

  return (
    <div class={style.home}>
      <h1>Projects</h1>

      <MyTable
        data={projects}
        mapping={{
          "Project Id": (p) => getProjectLink(p),
          "Project Name": (p) => p.projectName,
          "Project Description": (p) => p.projectDesc,
          "Scan Version": (p) => p.scanVersion,
          "Config Version": (p) => p.configVersion,
          "Scan Last Updated": (p) => p.scanUpdatedDt,
          "Config Last Updated": (p) => p.configUpdatedDt,
          Actions: (p) => deleteButton(p.projectId),
        }}
      />

      <MyButton
        label="New"
        action={() => {
          handleNew();
        }}
      />
    </div>
  );
};

export default Projects;
