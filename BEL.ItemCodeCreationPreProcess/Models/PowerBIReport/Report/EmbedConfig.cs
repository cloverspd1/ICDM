namespace BEL.ItemCodeCreationPreProcess.Models.PowerBIReport.Report
{
    using Microsoft.PowerBI.Api.V2.Models;
    using System;

    /// <summary>
    /// Embed Config
    /// </summary>
    public class EmbedConfig
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the embed URL.
        /// </summary>
        /// <value>
        /// The embed URL.
        /// </value>
        public string EmbedUrl { get; set; }

        /// <summary>
        /// Gets or sets the embed token.
        /// </summary>
        /// <value>
        /// The embed token.
        /// </value>
        public EmbedToken EmbedToken { get; set; }

        /// <summary>
        /// Gets the minutes to expiration.
        /// </summary>
        /// <value>
        /// The minutes to expiration.
        /// </value>
        public int MinutesToExpiration
        {
            get
            {
                var minutesToExpiration = EmbedToken.Expiration.Value - DateTime.UtcNow;
                return minutesToExpiration.Minutes;
            }
        }

        /// <summary>
        /// Gets or sets the is effective identity roles required.
        /// </summary>
        /// <value>
        /// The is effective identity roles required.
        /// </value>
        public bool? IsEffectiveIdentityRolesRequired { get; set; }

        /// <summary>
        /// Gets or sets the is effective identity required.
        /// </summary>
        /// <value>
        /// The is effective identity required.
        /// </value>
        public bool? IsEffectiveIdentityRequired { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [enable RLS].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [enable RLS]; otherwise, <c>false</c>.
        /// </value>
        public bool EnableRLS { get; set; }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        /// <value>
        /// The username.
        /// </value>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the roles.
        /// </summary>
        /// <value>
        /// The roles.
        /// </value>
        public string Roles { get; set; }

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        /// <value>
        /// The error message.
        /// </value>
        public string ErrorMessage { get; set; }
    }
}