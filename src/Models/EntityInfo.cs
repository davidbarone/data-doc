using System;

namespace DataDoc.Models
{
    /// <summary>
    /// Represents an entity object. Typically a table, or view.
    /// </summary>
    public class EntityInfo
    {
        public string ProjectName { get; set; }
        public string EntityName { get; set; }
        public string EntityType { get; set; }
        public int RowCount { get; set; }
        public DateTime CreatedDt { get; set; }
        public DateTime ModifiedDt { get; set; }
        public DateTime UpdatedDt { get; set; }
        public string Definition { get; set; }
    }
}