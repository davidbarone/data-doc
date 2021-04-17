namespace data_doc_api.Models
{
    /// <summary>
    /// Payload when updating an attribute config resource.
    /// </summary>
    public class AttributeDescConfigPayloadInfo
    {
        /// <summary>
        /// The attribute's business description
        /// </summary>
        public string AttributeDesc { get; set; }
        /// <summary>
        /// Optional business commentary for the attribute
        /// </summary>
        public string AttributeComment { get; set; }
        /// <summary>
        /// Optional value group for the attribute, defining the permitted values the attribute can take
        /// </summary>
        public int? ValueGroupId { get; set; }
    }
}