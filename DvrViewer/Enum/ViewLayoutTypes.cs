using System.ComponentModel;

namespace DvrViewer.Enum
{
    /// <summary>
    /// Represents the possible output area views
    /// </summary>
    public enum ViewLayoutTypes
    {
        /// <summary>
        /// The view in the output area should be a single window
        /// </summary>
        [Description("1 x 1")]
        View1By1 = 1,

        /// <summary>
        /// The view in the output area should be set to 1 by 2 windows
        /// </summary>
        [Description("1 x 2")]
        View1By2 = 2,

        /// <summary>
        /// The view in the output area should be set to 2 by 1 windows
        /// </summary>
        [Description("2 x 1")]
        View2By1 = 3,

        /// <summary>
        /// The view in the output area should be set to 2 by 2 windows
        /// </summary>
        [Description("2 x 2")]
        View2By2 = 4,

        /// <summary>
        /// The view in the output area should be set to 3 by 3 windows
        /// </summary>
        [Description("3 x 3")]
        View3By3 = 5
    }
}
