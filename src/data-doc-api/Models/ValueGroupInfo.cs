using System;

namespace data_doc_api.Models
{
    /// <summary>
    /// A Value Group
    /// </summary>
    public class ValueGroupInfo
    {
        /// <summary>
        /// Unique system id for the value group
        /// </summary>
        public int? ValueGroupId { get; set; }

        /// <summary>
        /// The project id
        /// </summary>
        public int ProjectId { get; set; }

        /// <summary>
        /// Value group name
        /// </summary>
        public string ValueGroupName { get; set; }

    }
}
