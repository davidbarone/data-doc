import { h } from 'preact';
import style from './style.css';
import { useState, useEffect } from 'preact/hooks';
import { getProject, updateProject } from "../../utils/apiFacade";

const Project = ({ projectId }) => {
    const [project, setProject] = useState({});

    useEffect(() => {
        getProject(projectId).then(p => setProject(p));
    },[]);

    const onInput = e => {
        const newValue = {...project, [e.target.name]: e.target.value}
        setProject(newValue)
    }
    
    const onSubmit = e => {
        updateProject(project.projectId, project);
        e.preventDefault();
    }
    
    return (
    <div class={style.home}>
        <h1>Project: {project.projectId}</h1>
            <form onSubmit={onSubmit}>
                <div class={style.field}>
                    <label>Project Name:</label>
                    <input type="text" name="projectName" value={project.projectName} onInput={onInput} />
                </div>

                <div class={style.field}>
                    <label>Project Description:</label>
                    <input type="text" name="projectDesc" value={project.projectDesc} onInput={onInput} />
                </div>

                <div class={style.field}>
                    <label>Project Comment:</label>
                    <textarea name="projectComment" value={project.projectComment} rows="6" onInput={onInput} />
                </div>

                <div class={style.field}>
                    <label>Connection String:</label>
                    <input type="text" name="connectionString" value={project.connectionString} onInput={onInput} />
                </div>

                <div class={style.field}>
                    <label>Is Active:</label>
                    <input type="checkbox" name="isActive" checked={project.isActive} onInput={onInput} />
                </div>

                <button type="submit">Submit</button>
                <a href="/projects">Back to projects</a>
            </form>
    </div>
    )
};

export default Project;
