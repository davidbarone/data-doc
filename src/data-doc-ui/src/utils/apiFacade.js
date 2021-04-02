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
    method: "PUT",
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
    method: "GET",
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
    method: "GET",
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
    method: "PATCH",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(entity),
  })
    .then((response) => response.json())
    .then((data) => data);
}

function getAttributes(projectId, entityName) {
  let url = encodeURI(
    `http://localhost:5000/attributes/${projectId}/${entityName}`
  );
  return fetch(url, {
    mode: "cors",
    method: "GET",
    headers: {
      "Content-Type": "application/json",
    },
  })
    .then((response) => response.json())
    .then((data) => data);
}

function getAttribute(projectId, entityName, attributeName) {
  let url = encodeURI(
    `http://localhost:5000/attributes/${projectId}/${entityName}/${attributeName}`
  );
  return fetch(url, {
    mode: "cors",
    method: "GET",
    headers: {
      "Content-Type": "application/json",
    },
  })
    .then((response) => response.json())
    .then((data) => data);
}

function setAttributeConfig(projectId, entityName, attributeName, isActive) {
  let url = encodeURI(
    `http://localhost:5000/attributes/config/${projectId}/${entityName}/${attributeName}`
  );
  return fetch(url, {
    mode: "cors",
    method: "PATCH",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify({
      isActive,
    }),
  })
    .then((response) => response.json())
    .then((data) => data);
}

function unsetAttributeConfig(projectId, entityName, attributeName) {
  let url = encodeURI(
    `http://localhost:5000/attributes/Config/${projectId}/${entityName}/${attributeName}`
  );
  return fetch(url, {
    mode: "cors",
    method: "DELETE",
    headers: {
      "Content-Type": "application/json",
    },
  })
    .then((response) => response.json())
    .then((data) => data);
}

function setAttributePrimaryKeyConfig(
  projectId,
  entityName,
  attributeName,
  isPrimaryKey
) {
  let url = encodeURI(
    `http://localhost:5000/attributes/PrimaryKey/${projectId}/${entityName}/${attributeName}`
  );
  return fetch(url, {
    mode: "cors",
    method: "PATCH",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify({
      isPrimaryKey,
    }),
  })
    .then((response) => response.json())
    .then((data) => data);
}

function unsetAttributePrimaryKeyConfig(projectId, entityName, attributeName) {
  let url = encodeURI(
    `http://localhost:5000/attributes/PrimaryKey/${projectId}/${entityName}/${attributeName}`
  );
  return fetch(url, {
    mode: "cors",
    method: "DELETE",
    headers: {
      "Content-Type": "application/json",
    },
  })
    .then((response) => response.json())
    .then((data) => data);
}

function getDownloadUrl(projectId) {
  return `http://localhost:5000/Projects/document/${projectId}`;
}

export {
  // Projects
  getProjects,
  getProject,
  updateProject,
  getDownloadUrl,
  // Entities
  getEntities,
  getEntity,
  updateEntity,
  // Attributes
  getAttributes,
  getAttribute,
  setAttributeConfig,
  unsetAttributeConfig,
  setAttributePrimaryKeyConfig,
  unsetAttributePrimaryKeyConfig,
};
