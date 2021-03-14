using System;
using System.Collections.Generic;

namespace data_doc_api.Models
{
    /// <summary>
    /// Represents a configured relationship in the database. The relationship can be manually entered, or be automatically scanned from source database.
    /// </summary>
    public class RelationshipAttributeInfo
    {
        /// <summary>
        /// The unique key
        /// </summary>
        public int RelationshipAttributeId { get; set; }

        /// <summary>
        /// The relationship id
        /// </summary>
        public int RelationshipId { get; set; }

        /// <summary>
        /// The parent attribute name
        /// </summary>
        public string ParentAttributeName { get; set; }

        /// <summary>
        /// The referenced attribute name
        /// </summary>
        public string ReferencedAttributeName { get; set; }
    }
}