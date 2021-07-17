using System;
using System.Collections.Generic;

namespace data_doc_api.Models
{
    /// <summary>
    /// Represents a backup object. Contains all information for a project.
    /// </summary>
    public class BackupInfo
    {
        /// <summary>
        /// The project details
        /// </summary>
        public ProjectInfo Project { get; set; }

        /// <summary>
        /// The entities for the project
        /// </summary>
        public IEnumerable<EntityDetailsInfo> Entities { get; set; }

        /// <summary>
        /// The attributes for the project
        /// </summary>
        public IEnumerable<AttributeDetailsInfo> Attributes { get; set; }

        /// <summary>
        /// The attribute hierarchies for the project
        /// </summary>
        public IEnumerable<AttributeHierarchyInfo> Hierarchies { get; set; }

        /// <summary>
        /// The entity dependencies for the project
        /// </summary>
        public IEnumerable<EntityDependencyInfo> Dependencies { get; set; }

        /// <summary>
        /// The relationships for the project
        /// </summary>
        public IEnumerable<RelationshipInfo> Relationships { get; set; }

        /// <summary>
        /// The calculations defined for the project
        /// </summary>
        public IEnumerable<CalculationInfo> Calculations { get; set; }

        /// <summary>
        /// The value groups for the project
        /// </summary>
        public IEnumerable<ValueGroupInfo> ValueGroups { get; set; }

        /// <summary>
        /// The values for the value groups in the project
        /// </summary>
        public IEnumerable<ValueInfo> Values { get; set; }
    }
}