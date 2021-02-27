using System;

namespace DataDoc.Models
{
    /// <summary>
    /// Represents an entity object. Typically a table, or view.
    /// </summary>
    public class AttributeInfo
    {
        public string ProjectName { get; set; }
        public string EntityName { get; set; }
        public string AttributeName { get; set; }
        public int Order { get; set; }
        public Boolean IsPrimaryKey { get; set; }
        public string DataType { get; set; }
        public int DataLength { get; set; }
        public int Precision { get; set; }
        public int Scale { get; set; }
    }
}