using System;

namespace data_doc_api.Models
{
    /// <summary>
    /// Represents the business metadata relating to a database entity.
    /// </summary>
    public class AttributeConfigScopedInfo : AttributeConfigInfo
    {
        /// <summary>
        /// The scoping level of the configuration.
        /// </summary>
        public string Scope { get; set; }
    }
}