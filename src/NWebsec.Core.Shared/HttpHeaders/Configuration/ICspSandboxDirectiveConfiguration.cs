// Copyright (c) Andr� N. Klingsheim. See License.txt in the project root for license information.

namespace NWebsec.Core.Common.HttpHeaders.Configuration
{
    /// <summary>
    ///     Defines the properties required for CSP sandbox directive configuration.
    /// </summary>
    public interface ICspSandboxDirectiveConfiguration
    {
        /// <summary>
        ///     Infrastructure. Not intended to be used by your code directly. An attempt to hide this from Intellisense has been
        ///     made.
        /// </summary>
        bool Enabled { get; set; }
        /// <summary>
        ///     Infrastructure. Not intended to be used by your code directly. An attempt to hide this from Intellisense has been
        ///     made.
        /// </summary>
        bool AllowDownloads { get; set; }
        /// <summary>
        ///     Infrastructure. Not intended to be used by your code directly. An attempt to hide this from Intellisense has been
        ///     made.
        /// </summary>
        bool AllowForms { get; set; }
        /// <summary>
        ///     Infrastructure. Not intended to be used by your code directly. An attempt to hide this from Intellisense has been
        ///     made.
        /// </summary>
        bool AllowModals { get; set; }
        /// <summary>
        ///     Infrastructure. Not intended to be used by your code directly. An attempt to hide this from Intellisense has been
        ///     made.
        /// </summary>
        bool AllowOrientationLock { get; set; }
        /// <summary>
        ///     Infrastructure. Not intended to be used by your code directly. An attempt to hide this from Intellisense has been
        ///     made.
        /// </summary>
        bool AllowPointerLock { get; set; }
        /// <summary>
        ///     Infrastructure. Not intended to be used by your code directly. An attempt to hide this from Intellisense has been
        ///     made.
        /// </summary>
        bool AllowPopups { get; set; }
        /// <summary>
        ///     Infrastructure. Not intended to be used by your code directly. An attempt to hide this from Intellisense has been
        ///     made.
        /// </summary>
        bool AllowPopupsToEscapeSandbox { get; set; }
        /// <summary>
        ///     Infrastructure. Not intended to be used by your code directly. An attempt to hide this from Intellisense has been
        ///     made.
        /// </summary>
        bool AllowPresentation { get; set; }
        /// <summary>
        ///     Infrastructure. Not intended to be used by your code directly. An attempt to hide this from Intellisense has been
        ///     made.
        /// </summary>
        bool AllowSameOrigin { get; set; }
        /// <summary>
        ///     Infrastructure. Not intended to be used by your code directly. An attempt to hide this from Intellisense has been
        ///     made.
        /// </summary>
        bool AllowScripts { get; set; }
        /// <summary>
        ///     Infrastructure. Not intended to be used by your code directly. An attempt to hide this from Intellisense has been
        ///     made.
        /// </summary>
        bool AllowTopNavigation { get; set; }
    }
}