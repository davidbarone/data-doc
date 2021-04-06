import { toast } from "../components/myToast";

function getProjects() {
  let url = `http://localhost:5000/projects`;
  return fetch(url, {
    mode: "cors",
    headers: {
      "Content-Type": "application/json",
    },
  })
    .then((response) => response.json())
    .then((data) => {
      return data;
    });
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
  }).then(() => {
    toast.show("Project updated successfully.", {
      timeout: 3000,
      position: "bottom-right",
      variant: "success",
    });
  });
}

function scanProject(projectId) {
  let url = encodeURI(`http://localhost:5000/projects/scan/${projectId}`);
  return fetch(url, {
    mode: "cors",
    method: "PUT",
    headers: {
      "Content-Type": "application/json",
    },
  });
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
    .then((response) => {
      if (!response.ok) {
        throw response.statusText;
      }
      response.json();
    })
    .then((data) => data)
    .then(() => {
      toast.show("Entity updated successfully.", {
        timeout: 3000,
        position: "bottom-right",
        variant: "success",
      });
    })
    .catch((error) => {
      toast.show(error, {
        timeout: 3000,
        position: "bottom-right",
        variant: "danger",
      });
    });
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

function setAttributeDescConfig(
  projectId,
  entityName,
  attributeName,
  attributeDesc,
  attributeComment
) {
  let url = encodeURI(
    `http://localhost:5000/attributes/Desc/${projectId}/${entityName}/${attributeName}`
  );
  return fetch(url, {
    mode: "cors",
    method: "PATCH",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify({
      attributeDesc,
      attributeComment,
    }),
  })
    .then((response) => response.json())
    .then((data) => data);
}

function unsetAttributeDescConfig(projectId, entityName, attributeName) {
  let url = encodeURI(
    `http://localhost:5000/attributes/Desc/${projectId}/${entityName}/${attributeName}`
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

function getRelationships(projectId) {
  let url = encodeURI(`http://localhost:5000/relationships/${projectId}`);
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

export {
  // Projects
  getProjects,
  getProject,
  updateProject,
  getDownloadUrl,
  scanProject,
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
  setAttributeDescConfig,
  unsetAttributeDescConfig,
  // Relationships
  getRelationships,
};
