using System;

namespace data_doc_api.Models
{
    /// <summary>
    /// Represents the full attribute information including both scanned (automated) and configured values.
    /// </summary>
    public class AttributeDetailsInfo : AttributeInfo
    {
        /// <summary>
        /// Unique Id
        /// </summary>
        public int? AttributeConfigId { get; set; }

        /// <summary>
        /// The attribute (business) description
        /// </summary>
        public string AttributeDesc { get; set; }

        /// <summary>
        /// Optional attribute comment
        /// </summary>
        public string AttributeComment { get; set; }

        /// <summary>
        /// The entity active flag.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// The scoping level of the configuration.
        /// </summary>
        public string Scope { get; set; }
    }
}