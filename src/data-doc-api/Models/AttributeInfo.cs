using System;

namespace data_doc_api.Models
{
    /// <summary>
    /// Represents an entity object. Typically a table, or view.
    /// </summary>
    public class AttributeInfo
    {
        /// <summary>
        /// The project that the attribute belongs to
        /// </summary>
        public int ProjectId { get; set; }
        /// <summary>
        /// The entity that the attribute belongs to
        /// </summary>
        public string EntityName { get; set; }
        /// <summary>
        /// The name of the attribute
        /// </summary>
        public string AttributeName { get; set; }
        /// <summary>
        /// The internal order of the attribute within the entity
        /// </summary>
        public int Order { get; set; }
        /// <summary>
        /// Set to true if the attribute is part of a primary key
        /// </summary>
        public Boolean IsPrimaryKey { get; set; }
        /// <summary>
        /// The data type of the attribute
        /// </summary>
        public string DataType { get; set; }
        /// <summary>
        /// The data length of the attribute if a character data type
        /// </summary>
        public int DataLength { get; set; }
        /// <summary>
        /// The precision of the data type if a numeric type
        /// </summary>
        public int Precision { get; set; }
        /// <summary>
        /// The scale of the data type if a numeric type
        /// </summary>
        public int Scale { get; set; }
        /// <summary>
        /// Set to true if the attribute allows null values
        /// </summary>
        public bool IsNullable { get; set; }
    }
}