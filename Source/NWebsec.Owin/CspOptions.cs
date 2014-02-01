// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;

namespace NWebsec.Owin
{
    public class CspOptions
    {
        public CspOptions()
        {
            DefaultSrcDirective = new CspDirective();
            ScriptSrc = new CspDirectiveUnsafeEval();
            ObjectSrc = new CspDirective();
            StyleSrc = new CspDirectiveUnsafeInline();
            ImgSrc = new CspDirective();
            MediaSrc = new CspDirective();
            FrameSrc = new CspDirective();
            FontSrc = new CspDirective();
            ConnectSrc = new CspDirective();
        }

        internal CspDirective DefaultSrcDirective { get; set; }
        public CspDirectiveUnsafeEval ScriptSrc { get; set; }
        public CspDirective ObjectSrc { get; set; }
        public CspDirectiveUnsafeInline StyleSrc { get; set; }
        public CspDirective ImgSrc { get; set; }
        public CspDirective MediaSrc { get; set; }
        public CspDirective FrameSrc { get; set; }
        public CspDirective FontSrc { get; set; }
        public CspDirective ConnectSrc { get; set; }

        public CspOptions WithDefaultSrc(Action<CspDirective> action)
        {
            action(DefaultSrcDirective);
            return this;
        }

        public CspOptions WithScriptSrc(Action<CspDirectiveUnsafeEval> action)
        {
            action(ScriptSrc);
            return this;
        }

        public CspOptions WithObjectSources(Action<CspDirective> action)
        {
            action(ObjectSrc);
            return this;
        }
        public CspOptions WithStyleSrc(Action<CspDirectiveUnsafeInline> action)
        {
            action(StyleSrc);
            return this;
        }
        public CspOptions WithImageSrc(Action<CspDirective> action)
        {
            action(ImgSrc);
            return this;
        }
        public CspOptions WithMediaSrc(Action<CspDirective> action)
        {
            action(MediaSrc);
            return this;
        }
        public CspOptions WithFrameSrc(Action<CspDirective> action)
        {
            action(FrameSrc);
            return this;
        }
        public CspOptions WithFontSrc(Action<CspDirective> action)
        {
            action(FontSrc);
            return this;
        }

        public CspOptions WithConnectSrc(Action<CspDirective> action)
        {
            action(ConnectSrc);
            return this;
        }

    }
}