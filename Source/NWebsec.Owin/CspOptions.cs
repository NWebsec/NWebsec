// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.ComponentModel;
using NWebsec.Core.HttpHeaders.Configuration;

namespace NWebsec.Owin
{
    public class CspOptions : ICspConfiguration, IFluentCspOptions
    {
        internal CspOptions()
        {
            Enabled = true;
            DefaultSrcDirective = new CspDirective();
            ScriptSrcDirective = new CspDirectiveUnsafeEval();
            ObjectSrcDirective = new CspDirective();
            StyleSrcDirective = new CspDirectiveUnsafeInline();
            ImgSrcDirective = new CspDirective();
            MediaSrcDirective = new CspDirective();
            FrameSrcDirective = new CspDirective();
            FontSrcDirective = new CspDirective();
            ConnectSrcDirective = new CspDirective();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool Enabled { get; set; }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool XContentSecurityPolicyHeader { get; set; }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool XWebKitCspHeader { get; set; }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public ICspDirectiveConfiguration DefaultSrcDirective { get; set; }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public ICspDirectiveUnsafeEvalConfiguration ScriptSrcDirective { get; set; }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public ICspDirectiveConfiguration ObjectSrcDirective { get; set; }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public ICspDirectiveUnsafeInlineConfiguration StyleSrcDirective { get; set; }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public ICspDirectiveConfiguration ImgSrcDirective { get; set; }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public ICspDirectiveConfiguration MediaSrcDirective { get; set; }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public ICspDirectiveConfiguration FrameSrcDirective { get; set; }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public ICspDirectiveConfiguration FontSrcDirective { get; set; }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public ICspDirectiveConfiguration ConnectSrcDirective { get; set; }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public ICspReportUriDirective ReportUriDirective { get; set; }

        /// <summary>
        /// Configures the CSP default-src directive.
        /// </summary>
        /// <param name="configurer">An <see cref="Action"/> that configures the sources for the directive.</param>
        /// <returns>The current <see cref="CspOptions" /> instance.</returns>
        public CspOptions DefaultSources(Action<ICspDirectiveConfiguration> configurer)
        {
            configurer(DefaultSrcDirective);
            return this;
        }

        /// <summary>
        /// Configures the CSP script-src directive.
        /// </summary>
        /// <param name="configurer">An <see cref="Action"/> that configures the sources for the directive.</param>
        /// <returns>The current <see cref="CspOptions" /> instance.</returns>
        public CspOptions ScriptSources(Action<ICspDirectiveUnsafeEvalConfiguration> configurer)
        {
            configurer(ScriptSrcDirective);
            return this;
        }

        /// <summary>
        /// Configures the CSP object-src directive.
        /// </summary>
        /// <param name="configurer">An <see cref="Action"/> that configures the sources for the directive.</param>
        /// <returns>The current <see cref="CspOptions" /> instance.</returns>
        public CspOptions ObjectSources(Action<ICspDirectiveConfiguration> configurer)
        {
            configurer(ObjectSrcDirective);
            return this;
        }

        /// <summary>
        /// Configures the CSP style-src directive.
        /// </summary>
        /// <param name="configurer">An <see cref="Action"/> that configures the sources for the directive.</param>
        /// <returns>The current <see cref="CspOptions" /> instance.</returns>
        public CspOptions StyleSources(Action<ICspDirectiveUnsafeInlineConfiguration> configurer)
        {
            configurer(StyleSrcDirective);
            return this;
        }

        /// <summary>
        /// Configures the CSP image-src directive.
        /// </summary>
        /// <param name="configurer">An <see cref="Action"/> that configures the sources for the directive.</param>
        /// <returns>The current <see cref="CspOptions" /> instance.</returns>
        public CspOptions ImageSources(Action<ICspDirectiveConfiguration> configurer)
        {
            configurer(ImgSrcDirective);
            return this;
        }

        /// <summary>
        /// Configures the CSP media-src directive.
        /// </summary>
        /// <param name="configurer">An <see cref="Action"/> that configures the sources for the directive.</param>
        /// <returns>The current <see cref="CspOptions" /> instance.</returns>
        public CspOptions MediaSources(Action<ICspDirectiveConfiguration> configurer)
        {
            configurer(MediaSrcDirective);
            return this;
        }

        /// <summary>
        /// Configures the CSP frame-src directive.
        /// </summary>
        /// <param name="configurer">An <see cref="Action"/> that configures the sources for the directive.</param>
        /// <returns>The current <see cref="CspOptions" /> instance.</returns>
        public CspOptions FrameSources(Action<ICspDirectiveConfiguration> configurer)
        {
            configurer(FrameSrcDirective);
            return this;
        }

        /// <summary>
        /// Configures the CSP font-src directive.
        /// </summary>
        /// <param name="configurer">An <see cref="Action"/> that configures the sources for the directive.</param>
        /// <returns>The current <see cref="CspOptions" /> instance.</returns>
        public CspOptions FontSources(Action<ICspDirectiveConfiguration> configurer)
        {
            configurer(FontSrcDirective);
            return this;
        }

        /// <summary>
        /// Configures the CSP connect-src directive.
        /// </summary>
        /// <param name="configurer">An <see cref="Action"/> that configures the sources for the directive.</param>
        /// <returns>The current <see cref="CspOptions" /> instance.</returns>
        public CspOptions ConnectSources(Action<ICspDirectiveConfiguration> configurer)
        {
            configurer(ConnectSrcDirective);
            return this;
        }
    }
}