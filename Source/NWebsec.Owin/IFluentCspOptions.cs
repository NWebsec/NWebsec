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
        CspOptions DefaultSources(Action<ICspDirectiveConfiguration> configurer);

        /// <summary>
        /// Configures the script-src directive.
        /// </summary>
        /// <param name="configurer">An <see cref="Action"/> that configures the sources for the directive.</param>
        /// <returns>The current <see cref="CspOptions" /> instance.</returns>
        CspOptions ScriptSources(Action<ICspDirectiveUnsafeEvalConfiguration> configurer);

        /// <summary>
        /// Configures the object-src directive.
        /// </summary>
        /// <param name="configurer">An <see cref="Action"/> that configures the sources for the directive.</param>
        /// <returns>The current <see cref="CspOptions" /> instance.</returns>
        CspOptions ObjectSources(Action<ICspDirectiveConfiguration> configurer);

        /// <summary>
        /// Configures the style-src directive.
        /// </summary>
        /// <param name="configurer">An <see cref="Action"/> that configures the sources for the directive.</param>
        /// <returns>The current <see cref="CspOptions" /> instance.</returns>
        CspOptions StyleSources(Action<ICspDirectiveUnsafeInlineConfiguration> configurer);

        /// <summary>
        /// Configures the image-src directive.
        /// </summary>
        /// <param name="configurer">An <see cref="Action"/> that configures the sources for the directive.</param>
        /// <returns>The current <see cref="CspOptions" /> instance.</returns>
        CspOptions ImageSources(Action<ICspDirectiveConfiguration> configurer);

        /// <summary>
        /// Configures the media-src directive.
        /// </summary>
        /// <param name="configurer">An <see cref="Action"/> that configures the sources for the directive.</param>
        /// <returns>The current <see cref="CspOptions" /> instance.</returns>
        CspOptions MediaSources(Action<ICspDirectiveConfiguration> configurer);

        /// <summary>
        /// Configures the frame-src directive.
        /// </summary>
        /// <param name="configurer">An <see cref="Action"/> that configures the sources for the directive.</param>
        /// <returns>The current <see cref="CspOptions" /> instance.</returns>
        CspOptions FrameSources(Action<ICspDirectiveConfiguration> configurer);

        /// <summary>
        /// Configures the font-src directive.
        /// </summary>
        /// <param name="configurer">An <see cref="Action"/> that configures the sources for the directive.</param>
        /// <returns>The current <see cref="CspOptions" /> instance.</returns>
        CspOptions FontSources(Action<ICspDirectiveConfiguration> configurer);

        /// <summary>
        /// Configures the connect-src directive.
        /// </summary>
        /// <param name="configurer">An <see cref="Action"/> that configures the sources for the directive.</param>
        /// <returns>The current <see cref="CspOptions" /> instance.</returns>
        CspOptions ConnectSources(Action<ICspDirectiveConfiguration> configurer);
    }
}