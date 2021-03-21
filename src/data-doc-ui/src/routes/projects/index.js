import { h } from 'preact';
import style from './style.css';
import { useState, useEffect } from 'preact/hooks';
import { getProjects } from "../../utils/apiFacade";

const Projects = () => {
    const [projects, setProjects] = useState([]);

    useEffect(() => {
        getProjects().then(p => setProjects(p));
    },[]);

    return (
    <div class={style.home}>
        <h1>Projects</h1>
            <p>This is the Projects component.</p>
            
            <table className="table">
            <thead>
                <tr>
                    <th>Project ID</th>
                    <th>Description</th>
                    <th>&nbsp;</th>
                </tr>
            </thead>
            <tbody>
                {projects.map(p => {
                    return (
                        <tr key={p.projectId}>
                            <td>
                                <a href={`/project/${p.projectId}`} projectId={p.projectId}>{p.projectId}</a>
                            </td>
                            <td>
                                {p.projectDesc}
                            </td>
                            <td> &nbsp;
                            </td>
                        </tr>
                    );
                })}
            </tbody>
        </table>            
        </div>
    )
};

export default Projects;
