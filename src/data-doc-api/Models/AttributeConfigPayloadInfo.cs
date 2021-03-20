namespace data_doc_api.Models
{
    /// <summary>
    /// Payload when updating an attribute config resource.
    /// </summary>
    public class AttributeConfigPayloadInfo
    {
        public string AttributeDesc { get; set; }
        public string AttributeComment { get; set; }
        public bool IsActive { get; set; }
    }
}