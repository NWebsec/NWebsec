// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

namespace NWebsec.Core.HttpHeaders.Configuration
{
    /// <summary>
    ///     Defines the properties required for CSP sandbox directive configuration.
    /// </summary>
    public interface ICspSimpleDirectiveConfiguration
    {
        /// <summary>
        ///     Infrastructure. Not intended to be used by your code directly. An attempt to hide this from Intellisense has been
        ///     made.
        /// </summary>
        bool Enabled { get; set; }
    }
}