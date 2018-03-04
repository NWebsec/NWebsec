// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Web.Mvc;
using NWebsec.Core.Common.HttpHeaders.Configuration.Validation;
using NWebsec.Core.Web;
using NWebsec.Csp;
using NWebsec.Mvc.Common.Csp;
using NWebsec.Mvc.Common.Helpers;
using NWebsec.Mvc.Helpers;
using NWebsec.Mvc.HttpHeaders.Internals;

namespace NWebsec.Mvc.HttpHeaders.Csp.Internals
{
    /// <summary>
    /// This class is abstract and cannot be used directly.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public abstract class CspPluginTypesAttributeBase : HttpHeaderAttributeBase
    {
        private readonly CspPluginTypesOverride _directive;
        private readonly CspConfigurationOverrideHelper _configurationOverrideHelper;
        private readonly HeaderOverrideHelper _headerOverrideHelper;
        private bool _paramsValidated;

        protected CspPluginTypesAttributeBase(params string[] mediaTypes)
        {
            _directive = new CspPluginTypesOverride { Enabled = true, InheritMediaTypes = true };
            _configurationOverrideHelper = new CspConfigurationOverrideHelper();
            _headerOverrideHelper = new HeaderOverrideHelper(new CspReportHelper());

            if (mediaTypes.Length > 0)
            {
                _directive.MediaTypes = mediaTypes;
            }
        }

        internal sealed override string ContextKeyIdentifier => ReportOnly ? "CspReportOnly" : "Csp";

        /// <summary>
        /// Sets whether the plugin-types directive is enabled in the CSP header. The default is true.
        /// </summary>
        public bool Enabled { get => _directive.Enabled; set => _directive.Enabled = value; }

        /// <summary>
        /// Gets or sets whether Media Types should be inherited from previous settings. The default is true.
        /// </summary>
        public bool InheritMediaSources { get => _directive.InheritMediaTypes; set => _directive.InheritMediaTypes = value; }

        /// <summary>
        /// Sets the media types for the directive. Media types are separated by exactly one whitespace.
        /// </summary>
        public string MediaTypes
        {
            get => throw new NotSupportedException();
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw CreateAttributeException("Media types cannot be set to null or an empty string.");

                if (value.StartsWith(" ") || value.EndsWith(" "))
                    throw CreateAttributeException("Media types must not contain leading or trailing whitespace: " + value);

                if (value.Contains("  "))
                    throw CreateAttributeException("Media types must be separated by exactly one whitespace: " + value);

                var mediaTypes = value.Split(' ');
                ValidateMediaTypes(mediaTypes);

                _directive.MediaTypes = mediaTypes;
            }
        }

        protected abstract bool ReportOnly { get; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ValidateParams();

            _configurationOverrideHelper.SetCspPluginTypesOverride(new HttpContextWrapper(filterContext.HttpContext), _directive, ReportOnly);
            base.OnActionExecuting(filterContext);
        }

        public sealed override void SetHttpHeadersOnActionExecuted(ActionExecutedContext filterContext)
        {
            _headerOverrideHelper.SetCspHeaders(new HttpContextWrapper(filterContext.HttpContext), ReportOnly);
        }

        internal void ValidateParams()
        {
            if (_paramsValidated) return;

            if (Enabled && SourcesNotConfigured())
                throw CreateAttributeException("No media types configured for Csp plugin-types attribute. Remove attribute, or set \"Enabled=false\"");

            _paramsValidated = true;
        }

        private void ValidateMediaTypes(string[] mediaTypes)
        {
            if (mediaTypes == null || mediaTypes.Length == 0) return;

            var validator = new Rfc2045MediaTypeValidator();

            foreach (var mediaType in mediaTypes)
            {
                try
                {
                    validator.Validate(mediaType);
                }
                catch (Exception e)
                {
                    throw CreateAttributeException("Invalid media type: " + mediaType, e);
                }
            }
        }

        private bool SourcesNotConfigured()
        {
            return (_directive.MediaTypes == null || _directive.MediaTypes.Length == 0);
        }
    }
}
