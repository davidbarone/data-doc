namespace data_doc_api.Models
{
    /// <summary>
    /// Optional configuration to override the default primary key status of an attriute
    /// </summary>
    public class AttributePrimaryKeyConfigPayloadInfo
    {
        /// <summary>
        /// Set to true if the attribute is part of a primary key
        /// </summary>
        public bool IsPrimaryKey { get; set; }
    }
}