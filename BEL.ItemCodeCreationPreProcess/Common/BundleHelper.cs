namespace BEL.ItemCodeCreationPreProcess.Common
{
    using System;
    using System.Configuration;

    /// <summary>
    /// Bundle Helper
    /// </summary>
    public static class BundleHelper
    {
        /// <summary>
        /// Gets the style version.
        /// </summary>
        /// <value>
        /// The style version.
        /// </value>
        public static string StyleVersion
        {
            get
            {
                return "<link href=\"{0}?v=" + ConfigurationManager.AppSettings["version"] + "_" + DateTime.Now.Millisecond + "\" rel=\"stylesheet\"/>";
            }
        }

        /// <summary>
        /// Gets the script version.
        /// </summary>
        /// <value>
        /// The script version.
        /// </value>
        public static string ScriptVersion
        {
            get
            {
                return "<script src=\"{0}?v=" + ConfigurationManager.AppSettings["version"] + "_" + DateTime.Now.Millisecond + "\"></script>";
            }
        }
    }
}