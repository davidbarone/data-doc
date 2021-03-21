import { h } from 'preact';
import style from './style.css';
import { useState, useEffect } from 'preact/hooks';
import { getProject } from "../../utils/apiFacade";

const Project = ({ projectId }) => {
    const [project, setProject] = useState({});

    useEffect(() => {
        getProject(projectId).then(p => setProject(p));
    },[]);

    return (
    <div class={style.home}>
        <h1>Project: {project.projectDesc}</h1>
    </div>
    )
};

export default Project;
