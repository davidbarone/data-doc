using System;

namespace data_doc_api.Models
{
    /// <summary>
    /// Represents a physical foreign key constraing in the database
    /// </summary>
    public class RelationshipScanInfo
    {
        /// <summary>
        /// The project id
        /// </summary>
        public int ProjectId { get; set; }

        /// <summary>
        /// The relationship / FK name
        /// </summary>
        public string RelationshipName { get; set; }

        /// <summary>
        /// The parent entity name
        /// </summary>
        public string ParentEntityName { get; set; }

        /// <summary>
        /// The parent attribute name
        /// </summary>
        public string ParentAttributeName { get; set; }

        /// <summary>
        /// The referenced entity name
        /// </summary>
        public string ReferencedEntityName { get; set; }

        /// <summary>
        /// The referenced attribute name
        /// </summary>
        public string ReferencedAttributeName { get; set; }
    }
}