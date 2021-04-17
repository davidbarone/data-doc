using System;

namespace data_doc_api.Models
{
    /// <summary>
    /// Represents an entity object. Typically a table, or view
    /// </summary>
    public class EntityDependencyInfo
    {
        /// <summary>
        /// The project id
        /// </summary>
        public int ProjectId { get; set; }

        /// <summary>
        /// The parent entity name
        /// </summary>
        public string ParentEntityName { get; set; }

        /// <summary>
        /// The child entity name
        /// </summary>
        public string ChildEntityName { get; set; }

    }
}