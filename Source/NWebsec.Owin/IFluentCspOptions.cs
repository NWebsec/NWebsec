// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using NWebsec.Core.HttpHeaders.Configuration;
using NWebsec.Fluent;

namespace NWebsec.Owin
{
    /// <summary>
    /// Fluent interface to configure options for Content-Security-Options.
    /// </summary>
    public interface IFluentCspOptions : IFluentInterface
    {
        /// <summary>
        /// Configures the default-src directive.
        /// </summary>
        /// <param name="configurer">An <see cref="Action"/> that configures the sources for the directive.</param>
        /// <returns>The current <see cref="CspOptions" /> instance.</returns>
        IFluentCspOptions DefaultSources(Action<ICspDirectiveConfiguration> configurer);

        /// <summary>
        /// Configures the script-src directive.
        /// </summary>
        /// <param name="configurer">An <see cref="Action"/> that configures the sources for the directive.</param>
        /// <returns>The current <see cref="CspOptions" /> instance.</returns>
        IFluentCspOptions ScriptSources(Action<ICspDirectiveUnsafeEvalConfiguration> configurer);

        /// <summary>
        /// Configures the object-src directive.
        /// </summary>
        /// <param name="configurer">An <see cref="Action"/> that configures the sources for the directive.</param>
        /// <returns>The current <see cref="CspOptions" /> instance.</returns>
        IFluentCspOptions ObjectSources(Action<ICspDirectiveConfiguration> configurer);

        /// <summary>
        /// Configures the style-src directive.
        /// </summary>
        /// <param name="configurer">An <see cref="Action"/> that configures the sources for the directive.</param>
        /// <returns>The current <see cref="CspOptions" /> instance.</returns>
        IFluentCspOptions StyleSources(Action<ICspDirectiveUnsafeInlineConfiguration> configurer);

        /// <summary>
        /// Configures the image-src directive.
        /// </summary>
        /// <param name="configurer">An <see cref="Action"/> that configures the sources for the directive.</param>
        /// <returns>The current <see cref="CspOptions" /> instance.</returns>
        IFluentCspOptions ImageSources(Action<ICspDirectiveConfiguration> configurer);

        /// <summary>
        /// Configures the media-src directive.
        /// </summary>
        /// <param name="configurer">An <see cref="Action"/> that configures the sources for the directive.</param>
        /// <returns>The current <see cref="CspOptions" /> instance.</returns>
        IFluentCspOptions MediaSources(Action<ICspDirectiveConfiguration> configurer);

        /// <summary>
        /// Configures the frame-src directive.
        /// </summary>
        /// <param name="configurer">An <see cref="Action"/> that configures the sources for the directive.</param>
        /// <returns>The current <see cref="CspOptions" /> instance.</returns>
        IFluentCspOptions FrameSources(Action<ICspDirectiveConfiguration> configurer);

        /// <summary>
        /// Configures the font-src directive.
        /// </summary>
        /// <param name="configurer">An <see cref="Action"/> that configures the sources for the directive.</param>
        /// <returns>The current <see cref="CspOptions" /> instance.</returns>
        IFluentCspOptions FontSources(Action<ICspDirectiveConfiguration> configurer);

        /// <summary>
        /// Configures the connect-src directive.
        /// </summary>
        /// <param name="configurer">An <see cref="Action"/> that configures the sources for the directive.</param>
        /// <returns>The current <see cref="CspOptions" /> instance.</returns>
        IFluentCspOptions ConnectSources(Action<ICspDirectiveConfiguration> configurer);

        /// <summary>
        /// Configures the fram-ancestors directive.
        /// </summary>
        /// <param name="configurer">An <see cref="Action"/> that configures the sources for the directive.</param>
        /// <returns>The current <see cref="CspOptions" /> instance.</returns>
        IFluentCspOptions FrameAncestors(Action<ICspDirectiveConfiguration> configurer);

        /// <summary>
        /// Configures the report-uri directive.
        /// </summary>
        /// <param name="configurer">An <see cref="Action"/> that configures the report URIs.</param>
        /// <returns>The current <see cref="CspOptions" /> instance.</returns>
        IFluentCspOptions ReportUris(Action<IFluentCspReportUriDirective> configurer);
    }
}