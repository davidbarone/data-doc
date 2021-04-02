namespace data_doc_api.Models
{
    /// <summary>
    /// Payload when updating an attribute config resource.
    /// </summary>
    public class AttributeDescConfigPayloadInfo
    {
        public string AttributeDesc { get; set; }
        public string AttributeComment { get; set; }
    }
}