// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using NWebsec.Core.HttpHeaders.Configuration;

namespace NWebsec.Owin
{
    

    public class CspOptions : ICspConfiguration
    {
        public CspOptions()
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
        public ICspReportUriDirective ReportUriDirective { get; set; }

        public CspOptions DefaultSources(Action<ICspDirectiveConfiguration> action)
        {
            action(DefaultSrcDirective as CspDirective);
            return this;
        }

        public CspOptions ScriptSources(Action<ICspDirectiveUnsafeEvalConfiguration> action)
        {
            action(ScriptSrcDirective);
            return this;
        }

        public CspOptions ObjectSources(Action<ICspDirectiveConfiguration> action)
        {
            action(ObjectSrcDirective);
            return this;
        }
        public CspOptions StyleSources(Action<ICspDirectiveUnsafeInlineConfiguration> action)
        {
            action(StyleSrcDirective);
            return this;
        }
        public CspOptions ImageSources(Action<ICspDirectiveConfiguration> action)
        {
            action(ImgSrcDirective);
            return this;
        }
        public CspOptions MediaSources(Action<ICspDirectiveConfiguration> action)
        {
            action(MediaSrcDirective);
            return this;
        }
        public CspOptions FrameSources(Action<ICspDirectiveConfiguration> action)
        {
            action(FrameSrcDirective);
            return this;
        }
        public CspOptions FontSources(Action<ICspDirectiveConfiguration> action)
        {
            action(FontSrcDirective);
            return this;
        }

        public CspOptions ConnectSources(Action<ICspDirectiveConfiguration> action)
        {
            action(ConnectSrcDirective);
            return this;
        }

    }
}
