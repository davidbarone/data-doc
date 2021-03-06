using System;

namespace data_doc_api.Models
{
    /// <summary>
    /// Represents an entity object. Typically a table, or view.
    /// </summary>
    public class EntityInfo
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
        /// The type of entity (table, view etc)
        /// </summary>
        public string EntityType { get; set; }

        /// <summary>
        /// The number of rows in the entity when last scanned
        /// </summary>
        public int RowCount { get; set; }

        /// <summary>
        /// The creation date of the entity
        /// </summary>
        public DateTime CreatedDt { get; set; }

        /// <summary>
        /// The date that the entity was last modified via DDL
        /// </summary>
        public DateTime ModifiedDt { get; set; }

        /// <summary>
        /// The date that the entity was last modified via DML
        /// </summary>
        public DateTime UpdatedDt { get; set; }

        /// <summary>
        /// The definition code of the entity (if applicable)
        /// </summary>
        public string Definition { get; set; }
    }
}