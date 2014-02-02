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
        public ICspDirective DefaultSrcDirective { get; set; }
        public ICspDirectiveUnsafeEval ScriptSrcDirective { get; set; }
        public ICspDirective ObjectSrcDirective { get; set; }
        public ICspDirectiveUnsafeInline StyleSrcDirective { get; set; }
        public ICspDirective ImgSrcDirective { get; set; }
        public ICspDirective MediaSrcDirective { get; set; }
        public ICspDirective FrameSrcDirective { get; set; }
        public ICspDirective FontSrcDirective { get; set; }
        public ICspDirective ConnectSrcDirective { get; set; }

        public CspOptions WithDefaultSrc(Action<ICspDirective> action)
        {
            action(DefaultSrcDirective);
            return this;
        }

        public CspOptions WithScriptSrc(Action<ICspDirectiveUnsafeEval> action)
        {
            action(ScriptSrcDirective);
            return this;
        }

        public CspOptions WithObjectSources(Action<ICspDirective> action)
        {
            action(ObjectSrcDirective);
            return this;
        }
        public CspOptions WithStyleSrc(Action<ICspDirectiveUnsafeInline> action)
        {
            action(StyleSrcDirective);
            return this;
        }
        public CspOptions WithImageSrc(Action<ICspDirective> action)
        {
            action(ImgSrcDirective);
            return this;
        }
        public CspOptions WithMediaSrc(Action<ICspDirective> action)
        {
            action(MediaSrcDirective);
            return this;
        }
        public CspOptions WithFrameSrc(Action<ICspDirective> action)
        {
            action(FrameSrcDirective);
            return this;
        }
        public CspOptions WithFontSrc(Action<ICspDirective> action)
        {
            action(FontSrcDirective);
            return this;
        }

        public CspOptions WithConnectSrc(Action<ICspDirective> action)
        {
            action(ConnectSrcDirective);
            return this;
        }

    }
}
