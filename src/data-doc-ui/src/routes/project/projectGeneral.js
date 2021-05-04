import { h } from "preact";
import style from "./style.css";
import { useState, useEffect } from "preact/hooks";
import {
  getProject,
  updateProject,
  getDownloadUrl,
  getBackupUrl,
  restoreProject,
} from "../../utils/apiFacade";
import MyInput from "../../components/myInput/myInput";
import MyButton from "../../components/myButton/myButton";
import MyFileUploader from "../../components/myFileUploader/myFileUploader";

const ProjectGeneral = ({ projectId }) => {
  const [project, setProject] = useState({});

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

  return (
    <div>
      <h3>General Information</h3>
      <form>
        <MyInput
          name="projectName"
          target={project}
          setTarget={setProject}
          label="Project Name"
          type="input"
        />
        <MyInput
          name="projectDesc"
          target={project}
          setTarget={setProject}
          label="Project Description"
          type="input"
        />
        <MyInput
          name="projectComment"
          target={project}
          setTarget={setProject}
          label="Project Comment"
          type="input"
          rows="10"
        />
        <MyInput
          name="connectionString"
          target={project}
          setTarget={setProject}
          label="Connection String"
          type="input"
        />
        <MyInput
          name="isActive"
          target={project}
          setTarget={setProject}
          label="Active"
          type="checkbox"
        />
        <MyButton
          action={(e) => {
            onSubmit(e);
            e.preventDefault();
          }}
          label="Submit"
        />
        | <a href={getDownloadUrl(project.projectId)}>Document</a> |
        <a href={getBackupUrl(project.projectId)}>Backup</a> |
        <MyFileUploader
          label="Restore"
          name="restore"
          action={(event, file) => {
            restoreProject(projectId, file).then(() => {
              getProject(projectId).then((p) => setProject(p));
            });
          }}
        />
        | <a href="/projects">Back to projects</a>
      </form>
    </div>
  );
};

export default ProjectGeneral;
