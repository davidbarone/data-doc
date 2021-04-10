using System;
using System.Collections.Generic;

namespace data_doc_api.Models
{
    /// <summary>
    /// Represents the full attribute information including both scanned (automated) and configured values.
    /// </summary>
    public class AttributeDetailsInfo : AttributeInfo
    {
        /// <summary>
        /// Unique Id of attribute configuration
        /// </summary>
        public int? AttributeConfigId { get; set; }

        /// <summary>
        /// The entity active flag
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Unique Id of attribute description configuration
        /// </summary>
        public int? AttributeDescConfigId { get; set; }

        /// <summary>
        /// The attribute (business) description
        /// </summary>
        public string AttributeDesc { get; set; }

        /// <summary>
        /// Optional attribute comment
        /// </summary>
        public string AttributeComment { get; set; }

        /// <summary>
        /// Optional value group id
        /// </summary>
        public int? ValueGroupId { get; set; }

        /// <summary>
        /// The scoping level of the configuration
        /// </summary>
        public string DescScope { get; set; }

        /// <summary>
        /// Unique Id of attribute primary key configuration
        /// </summary>
        public int? AttributePrimaryKeyConfigId { get; set; }

        /// <summary>
        /// Data type description
        /// </summary>
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