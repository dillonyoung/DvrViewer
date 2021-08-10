using System;

namespace DvrViewer.Configuration
{
    /// <summary>
    /// Represents the file format for a configuration file
    /// </summary>
    public class FileFormat
    {
        /// <summary>
        /// The ID of the file
        /// </summary>
        public string Id { get; set; } = "DVRVIEW";

        /// <summary>
        /// The date/time the configuration was first saved
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// The date/time the configuration was last saved
        /// </summary>
        public DateTime LastModified { get; set; }

        /// <summary>
        /// The number of time the configuration has been saved
        /// </summary>
        public int SaveCount { get; set; }

        /// <summary>
        /// The configuration which has been saved
        /// </summary>
        public object Configuration { get; set; }
    }
}
