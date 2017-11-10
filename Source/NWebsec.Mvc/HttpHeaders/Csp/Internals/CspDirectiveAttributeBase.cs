// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Web.Mvc;
using NWebsec.Core.HttpHeaders.Csp;
using NWebsec.Mvc.Csp;
using NWebsec.Mvc.Helpers;
using NWebsec.Mvc.HttpHeaders.Internals;

namespace NWebsec.Mvc.HttpHeaders.Csp.Internals
{
    /// <summary>
    /// This is a base class which should not be used directly.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public abstract class CspDirectiveAttributeBase : HttpHeaderAttributeBase
    {
        private readonly CspConfigurationOverrideHelper _headerConfigurationOverrideHelper;
        private readonly HeaderOverrideHelper _headerOverrideHelper;
        protected CspDirectiveOverride DirectiveConfig;
        private bool _paramsValidated;

        protected CspDirectiveAttributeBase()
        {
            DirectiveConfig = new CspDirectiveOverride()
            {
                Enabled = true,
                InheritOtherSources = true
            };
            _headerConfigurationOverrideHelper = new CspConfigurationOverrideHelper();
            _headerOverrideHelper = new HeaderOverrideHelper();
        }

        internal sealed override string ContextKeyIdentifier
        {
            get { return ReportOnly ? "CspReportOnly" : "Csp"; }
        }

        /// <summary>
        /// Gets or sets whether the CSP directive is enabled in the CSP header. The default is true.
        /// </summary>
        public bool Enabled { get { return DirectiveConfig.Enabled; } set { DirectiveConfig.Enabled = value; } }

        /// <summary>
        /// Sets whether the 'none' source is included in the directive. Not setting it will inherit existing configuration.
        /// </summary>
        public bool None { get { throw new NotSupportedException(); } set { DirectiveConfig.None = value; } }
        /// <summary>
        /// Gets or sets whether the 'self' source is included in the directive. Not setting it will inherit existing configuration (default behaviour).
        /// </summary>
        public bool Self { get { throw new NotSupportedException(); } set { DirectiveConfig.Self = value; } }
        /// <summary>
        /// Gets or sets whether CustomSources should be inherited from previous settings. The default is true.
        /// </summary>
        public bool InheritCustomSources { get { return DirectiveConfig.InheritOtherSources; } set { DirectiveConfig.InheritOtherSources = value; } }
        /// <summary>
        /// Gets or sets other sources for the directive. Sources are separated by exactly one whitespace. Source examples are scheme only ("https:"), any host ("*"),
        /// a host ("*.nwebsec.com", "www.nwebsec.com", "https://www.nwebsec.com", "www.nwebsec.com:443", "https://www.nwebsec.com:*").
        /// See the Content Security Policy Level 2 standard for details.
        /// </summary>
        public string CustomSources
        {
            get
            {
                throw new NotSupportedException();
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw CreateAttributeException("Sources cannot be set to null or an empty string.");

                if (value.StartsWith(" ") || value.EndsWith(" "))
                    throw CreateAttributeException("Sources must not contain leading or trailing whitespace: " + value);

                if (value.Contains("  "))
                    throw CreateAttributeException("Sources must be separated by exactly one whitespace: " + value);

                var sources = value.Split(' ');

                try
                {
                    DirectiveConfig.OtherSources = sources.Select(s => CspUriSource.Parse(s).ToString()).ToArray();
                }
                catch (Exception e)
                {
                    throw CreateAttributeException("Invalid source(s): " + value, e);
                }
            }
        }

        protected abstract CspDirectives Directive { get; }
        protected abstract bool ReportOnly { get; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ValidateParams();

            _headerConfigurationOverrideHelper.SetCspDirectiveOverride(filterContext.HttpContext, Directive, DirectiveConfig, ReportOnly);

            base.OnActionExecuting(filterContext);
        }

        internal void ValidateParams()
        {
            if (_paramsValidated) return;

            if (Enabled && KeywordSourcesNotConfigured() && DirectiveConfig.OtherSources == null)
            {
                throw CreateAttributeException("No sources configured for attribute. Remove attribute, or set \"Enabled=false\".");
            }

            if (NoneCombinedWithOtherSources())
            {
                throw CreateAttributeException("The 'none' source cannot be combined with other sources.");
                
            }
            _paramsValidated = true;
        }

        private bool KeywordSourcesNotConfigured()
        {
            return (DirectiveConfig.None.HasValue ||
                    DirectiveConfig.Self.HasValue ||
                    DirectiveConfig.StrictDynamic.HasValue || 
                    DirectiveConfig.UnsafeInline.HasValue ||
                    DirectiveConfig.UnsafeEval.HasValue) == false;
        }

        private bool NoneCombinedWithOtherSources()
        {
            if (!DirectiveConfig.None.HasValue || !(bool) DirectiveConfig.None)
            {
                //None not configured, or set to false
                return false;
            }

            return ((DirectiveConfig.Self.HasValue && (bool)DirectiveConfig.Self) ||
                    (DirectiveConfig.UnsafeInline.HasValue && (bool)DirectiveConfig.UnsafeInline) ||
                    (DirectiveConfig.UnsafeEval.HasValue && (bool)DirectiveConfig.UnsafeEval) ||
                    (DirectiveConfig.StrictDynamic.HasValue && (bool)DirectiveConfig.StrictDynamic) ||
                    DirectiveConfig.OtherSources != null);
        }
        
        public sealed override void SetHttpHeadersOnActionExecuted(ActionExecutedContext filterContext)
        {
            _headerOverrideHelper.SetCspHeaders(filterContext.HttpContext, ReportOnly);
        }
    }
}
