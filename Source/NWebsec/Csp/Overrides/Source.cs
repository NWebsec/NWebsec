// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

namespace NWebsec.Csp.Overrides
{
    public enum Source
    {
        /// <summary>
        /// The source should inherit previous settings.
        /// </summary>
        Inherit,
        /// <summary>
        /// The source should be enabled, overriding previous settings.
        /// </summary>
        Enable,
        /// <summary>
        /// The source should be disabled, clearing previous settings.
        /// </summary>
        Disable
    }
}