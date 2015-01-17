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
            SandboxDirective = new FluentCspSandboxDirective();
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
        public ICspSandboxDirectiveConfiguration SandboxDirective { get; set; }

        public ICspReportUriDirectiveConfiguration ReportUriDirective { get; set; }

        public IFluentCspOptions DefaultSources(Action<ICspDirectiveBasicConfiguration> configurer)
        {
            configurer(DefaultSrcDirective);
            return this;
        }

        public IFluentCspOptions ScriptSources(Action<ICspDirectiveConfiguration> configurer)
        {
            configurer(ScriptSrcDirective);
            return this;
        }

        public IFluentCspOptions ObjectSources(Action<ICspDirectiveBasicConfiguration> configurer)
        {
            configurer(ObjectSrcDirective);
            return this;
        }

        public IFluentCspOptions StyleSources(Action<ICspDirectiveUnsafeInlineConfiguration> configurer)
        {
            configurer(StyleSrcDirective);
            return this;
        }

        public IFluentCspOptions ImageSources(Action<ICspDirectiveBasicConfiguration> configurer)
        {
            configurer(ImgSrcDirective);
            return this;
        }

        public IFluentCspOptions MediaSources(Action<ICspDirectiveBasicConfiguration> configurer)
        {
            configurer(MediaSrcDirective);
            return this;
        }

        public IFluentCspOptions FrameSources(Action<ICspDirectiveBasicConfiguration> configurer)
        {
            configurer(FrameSrcDirective);
            return this;
        }

        public IFluentCspOptions FontSources(Action<ICspDirectiveBasicConfiguration> configurer)
        {
            configurer(FontSrcDirective);
            return this;
        }

        public IFluentCspOptions ConnectSources(Action<ICspDirectiveBasicConfiguration> configurer)
        {
            configurer(ConnectSrcDirective);
            return this;
        }

        public IFluentCspOptions BaseUris(Action<ICspDirectiveBasicConfiguration> configurer)
        {
            configurer(BaseUriDirective);
            return this;
        }

        public IFluentCspOptions ChildSources(Action<ICspDirectiveBasicConfiguration> configurer)
        {
            configurer(ChildSrcDirective);
            return this;
        }

        public IFluentCspOptions FormActions(Action<ICspDirectiveBasicConfiguration> configurer)
        {
            configurer(FormActionDirective);
            return this;
        }

        public IFluentCspOptions FrameAncestors(Action<ICspDirectiveBasicConfiguration> configurer)
        {
            configurer(FrameAncestorsDirective);
            return this;
        }

        public IFluentCspOptions Sandbox(Action<IFluentCspSandboxDirective> configurer)
        {
            configurer((IFluentCspSandboxDirective)SandboxDirective);
            return this;
        }

        public IFluentCspOptions ReportUris(Action<IFluentCspReportUriDirective> configurer)
        {
            configurer((IFluentCspReportUriDirective)ReportUriDirective);
            return this;
        }
    }
}