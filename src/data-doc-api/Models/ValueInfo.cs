using System;

namespace data_doc_api.Models
{
    /// <summary>
    /// A value within a value group.
    /// </summary>
    public class ValueInfo
    {
        /// <summary>
        /// Unique system id for the value
        /// </summary>
        public int? ValueId { get; set; }

        /// <summary>
        /// Unique system id for the value group
        /// </summary>
        public int ValueGroupId { get; set; }

        /// <summary>
        /// The value
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// The description
        /// </summary>
        public string Desc { get; set; }

    }
}
