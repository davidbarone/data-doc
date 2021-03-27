function getProjects() {
  let url = `http://localhost:5000/projects`;
  return fetch(url, {
    mode: "cors",
    headers: {
      "Content-Type": "application/json",
    },
  })
    .then((response) => response.json())
    .then((data) => data);
}

function getProject(projectId) {
  let url = `http://localhost:5000/projects/${projectId}`;
  return fetch(url, {
    mode: "cors",
    headers: {
      "Content-Type": "application/json",
    },
  })
    .then((response) => response.json())
    .then((data) => data);
}

function updateProject(projectId, project) {
  let url = `http://localhost:5000/projects/${projectId}`;
  return fetch(url, {
    mode: "cors",
    method: "put",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(project),
  })
    .then((response) => response.json())
    .then((data) => data);
}

function getEntities(projectId) {
  let url = `http://localhost:5000/entities/${projectId}`;
  return fetch(url, {
    mode: "cors",
    method: "get",
    headers: {
      "Content-Type": "application/json",
    },
  })
    .then((response) => response.json())
    .then((data) => data);
}

function getEntity(projectId, entityName) {
  let url = encodeURI(
    `http://localhost:5000/entities/${projectId}/${entityName}`
  );
  return fetch(url, {
    mode: "cors",
    method: "get",
    headers: {
      "Content-Type": "application/json",
    },
  })
    .then((response) => response.json())
    .then((data) => data);
}

function updateEntity(projectId, entityName, entity) {
  let url = encodeURI(
    `http://localhost:5000/entities/${projectId}/${entityName}`
  );
  return fetch(url, {
    mode: "cors",
    method: "patch",
    headers: {
      "Content-Type": "application/json",
    },
    body: "{entity:'test'}", //JSON.stringify(entity),
  })
    .then((response) => response.json())
    .then((data) => data);
}

export {
  getProjects,
  getProject,
  updateProject,
  getEntities,
  getEntity,
  updateEntity,
};
