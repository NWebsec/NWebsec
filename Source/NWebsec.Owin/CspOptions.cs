// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
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
            ReportUriDirective = new CspReportUriDirective();
        }

        public bool Enabled { get; set; }

        public bool XContentSecurityPolicyHeader { get; set; }

        public bool XWebKitCspHeader { get; set; }

        public ICspDirectiveConfiguration DefaultSrcDirective { get; set; }

        public ICspDirectiveUnsafeEvalConfiguration ScriptSrcDirective { get; set; }

        public ICspDirectiveConfiguration ObjectSrcDirective { get; set; }

        public ICspDirectiveUnsafeInlineConfiguration StyleSrcDirective { get; set; }

        public ICspDirectiveConfiguration ImgSrcDirective { get; set; }

        public ICspDirectiveConfiguration MediaSrcDirective { get; set; }

        public ICspDirectiveConfiguration FrameSrcDirective { get; set; }

        public ICspDirectiveConfiguration FontSrcDirective { get; set; }

        public ICspDirectiveConfiguration ConnectSrcDirective { get; set; }

        public ICspReportUriDirectiveConfiguration ReportUriDirective { get; set; }

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

        /// <summary>
        /// Configures the CSP report-uri directive.
        /// </summary>
        /// <param name="configurer">An <see cref="Action"/> that configures the report URIs.</param>
        /// <returns>The current <see cref="CspOptions" /> instance.</returns>
        public CspOptions ReportUris(Action<IFluentCspReportUriDirective> configurer)
        {
            configurer((IFluentCspReportUriDirective)ReportUriDirective);
            return this;
        }
    }
}