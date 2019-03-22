﻿// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Web.Mvc;
using NWebsec.Core.Common.HttpHeaders;
using NWebsec.Core.Common.HttpHeaders.Configuration;
using NWebsec.Core.Web;
using NWebsec.Csp;
using NWebsec.Mvc.Common.Helpers;
using NWebsec.Mvc.HttpHeaders.Internals;

namespace NWebsec.Mvc.HttpHeaders
{
    /// <summary>
    /// Specifies whether the X-Xss-Protection security header should be set in the HTTP response.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class XXssProtectionAttribute : HttpHeaderAttributeBase
    {
        private readonly XXssProtectionConfiguration _config;
        private readonly HeaderConfigurationOverrideHelper _headerConfigurationOverrideHelper;
        private readonly HeaderOverrideHelper _headerOverrideHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="XXssProtectionAttribute"/> class
        /// </summary>
        public XXssProtectionAttribute()
        {
            _config = new XXssProtectionConfiguration { Policy = XXssPolicy.FilterEnabled, BlockMode = true };
            _headerConfigurationOverrideHelper = new HeaderConfigurationOverrideHelper();
            _headerOverrideHelper = new HeaderOverrideHelper(new CspReportHelper());
        }

        /// <summary>
        /// Gets or sets whether the X-Xss-Protection security header should be set in the HTTP response.
        /// Possible values are: Disabled, FilterDisabled, FilterEnabled. The default is FilterEnabled.
        /// </summary>
        public XXssProtectionPolicy Policy
        {
            get => throw new NotSupportedException();
            set
            {
                switch (value)
                {
                    case XXssProtectionPolicy.Disabled:
                        _config.Policy = XXssPolicy.Disabled;
                        break;

                    case XXssProtectionPolicy.FilterDisabled:
                        _config.Policy = XXssPolicy.FilterDisabled;
                        break;

                    case XXssProtectionPolicy.FilterEnabled:
                        _config.Policy = XXssPolicy.FilterEnabled;
                        break;
                    default:
                        throw  new ArgumentException("Unknown value: " + value);
                }
            }
        }

        /// <summary>
        /// Gets or sets whether to enable the IE XSS filter block mode. This setting only takes effect when the Policy is set to FilterEnabled.
        /// The default is true.
        /// </summary>
        public bool BlockMode
        {
            get => _config.BlockMode;
            set => _config.BlockMode = value;
        }

        /// <summary>
        /// Gets or sets report uri for XSS violation report.
        /// </summary>
        public string ReportUri
        {
            get => _config.ReportUri;
            set => _config.ReportUri = value;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            _headerConfigurationOverrideHelper.SetXXssProtectionOverride(new HttpContextWrapper(filterContext.HttpContext), _config);
            base.OnActionExecuting(filterContext);
        }

        public override void SetHttpHeadersOnActionExecuted(ActionExecutedContext filterContext)
        {
            _headerOverrideHelper.SetXXssProtectionHeader(new HttpContextWrapper(filterContext.HttpContext));
        }
    }
}
