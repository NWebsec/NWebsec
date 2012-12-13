// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using NWebsec.Csp.Overrides;
using NWebsec.HttpHeaders;
using NWebsec.Modules.Configuration.Csp;

namespace NWebsec.Mvc.HttpHeaders.Csp
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class CspDefaultSrcAttribute : CspDirectiveAttributeBase
    {
        protected override HttpHeaderHelper.CspDirectives Directive
        {
            get { return HttpHeaderHelper.CspDirectives.DefaultSrc; }
        }

        protected override bool ReportOnly
        {
            get { return false; }
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class CspScriptSrcAttribute : CspDirectiveAttributeBase
    {
        public Source UnsafeInline { get; set; }
        public Source UnsafeEval { get; set; }

        protected override HttpHeaderHelper.CspDirectives Directive
        {
            get { return HttpHeaderHelper.CspDirectives.ScriptSrc; }
        }

        protected override bool ReportOnly
        {
            get { return false; }
        }

        protected override CspDirectiveBaseOverride GetNewDirectiveConfigurationElement()
        {
            return new CspDirectiveUnsafeInlineUnsafeEvalOverride { UnsafeInline = UnsafeInline, UnsafeEval = UnsafeEval };
        }

        protected override void ValidateParams()
        {
            if (UnsafeInline != Source.Inherit || UnsafeEval != Source.Inherit)
                return;

            base.ValidateParams();
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class CspObjectSrcAttribute : CspDirectiveAttributeBase
    {
        protected override HttpHeaderHelper.CspDirectives Directive
        {
            get { return HttpHeaderHelper.CspDirectives.ObjectSrc; }
        }

        protected override bool ReportOnly
        {
            get { return false; }
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class CspStyleSrcAttribute : CspDirectiveAttributeBase
    {
        public Source UnsafeInline { get; set; }

        protected override HttpHeaderHelper.CspDirectives Directive
        {
            get { return HttpHeaderHelper.CspDirectives.StyleSrc; }
        }

        protected override bool ReportOnly
        {
            get { return false; }
        }

        protected override CspDirectiveBaseOverride GetNewDirectiveConfigurationElement()
        {
            return new CspDirectiveUnsafeInlineOverride() { UnsafeInline = UnsafeInline };
        }

        protected override void ValidateParams()
        {
            if (UnsafeInline != Source.Inherit)
                return;

            base.ValidateParams();
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class CspImgSrcAttribute : CspDirectiveAttributeBase
    {
        protected override HttpHeaderHelper.CspDirectives Directive
        {
            get { return HttpHeaderHelper.CspDirectives.ImgSrc; }
        }

        protected override bool ReportOnly
        {
            get { return false; }
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class CspMediaSrcAttribute : CspDirectiveAttributeBase
    {
        protected override HttpHeaderHelper.CspDirectives Directive
        {
            get { return HttpHeaderHelper.CspDirectives.MediaSrc; }
        }

        protected override bool ReportOnly
        {
            get { return false; }
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class CspFrameSrcAttribute : CspDirectiveAttributeBase
    {
        protected override HttpHeaderHelper.CspDirectives Directive
        {
            get { return HttpHeaderHelper.CspDirectives.FrameSrc; }
        }

        protected override bool ReportOnly
        {
            get { return false; }
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class CspFontSrcAttribute : CspDirectiveAttributeBase
    {
        protected override HttpHeaderHelper.CspDirectives Directive
        {
            get { return HttpHeaderHelper.CspDirectives.FontSrc; }
        }

        protected override bool ReportOnly
        {
            get { return false; }
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class CspConnectSrcAttribute : CspDirectiveAttributeBase
    {
        protected override HttpHeaderHelper.CspDirectives Directive
        {
            get { return HttpHeaderHelper.CspDirectives.ConnectSrc; }
        }

        protected override bool ReportOnly
        {
            get { return false; }
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class CspDefaultSrcReportOnlyAttribute : CspDirectiveAttributeBase
    {
        protected override HttpHeaderHelper.CspDirectives Directive
        {
            get { return HttpHeaderHelper.CspDirectives.DefaultSrc; }
        }

        protected override bool ReportOnly
        {
            get { return true; }
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class CspScriptSrcReportOnlyAttribute : CspScriptSrcAttribute
    {
        protected override bool ReportOnly
        {
            get { return true; }
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class CspObjectSrcReportOnlyAttribute : CspDirectiveAttributeBase
    {
        protected override HttpHeaderHelper.CspDirectives Directive
        {
            get { return HttpHeaderHelper.CspDirectives.ObjectSrc; }
        }

        protected override bool ReportOnly
        {
            get { return true; }
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class CspStyleSrcReportOnlyAttribute : CspStyleSrcAttribute
    {
        protected override bool ReportOnly
        {
            get { return true; }
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class CspImgSrcReportOnlyAttribute : CspDirectiveAttributeBase
    {
        protected override HttpHeaderHelper.CspDirectives Directive
        {
            get { return HttpHeaderHelper.CspDirectives.ImgSrc; }
        }

        protected override bool ReportOnly
        {
            get { return true; }
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class CspMediaSrcReportOnlyAttribute : CspDirectiveAttributeBase
    {
        protected override HttpHeaderHelper.CspDirectives Directive
        {
            get { return HttpHeaderHelper.CspDirectives.MediaSrc; }
        }

        protected override bool ReportOnly
        {
            get { return true; }
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class CspFrameSrcReportOnlyAttribute : CspDirectiveAttributeBase
    {
        protected override HttpHeaderHelper.CspDirectives Directive
        {
            get { return HttpHeaderHelper.CspDirectives.FrameSrc; }
        }

        protected override bool ReportOnly
        {
            get { return true; }
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class CspFontSrcReportOnlyAttribute : CspDirectiveAttributeBase
    {
        protected override HttpHeaderHelper.CspDirectives Directive
        {
            get { return HttpHeaderHelper.CspDirectives.FontSrc; }
        }

        protected override bool ReportOnly
        {
            get { return true; }
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class CspConnectSrcReportOnlyAttribute : CspDirectiveAttributeBase
    {
        protected override HttpHeaderHelper.CspDirectives Directive
        {
            get { return HttpHeaderHelper.CspDirectives.ConnectSrc; }
        }

        protected override bool ReportOnly
        {
            get { return true; }
        }
    }
}