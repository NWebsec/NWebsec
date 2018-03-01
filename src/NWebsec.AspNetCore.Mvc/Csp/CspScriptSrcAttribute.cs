// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using NWebsec.AspNetCore.Mvc.Csp.Internals;
using NWebsec.Mvc.Common.Helpers;

namespace NWebsec.AspNetCore.Mvc.Csp
{
    /// <summary>
    /// When applied to a controller or action method, enables the script-src directive for the CSP header (CSP 1.0). 
    /// </summary>
    public class CspScriptSrcAttribute : CspDirectiveAttributeBase
    {
        /// <summary>
        /// Gets or sets whether the 'unsafe-inline' source is included in the directive. Not setting it will inherit existing configuration.
        /// </summary>
        public bool UnsafeInline { get => throw new NotSupportedException(); set => DirectiveConfig.UnsafeInline = value; }

        /// <summary>
        /// Gets or sets whether the 'unsafe-eval' source is included in the directive. Not setting it will inherit existing configuration.
        /// </summary>
        public bool UnsafeEval
        {
            get => throw new NotSupportedException(); set => DirectiveConfig.UnsafeEval = value;
        }

        /// <summary>
        /// Gets or sets whether the 'strict-dynamic' source is included in the directive. Not setting it will inherit existing configuration.
        /// </summary>
        public bool StrictDynamic
        {
            get => throw new NotSupportedException(); set => DirectiveConfig.StrictDynamic = value;
        }

        protected override CspDirectives Directive => CspDirectives.ScriptSrc;

        protected override bool ReportOnly => false;
    }
}