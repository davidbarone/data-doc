using System;
using System.Collections.Generic;

namespace data_doc_api.Models
{
    /// <summary>
    /// Represents an entity object. Typically a table, or view.
    /// </summary>
    public class AttributeInfo
    {
        public int ProjectId { get; set; }
        public string EntityName { get; set; }
        public string AttributeName { get; set; }
        public int Order { get; set; }
        public Boolean IsPrimaryKey { get; set; }
        public string DataType { get; set; }
        public int DataLength { get; set; }
        public int Precision { get; set; }
        public int Scale { get; set; }
        public bool IsNullable { get; set; }

        public string DataTypeDesc
        {
            get
            {
                List<string> charTypes = new List<string>() {
                    "char", "varchar", "nchar", "nvarchar", "varbinary", "binary"
                };
                List<string> decimalTypes = new List<string>() {
                    "decimal", "numeric"
                };
                var type = this.DataType.ToLower();
                if (charTypes.Contains(type))
                {
                    return $"{this.DataType}({this.DataLength})";
                }
                else if (decimalTypes.Contains(type))
                {
                    return $"{this.DataType}({this.Precision}, {this.Scale})";
                }
                else
                {
                    return $"{this.DataType}";
                }
            }
        }

    }
}