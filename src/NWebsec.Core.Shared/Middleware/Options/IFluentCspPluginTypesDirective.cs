// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using NWebsec.Core.Common.Fluent;

namespace NWebsec.Core.Common.Middleware.Options
{
    public interface IFluentCspPluginTypesDirective : IFluentInterface
    {
        /// <summary>
        ///     Sets the media types for the CSP plugin-types directive.
        /// </summary>
        void MediaTypes(params string[] pluginType);
    }
}