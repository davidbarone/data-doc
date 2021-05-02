using System;

namespace data_doc_api.Models
{
    /// <summary>
    /// Represents an entity object. Typically a table, or view
    /// </summary>
    public class AttributeHierarchyInfo
    {
        /// <summary>
        /// The project id
        /// </summary>
        public int ProjectId { get; set; }

        /// <summary>
        /// The entity name
        /// </summary>
        public string EntityName { get; set; }

        /// <summary>
        /// The name of the attribute - the attribute on the many side
        /// </summary>
        public string ParentAttributeName { get; set; }

        /// <summary>
        /// The name of the child attribute - the attribute on the one side
        /// </summary>
        public string ChildAttributeName { get; set; }

        /// <summary>
        /// Set to true if the relationship is at the root level (related to the primary key attribute(s))
        /// </summary>
        public bool IsRoot { get; set; }

        /// <summary>
        /// Set to true if the hierarchy relationship is 1:1
        /// </summary>
        public bool IsOneToOneRelationship { get; set; }
    }
}