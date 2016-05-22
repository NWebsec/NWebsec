// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using NWebsec.AspNetCore.Mvc.Helpers;
using NWebsec.AspNetCore.Mvc.HttpHeaders.Csp.Internals;

namespace NWebsec.AspNetCore.Mvc.HttpHeaders.Csp
{
    /// <summary>
    /// When applied to a controller or action method, enables the style-src directive for the CSP header (CSP 1.0). 
    /// </summary>
    public class CspStyleSrcAttribute : CspDirectiveAttributeBase
    {
        /// <summary>
        /// Gets or sets whether the 'unsafe-inline' source is included in the directive. Not setting it will inherit existing configuration.
        /// </summary>
        public bool UnsafeInline { get { throw new NotSupportedException(); } set { DirectiveConfig.UnsafeInline = value; } }

        protected override CspDirectives Directive => CspDirectives.StyleSrc;

        protected override bool ReportOnly => false;
    }
}