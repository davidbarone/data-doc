namespace data_doc_api.Models
{
    /// <summary>
    /// Payload when updating an entity config resource
    /// </summary>
    public class EntityConfigPayloadInfo
    {
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
        /// If set to true, then the documenter will display the contents of the entity
        /// </summary>
        public bool ShowData { get; set; }

        /// <summary>
        /// If set to true, then the documenter will display the definition of the entity
        /// </summary>
        public bool ShowDefinition { get; set; }
    }
}