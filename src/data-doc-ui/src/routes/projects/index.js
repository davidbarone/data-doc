import { h } from "preact";
import style from "./style.css";
import { useState, useEffect } from "preact/hooks";
import { getProjects } from "../../utils/apiFacade";
import MyTable from "../../components/myTable/myTable";

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

  return (
    <div class={style.home}>
      <h1>Projects</h1>

      <MyTable
        data={projects}
        mapping={{
          "Project Id": (p) => getProjectLink(p),
          "Project Name": (p) => p.projectName,
          "Project Description": (p) => p.projectDesc,
        }}
      />
    </div>
  );
};

export default Projects;
