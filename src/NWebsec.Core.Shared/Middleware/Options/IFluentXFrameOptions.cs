// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using NWebsec.Core.Common.Fluent;

namespace NWebsec.Core.Common.Middleware.Options
{
    /// <summary>
    /// Fluent interface to configure options for X-Frame-Options.
    /// </summary>
    public interface IFluentXFrameOptions : IFluentInterface
    {
        /// <summary>
        /// Enables the Deny directive.
        /// </summary>
        void Deny();

        /// <summary>
        /// Enables the SameOrigin directive.
        /// </summary>
        void SameOrigin();
    }
}