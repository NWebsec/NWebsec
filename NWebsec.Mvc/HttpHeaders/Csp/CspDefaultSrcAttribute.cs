// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using NWebsec.Csp.Overrides;
using NWebsec.HttpHeaders;

namespace NWebsec.Mvc.HttpHeaders.Csp
{
    /// <summary>
    /// When applied to a controller or action method, enables the default-src directive for the CSP header. 
    /// </summary>
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

    /// <summary>
    /// When applied to a controller or action method, enables the script-src directive for the CSP header. 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class CspScriptSrcAttribute : CspDirectiveAttributeBase
    {
        /// <summary>
        /// Gets or sets whether the 'unsafe-inline' source is included in the directive. Possible values are Inherit, Enabled or Disabled. The default is Inherit.
        /// </summary>
        public Source UnsafeInline { get; set; }
        /// <summary>
        /// Gets or sets whether the 'unsafe-eval' source is included in the directive. Possible values are Inherit, Enabled or Disabled. The default is Inherit.
        /// </summary>
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

    /// <summary>
    /// When applied to a controller or action method, enables the object-src directive for the CSP header. 
    /// </summary>
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

    /// <summary>
    /// When applied to a controller or action method, enables the style-src directive for the CSP header. 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class CspStyleSrcAttribute : CspDirectiveAttributeBase
    {
        /// <summary>
        /// Gets or sets whether the 'unsafe-inline' source is included in the directive. Possible values are Inherit, Enabled or Disabled. The default is Inherit.
        /// </summary>
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
            return new CspDirectiveUnsafeInlineOverride { UnsafeInline = UnsafeInline };
        }

        protected override void ValidateParams()
        {
            if (UnsafeInline != Source.Inherit)
                return;

            base.ValidateParams();
        }
    }

    /// <summary>
    /// When applied to a controller or action method, enables the img-src directive for the CSP header. 
    /// </summary>
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

    /// <summary>
    /// When applied to a controller or action method, enables the media-src directive for the CSP header. 
    /// </summary>
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

    /// <summary>
    /// When applied to a controller or action method, enables the frame-src directive for the CSP header. 
    /// </summary>
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

    /// <summary>
    /// When applied to a controller or action method, enables the font-src directive for the CSP header. 
    /// </summary>
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

    /// <summary>
    /// When applied to a controller or action method, enables the connect-src directive for the CSP header. 
    /// </summary>
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

    /// <summary>
    /// When applied to a controller or action method, enables the default-src directive for the CSP Report Only header. 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class CspDefaultSrcReportOnlyAttribute : CspDefaultSrcAttribute
    {
        protected override bool ReportOnly
        {
            get { return true; }
        }
    }

    /// <summary>
    /// When applied to a controller or action method, enables the script-src directive for the CSP Report Only header. 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class CspScriptSrcReportOnlyAttribute : CspScriptSrcAttribute
    {
        protected override bool ReportOnly
        {
            get { return true; }
        }
    }

    /// <summary>
    /// When applied to a controller or action method, enables the object-src directive for the CSP Report Only header. 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class CspObjectSrcReportOnlyAttribute : CspObjectSrcAttribute
    {
        protected override bool ReportOnly
        {
            get { return true; }
        }
    }

    /// <summary>
    /// When applied to a controller or action method, enables the style-src directive for the CSP Report Only header. 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class CspStyleSrcReportOnlyAttribute : CspStyleSrcAttribute
    {
        protected override bool ReportOnly
        {
            get { return true; }
        }
    }

    /// <summary>
    /// When applied to a controller or action method, enables the img-src directive for the CSP Report Only header. 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class CspImgSrcReportOnlyAttribute : CspImgSrcAttribute
    {
        protected override bool ReportOnly
        {
            get { return true; }
        }
    }

    /// <summary>
    /// When applied to a controller or action method, enables the media-src directive for the CSP Report Only header. 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class CspMediaSrcReportOnlyAttribute : CspMediaSrcAttribute
    {
        protected override bool ReportOnly
        {
            get { return true; }
        }
    }

    /// <summary>
    /// When applied to a controller or action method, enables the frame-src directive for the CSP Report Only header. 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class CspFrameSrcReportOnlyAttribute : CspFrameSrcAttribute
    {
        protected override bool ReportOnly
        {
            get { return true; }
        }
    }

    /// <summary>
    /// When applied to a controller or action method, enables the font-src directive for the CSP Report Only header. 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class CspFontSrcReportOnlyAttribute : CspFontSrcAttribute
    {
        protected override bool ReportOnly
        {
            get { return true; }
        }
    }

    /// <summary>
    /// When applied to a controller or action method, enables the connect-src directive for the CSP Report Only header. 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class CspConnectSrcReportOnlyAttribute : CspConnectSrcAttribute
    {
        protected override bool ReportOnly
        {
            get { return true; }
        }
    }
}