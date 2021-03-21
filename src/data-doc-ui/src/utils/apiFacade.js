function getProjects() {
    let url = `http://localhost:5000/projects`;
    return fetch(url, {
        mode: "cors",
        headers: {
            'Content-Type':'application/json'
          }        
    })
        .then(response => response.json())
        .then(data => data)
}

function getProject(projectId) {
    let url = `http://localhost:5000/projects/${projectId}`;
    return fetch(url, {
        mode: "cors",
        headers: {
            'Content-Type':'application/json'
          }        
    })
        .then(response => response.json())
        .then(data => data)
}

function updateProject(projectId, project) {
    let url = `http://localhost:5000/projects/${projectId}`;
    return fetch(url, {
        mode: "cors",
        method: "put",
        headers: {
            'Content-Type':'application/json'
        },
        body: JSON.stringify(project)
    })
        .then(response => response.json())
        .then(data => data)
}

export {
    getProjects,
    getProject,
    updateProject
}