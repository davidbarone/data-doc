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
  attributeComment,
  valueGroupId
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
      valueGroupId,
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

function getRelationship(relationshipId) {
  let url = encodeURI(
    `http://localhost:5000/relationships/single/${relationshipId}`
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

function updateRelationship(relationshipId, relationship) {
  let url = encodeURI(`http://localhost:5000/relationships/${relationshipId}`);
  return fetch(url, {
    mode: "cors",
    method: "PUT",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(relationship),
  })
    .then((response) => response.json())
    .then((data) => data);
}

function createRelationship(relationship) {
  alert("create rel");
  let url = encodeURI(`http://localhost:5000/relationships/`);
  return fetch(url, {
    mode: "cors",
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(relationship),
  })
    .then((response) => response.json())
    .then((data) => data);
}

function deleteRelationship(relationshipId) {
  let url = encodeURI(`http://localhost:5000/relationships/${relationshipId}`);
  return fetch(url, {
    mode: "cors",
    method: "DELETE",
    headers: {
      "Content-Type": "application/json",
    },
  }).then(() => {});
}

function scanRelationships(projectId) {
  let url = encodeURI(`http://localhost:5000/relationships/scan/${projectId}`);
  return fetch(url, {
    mode: "cors",
    method: "PUT",
    headers: {
      "Content-Type": "application/json",
    },
  }).then(() => {});
}

function getValueGroups(projectId) {
  let url = encodeURI(`http://localhost:5000/valueGroups/${projectId}`);
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

function createValueGroup(valueGroup) {
  let url = encodeURI(`http://localhost:5000/valueGroups/`);
  return fetch(url, {
    mode: "cors",
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(valueGroup),
  })
    .then((response) => response.json())
    .then((data) => data);
}

function scanValues(valueGroupId, attribute) {
  let url = encodeURI(
    `http://localhost:5000/values/${valueGroupId}/${attribute.projectId}/${attribute.entityName}/${attribute.attributeName}`
  );
  return fetch(url, {
    mode: "cors",
    method: "PUT",
    headers: {
      "Content-Type": "application/json",
    },
  });
}

function getValues(valueGroupId) {
  let url = encodeURI(`http://localhost:5000/values/${valueGroupId}`);
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

function createValue(value) {
  let url = encodeURI(`http://localhost:5000/values/`);
  return fetch(url, {
    mode: "cors",
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(value),
  });
}

function updateValue(valueId, value) {
  let url = encodeURI(`http://localhost:5000/values/${valueId}`);
  return fetch(url, {
    mode: "cors",
    method: "PUT",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(value),
  });
}

function deleteValue(valueId) {
  let url = encodeURI(`http://localhost:5000/values/${valueId}`);
  return fetch(url, {
    mode: "cors",
    method: "DELETE",
    headers: {
      "Content-Type": "application/json",
    },
  });
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
  getRelationship,
  createRelationship,
  updateRelationship,
  deleteRelationship,
  scanRelationships,
  // Value Groups
  getValueGroups,
  createValueGroup,
  // Values
  scanValues,
  getValues,
  createValue,
  updateValue,
  deleteValue,
};
