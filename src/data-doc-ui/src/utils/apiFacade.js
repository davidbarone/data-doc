import { toast } from "../components/myToast";

function handleErrors(response, successMessage) {
  if (!response.ok) {
    throw Error(response.statusText);
  }
  toastSuccess(successMessage);
  return response;
}

function toastSuccess(message) {
  toast.show(message, {
    timeout: 3000,
    position: "bottom-right",
    variant: "success",
  });
}

function toastFailure(message) {
  toast.show(message, {
    timeout: 3000,
    position: "bottom-right",
    variant: "danger",
  });
}

function createProject(project) {
  let url = `http://localhost:5000/projects`;
  return fetch(url, {
    mode: "cors",
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(project),
  })
    .then((response) => handleErrors(response, "Project created successfully"))
    .then((response) => response.json())
    .then((data) => {
      return data;
    })
    .catch((response) => toastFailure(response));
}

function deleteProject(projectId) {
  let url = `http://localhost:5000/projects/${projectId}`;
  return fetch(url, {
    mode: "cors",
    method: "DELETE",
    headers: {
      "Content-Type": "application/json",
    },
  })
    .then((response) => handleErrors(response, "Project deleted successfully"))
    .catch((response) => toastFailure(response));
}

function getProjects() {
  let url = `http://localhost:5000/projects`;
  return fetch(url, {
    mode: "cors",
    headers: {
      "Content-Type": "application/json",
    },
  })
    .then((response) =>
      handleErrors(response, "Projects retrieved successfully")
    )
    .then((response) => response.json())
    .then((data) => {
      return data;
    })
    .catch((error) => toastFailure(error));
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
    .then((response) => handleErrors(response, "Project updated successfully"))
    .catch((response) => toastFailure(response));
}

function scanProject(projectId) {
  let url = encodeURI(`http://localhost:5000/projects/scan/${projectId}`);
  return fetch(url, {
    mode: "cors",
    method: "PUT",
    headers: {
      "Content-Type": "application/json",
    },
  }).then(() => {
    toastSuccess("Entities scanned successfully");
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
    .then((response) => handleErrors(response, "Entity updated successfully"))
    .then((response) => {
      if (!response.ok) {
        throw response.statusText;
      }
      response.json();
    })
    .then((data) => data)
    .catch((error) => {
      toastFailure(error);
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
    .then((response) =>
      handleErrors(response, "Attribute configuration set successfully")
    )
    .then((response) => response.json())
    .then((data) => data)
    .catch((error) => {
      toastFailure(error);
    });
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
    .then((response) =>
      handleErrors(response, "Attribute configuration unset successfully")
    )
    .then((response) => response.json())
    .then((data) => data)
    .catch((error) => {
      toastFailure(error);
    });
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
    .then((response) =>
      handleErrors(response, "Attribute primary key set successfully")
    )
    .then((response) => response.json())
    .then((data) => data)
    .catch((error) => {
      toastFailure(error);
    });
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
    .then((response) =>
      handleErrors(response, "Attribute primary key unset successfully")
    )
    .then((response) => response.json())
    .then((data) => data)
    .catch((error) => toastFailure(error));
}

function setAttributeDescConfig(
  projectId,
  entityName,
  attributeName,
  scope,
  attributeDesc,
  attributeComment,
  valueGroupId
) {
  let url = encodeURI(
    `http://localhost:5000/attributes/Desc/${projectId}/${entityName}/${attributeName}/${scope}`
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
    .then((response) =>
      handleErrors(response, "Attribute description set successfully")
    )
    .then((response) => response.json())
    .then((data) => data)
    .catch((error) => toastFailure(error));
}

function unsetAttributeDescConfig(projectId, entityName, attributeName, scope) {
  let url = encodeURI(
    `http://localhost:5000/attributes/Desc/${projectId}/${entityName}/${attributeName}/${scope}`
  );
  return fetch(url, {
    mode: "cors",
    method: "DELETE",
    headers: {
      "Content-Type": "application/json",
    },
  })
    .then((response) =>
      handleErrors(response, "Attribute description unset successfully")
    )
    .then((response) => response.json())
    .then((data) => data)
    .catch((error) => toastFailure(error));
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
  let url = encodeURI(`http://localhost:5000/relationships/`);
  return fetch(url, {
    mode: "cors",
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(relationship),
  })
    .then((response) =>
      handleErrors(response, "Relationship created successfully")
    )
    .then((response) => response.json())
    .then((data) => data)
    .catch((error) => toastFailure(error));
}

function deleteRelationship(relationshipId) {
  let url = encodeURI(`http://localhost:5000/relationships/${relationshipId}`);
  return fetch(url, {
    mode: "cors",
    method: "DELETE",
    headers: {
      "Content-Type": "application/json",
    },
  })
    .then((response) =>
      handleErrors(response, "Relationship deleted successfully")
    )
    .catch((error) => toastFailure(error));
}

function scanRelationships(projectId) {
  let url = encodeURI(`http://localhost:5000/relationships/scan/${projectId}`);
  return fetch(url, {
    mode: "cors",
    method: "PUT",
    headers: {
      "Content-Type": "application/json",
    },
  })
    .then((response) =>
      handleErrors(response, "Relationships scanned successfully")
    )
    .catch((error) => toastFailure(error));
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
    .then((response) =>
      handleErrors(response, "Value group created successfully")
    )
    .then((response) => response.json())
    .then((data) => data)
    .catch((error) => toastFailure(error));
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
  })
    .then((response) => handleErrors(response, "Values scanned successfully"))
    .catch((error) => toastFailure(error));
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
  })
    .then((response) => handleErrors(response, "Value created successfully"))
    .catch((error) => toastFailure(error));
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
  })
    .then((response) => handleErrors(response, "Value updated successfully"))
    .catch((error) => toastFailure(error));
}

function deleteValue(valueId) {
  let url = encodeURI(`http://localhost:5000/values/${valueId}`);
  return fetch(url, {
    mode: "cors",
    method: "DELETE",
    headers: {
      "Content-Type": "application/json",
    },
  })
    .then((response) => handleErrors(response, "Value deleted successfully"))
    .catch((error) => toastFailure(error));
}

export {
  // Projects
  getProjects,
  getProject,
  createProject,
  deleteProject,
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
