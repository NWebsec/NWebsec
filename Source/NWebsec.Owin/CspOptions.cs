// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.ComponentModel;
using NWebsec.Core.HttpHeaders.Configuration;
using NWebsec.Owin.Fluent;

namespace NWebsec.Owin
{
    public class CspOptions : ICspConfiguration, IFluentInterface
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