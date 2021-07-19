using System;

namespace data_doc_api.Models
{
    /// <summary>
    /// Represents a calculation object. Used to document calculation operations on an entity (for example a custom aggregataion, filter or measure/metric)
    /// </summary>
    public class CalculationInfo
    {
        /// <summary>
        /// Unique calculation id
        /// </summary>
        public int? CalculationId { get; set; }
        /// <summary>
        /// The project that the calculation belongs to
        /// </summary>
        public int ProjectId { get; set; }
        /// <summary>
        /// The entity that the calculation belongs to
        /// </summary>
        public string EntityName { get; set; }
        /// <summary>
        /// The name of the calculation
        /// </summary>
        public string CalculationName { get; set; }
        /// <summary>
        /// The calculation description
        /// </summary>
        public string CalculationDesc { get; set; }
        /// <summary>
        /// The calculation comment
        /// </summary>
        public string CalculationComment { get; set; }
        /// <summary>
        /// The expression or formula to describe the calculation. Can be textual, pseudocode, or code from a BI tool.
        /// </summary>
        public string Formula { get; set; }
    }
}