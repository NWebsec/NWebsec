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
            ScriptSrcDirective = new CspDirective();
            ObjectSrcDirective = new CspDirective();
            StyleSrcDirective = new CspDirective();
            ImgSrcDirective = new CspDirective();
            MediaSrcDirective = new CspDirective();
            FrameSrcDirective = new CspDirective();
            FontSrcDirective = new CspDirective();
            ConnectSrcDirective = new CspDirective();
            BaseUriDirective = new CspDirective();
            ChildSrcDirective = new CspDirective();
            FormActionDirective = new CspDirective();
            FrameAncestorsDirective = new CspDirective();
            ReportUriDirective = new CspReportUriDirective();
        }

        public bool Enabled { get; set; }

        public ICspDirectiveConfiguration DefaultSrcDirective { get; set; }

        public ICspDirectiveConfiguration ScriptSrcDirective { get; set; }

        public ICspDirectiveConfiguration ObjectSrcDirective { get; set; }

        public ICspDirectiveConfiguration StyleSrcDirective { get; set; }

        public ICspDirectiveConfiguration ImgSrcDirective { get; set; }

        public ICspDirectiveConfiguration MediaSrcDirective { get; set; }

        public ICspDirectiveConfiguration FrameSrcDirective { get; set; }

        public ICspDirectiveConfiguration FontSrcDirective { get; set; }

        public ICspDirectiveConfiguration ConnectSrcDirective { get; set; }

        public ICspDirectiveConfiguration BaseUriDirective { get; set; }

        public ICspDirectiveConfiguration ChildSrcDirective { get; set; }

        public ICspDirectiveConfiguration FormActionDirective { get; set; }

        public ICspDirectiveConfiguration FrameAncestorsDirective { get; set; }

        public ICspReportUriDirectiveConfiguration ReportUriDirective { get; set; }

        /// <summary>
        /// Configures the default-src directive (CSP 1.0).
        /// </summary>
        /// <param name="configurer">An <see cref="Action"/> that configures the sources for the directive.</param>
        /// <returns>The current <see cref="CspOptions" /> instance.</returns>
        public IFluentCspOptions DefaultSources(Action<ICspDirectiveBasicConfiguration> configurer)
        {
            configurer(DefaultSrcDirective);
            return this;
        }

        /// <summary>
        /// Configures the script-src directive (CSP 1.0).
        /// </summary>
        /// <param name="configurer">An <see cref="Action"/> that configures the sources for the directive.</param>
        /// <returns>The current <see cref="CspOptions" /> instance.</returns>
        public IFluentCspOptions ScriptSources(Action<ICspDirectiveConfiguration> configurer)
        {
            configurer(ScriptSrcDirective);
            return this;
        }

        /// <summary>
        /// Configures the object-src directive (CSP 1.0).
        /// </summary>
        /// <param name="configurer">An <see cref="Action"/> that configures the sources for the directive.</param>
        /// <returns>The current <see cref="CspOptions" /> instance.</returns>
        public IFluentCspOptions ObjectSources(Action<ICspDirectiveBasicConfiguration> configurer)
        {
            configurer(ObjectSrcDirective);
            return this;
        }

        /// <summary>
        /// Configures the style-src directive (CSP 1.0).
        /// </summary>
        /// <param name="configurer">An <see cref="Action"/> that configures the sources for the directive.</param>
        /// <returns>The current <see cref="CspOptions" /> instance.</returns>
        public IFluentCspOptions StyleSources(Action<ICspDirectiveUnsafeInlineConfiguration> configurer)
        {
            configurer(StyleSrcDirective);
            return this;
        }

        /// <summary>
        /// Configures the image-src directive (CSP 1.0).
        /// </summary>
        /// <param name="configurer">An <see cref="Action"/> that configures the sources for the directive.</param>
        /// <returns>The current <see cref="CspOptions" /> instance.</returns>
        public IFluentCspOptions ImageSources(Action<ICspDirectiveBasicConfiguration> configurer)
        {
            configurer(ImgSrcDirective);
            return this;
        }

        /// <summary>
        /// Configures the media-src directive (CSP 1.0).
        /// </summary>
        /// <param name="configurer">An <see cref="Action"/> that configures the sources for the directive.</param>
        /// <returns>The current <see cref="CspOptions" /> instance.</returns>
        public IFluentCspOptions MediaSources(Action<ICspDirectiveBasicConfiguration> configurer)
        {
            configurer(MediaSrcDirective);
            return this;
        }

        /// <summary>
        /// Configures the frame-src directive (CSP 1.0).
        /// </summary>
        /// <remarks>This directive has been deprecated in CSP 2, consider using child-src instead.</remarks>
        /// <param name="configurer">An <see cref="Action"/> that configures the sources for the directive.</param>
        /// <returns>The current <see cref="CspOptions" /> instance.</returns>
        public IFluentCspOptions FrameSources(Action<ICspDirectiveBasicConfiguration> configurer)
        {
            configurer(FrameSrcDirective);
            return this;
        }

        /// <summary>
        /// Configures the font-src directive (CSP 1.0).
        /// </summary>
        /// <param name="configurer">An <see cref="Action"/> that configures the sources for the directive.</param>
        /// <returns>The current <see cref="CspOptions" /> instance.</returns>
        public IFluentCspOptions FontSources(Action<ICspDirectiveBasicConfiguration> configurer)
        {
            configurer(FontSrcDirective);
            return this;
        }

        /// <summary>
        /// Configures the connect-src directive (CSP 1.0).
        /// </summary>
        /// <param name="configurer">An <see cref="Action"/> that configures the sources for the directive.</param>
        /// <returns>The current <see cref="CspOptions" /> instance.</returns>
        public IFluentCspOptions ConnectSources(Action<ICspDirectiveBasicConfiguration> configurer)
        {
            configurer(ConnectSrcDirective);
            return this;
        }

        /// <summary>
        /// Configures the base-uri directive (CSP 2).
        /// </summary>
        /// <param name="configurer">An <see cref="Action"/> that configures the sources for the directive.</param>
        /// <returns>The current <see cref="CspOptions" /> instance.</returns>
        public IFluentCspOptions BaseUris(Action<ICspDirectiveBasicConfiguration> configurer)
        {
            configurer(BaseUriDirective);
            return this;
        }

        /// <summary>
        /// Configures the child-src directive (CSP 2).
        /// </summary>
        /// <param name="configurer">An <see cref="Action"/> that configures the sources for the directive.</param>
        /// <returns>The current <see cref="CspOptions" /> instance.</returns>
        public IFluentCspOptions ChildSources(Action<ICspDirectiveBasicConfiguration> configurer)
        {
            configurer(ChildSrcDirective);
            return this;
        }

        /// <summary>
        /// Configures the form-action directive (CSP 2).
        /// </summary>
        /// <param name="configurer">An <see cref="Action"/> that configures the sources for the directive.</param>
        /// <returns>The current <see cref="CspOptions" /> instance.</returns>
        public IFluentCspOptions FormActions(Action<ICspDirectiveBasicConfiguration> configurer)
        {
            configurer(FormActionDirective);
            return this;
        }

        /// <summary>
        /// Configures the frame-ancestors directive (CSP 2).
        /// </summary>
        /// <param name="configurer">An <see cref="Action"/> that configures the sources for the directive.</param>
        /// <returns>The current <see cref="CspOptions" /> instance.</returns>
        public IFluentCspOptions FrameAncestors(Action<ICspDirectiveBasicConfiguration> configurer)
        {
            configurer(FrameAncestorsDirective);
            return this;
        }

        /// <summary>
        /// Configures the report-uri directive (CSP 1). Support for absolute URIs was introduced in CSP 2.
        /// </summary>
        /// <param name="configurer">An <see cref="Action"/> that configures the report URIs.</param>
        /// <returns>The current <see cref="CspOptions" /> instance.</returns>
        public IFluentCspOptions ReportUris(Action<IFluentCspReportUriDirective> configurer)
        {
            configurer((IFluentCspReportUriDirective)ReportUriDirective);
            return this;
        }
    }
}