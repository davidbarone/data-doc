using System;

namespace data_doc_api.Models
{
    /// <summary>
    /// Project model
    /// </summary>
    public class ProjectInfo
    {
        /// <summary>
        /// Unique system id for the project
        /// </summary>
        public int ProjectId { get; set; }

        /// <summary>
        /// Project name. Must be unique
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// Project description
        /// </summary>
        public string ProjectDesc { get; set; }

        /// <summary>
        /// Project comments
        /// </summary>
        public string ProjectComment { get; set; }

        /// <summary>
        /// Connection string to the database
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Version number relating to last scan
        /// </summary>
        public int ScanVersion { get; set; }

        /// <summary>
        /// DateTime of last scan
        /// </summary>
        public DateTime ScanUpdatedDt { get; set; }

        /// <summary>
        /// Version number relating to configuration of metadata
        /// </summary>
        public int ConfigVersion { get; set; }

        /// <summary>
        /// DateTime of last configuration change
        /// </summary>
        public DateTime ConfigUpdatedDt { get; set; }

        /// <summary>
        /// Set to true if the project is currently active
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Returns the (single/highest) version for the project
        /// </summary>
        public int Version
        {
            get
            {
                return ScanVersion > ConfigVersion ? ScanVersion : ConfigVersion;
            }
        }

        /// <summary>
        /// Returns the last updated date for the project
        /// </summary>
        public DateTime UpdatedDt
        {
            get
            {
                return ScanUpdatedDt > ConfigUpdatedDt ? ScanUpdatedDt : ConfigUpdatedDt;
            }
        }

    }
}
