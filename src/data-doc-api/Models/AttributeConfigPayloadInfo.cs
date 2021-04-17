namespace data_doc_api.Models
{
    /// <summary>
    /// Payload when updating an attribute config resource.
    /// </summary>
    public class AttributeConfigPayloadInfo
    {
        /// <summary>
        /// Set to true if the attribute is visible in the documentation
        /// </summary>
        public bool IsActive { get; set; }
    }
}