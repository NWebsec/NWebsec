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
        /// Configures the default-src directive (CSP 1.0).
        /// </summary>
        /// <param name="configurer">An <see cref="Action"/> that configures the sources for the directive.</param>
        /// <returns>The current <see cref="CspOptions" /> instance.</returns>
        IFluentCspOptions DefaultSources(Action<ICspDirectiveBasicConfiguration> configurer);

        /// <summary>
        /// Configures the script-src directive (CSP 1.0).
        /// </summary>
        /// <param name="configurer">An <see cref="Action"/> that configures the sources for the directive.</param>
        /// <returns>The current <see cref="CspOptions" /> instance.</returns>
        IFluentCspOptions ScriptSources(Action<ICspDirectiveConfiguration> configurer);

        /// <summary>
        /// Configures the object-src directive (CSP 1.0).
        /// </summary>
        /// <param name="configurer">An <see cref="Action"/> that configures the sources for the directive.</param>
        /// <returns>The current <see cref="CspOptions" /> instance.</returns>
        IFluentCspOptions ObjectSources(Action<ICspDirectiveBasicConfiguration> configurer);

        /// <summary>
        /// Configures the style-src directive (CSP 1.0).
        /// </summary>
        /// <param name="configurer">An <see cref="Action"/> that configures the sources for the directive.</param>
        /// <returns>The current <see cref="CspOptions" /> instance.</returns>
        IFluentCspOptions StyleSources(Action<ICspDirectiveUnsafeInlineConfiguration> configurer);

        /// <summary>
        /// Configures the image-src directive (CSP 1.0).
        /// </summary>
        /// <param name="configurer">An <see cref="Action"/> that configures the sources for the directive.</param>
        /// <returns>The current <see cref="CspOptions" /> instance.</returns>
        IFluentCspOptions ImageSources(Action<ICspDirectiveBasicConfiguration> configurer);

        /// <summary>
        /// Configures the media-src directive (CSP 1.0).
        /// </summary>
        /// <param name="configurer">An <see cref="Action"/> that configures the sources for the directive.</param>
        /// <returns>The current <see cref="CspOptions" /> instance.</returns>
        IFluentCspOptions MediaSources(Action<ICspDirectiveBasicConfiguration> configurer);

        /// <summary>
        /// Configures the frame-src directive (CSP 1.0).
        /// </summary>
        /// <param name="configurer">An <see cref="Action"/> that configures the sources for the directive.</param>
        /// <returns>The current <see cref="CspOptions" /> instance.</returns>
        IFluentCspOptions FrameSources(Action<ICspDirectiveBasicConfiguration> configurer);

        /// <summary>
        /// Configures the font-src directive (CSP 1.0).
        /// </summary>
        /// <param name="configurer">An <see cref="Action"/> that configures the sources for the directive.</param>
        /// <returns>The current <see cref="CspOptions" /> instance.</returns>
        IFluentCspOptions FontSources(Action<ICspDirectiveBasicConfiguration> configurer);

        /// <summary>
        /// Configures the connect-src directive (CSP 1.0).
        /// </summary>
        /// <param name="configurer">An <see cref="Action"/> that configures the sources for the directive.</param>
        /// <returns>The current <see cref="CspOptions" /> instance.</returns>
        IFluentCspOptions ConnectSources(Action<ICspDirectiveBasicConfiguration> configurer);

        /// <summary>
        /// Configures the base-uri directive (CSP 2).
        /// </summary>
        /// <param name="configurer">An <see cref="Action"/> that configures the sources for the directive.</param>
        /// <returns>The current <see cref="CspOptions" /> instance.</returns>
        IFluentCspOptions BaseUris(Action<ICspDirectiveBasicConfiguration> configurer);

        /// <summary>
        /// Configures the child-src directive (CSP 2).
        /// </summary>
        /// <param name="configurer">An <see cref="Action"/> that configures the sources for the directive.</param>
        /// <returns>The current <see cref="CspOptions" /> instance.</returns>
        IFluentCspOptions ChildSources(Action<ICspDirectiveBasicConfiguration> configurer);

        /// <summary>
        /// Configures the form-action directive (CSP 2).
        /// </summary>
        /// <param name="configurer">An <see cref="Action"/> that configures the sources for the directive.</param>
        /// <returns>The current <see cref="CspOptions" /> instance.</returns>
        IFluentCspOptions FormActions(Action<ICspDirectiveBasicConfiguration> configurer);

        /// <summary>
        /// Configures the fram-ancestors directive (CSP 2).
        /// </summary>
        /// <param name="configurer">An <see cref="Action"/> that configures the sources for the directive.</param>
        /// <returns>The current <see cref="CspOptions" /> instance.</returns>
        IFluentCspOptions FrameAncestors(Action<ICspDirectiveBasicConfiguration> configurer);

        /// <summary>
        /// Configures the report-uri directive (CSP 1.0). Support for absolute URIs was introduced in CSP 2.
        /// </summary>
        /// <param name="configurer">An <see cref="Action"/> that configures the report URIs.</param>
        /// <returns>The current <see cref="CspOptions" /> instance.</returns>
        IFluentCspOptions ReportUris(Action<IFluentCspReportUriDirective> configurer);
    }
}