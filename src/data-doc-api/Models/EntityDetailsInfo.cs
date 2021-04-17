using System;

namespace data_doc_api.Models
{
    /// <summary>
    /// Represents the full entity information including both scanned (automated) and configured values
    /// </summary>
    public class EntityDetailsInfo : EntityInfo
    {
        /// <summary>
        /// Unique Id for the entity configuration
        /// </summary>
        public int? EntityConfigId { get; set; }

        /// <summary>
        /// The alias / business name for the entity
        /// </summary>
        public string EntityAlias { get; set; }

        /// <summary>
        /// The entity (business) description
        /// </summary>
        public string EntityDesc { get; set; }

        /// <summary>
        /// Additional entity comments
        /// </summary>
        public string EntityComment { get; set; }

        /// <summary>
        /// The entity active flag
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// If set to true, then the documenter will display the contents of the entiy
        /// </summary>
        public bool ShowData { get; set; }

        /// <summary>
        /// If set to true, then the documenter will display the definition of the entity
        /// </summary>
        public bool ShowDefinition { get; set; }
    }
}