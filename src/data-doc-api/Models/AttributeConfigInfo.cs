using System;

namespace data_doc_api.Models
{
    /// <summary>
    /// Represents the business metadata relating to a database entity.
    /// </summary>
    public class AttributeConfigInfo
    {
        /// <summary>
        /// Unique Id
        /// </summary>
        public int? AttributeConfigId { get; set; }

        /// <summary>
        /// The project id
        /// </summary>
        public int ProjectId { get; set; }

        /// <summary>
        /// The entity name
        /// </summary>
        public string EntityName { get; set; }

        /// <summary>
        /// The attribute name
        /// </summary>
        public string AttributeName { get; set; }

        /// <summary>
        /// The attribute (business) description
        /// </summary>
        public string AttributeDesc { get; set; }

        /// <summary>
        /// The entity active flag.
        /// </summary>
        public bool IsActive { get; set; }
    }
}