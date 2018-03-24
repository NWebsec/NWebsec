// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using NWebsec.Core.Common.Fluent;

namespace NWebsec.Core.Common.Middleware.Options
{
    /// <summary>
    /// Fluent interface to configure options for X-Xss-Protection.
    /// </summary>
    public interface IFluentXXssProtectionOptions : IFluentInterface
    {
        /// <summary>
        /// Configures the header to explicitly disable protection.
        /// </summary>
        void Disabled();

        /// <summary>
        /// Configures the header to explicitly enable protection.
        /// </summary>
        void Enabled();

        /// <summary>
        /// Configures the header to explicitly enable protection with block mode.
        /// </summary>
        void EnabledWithBlockMode();
    }
}