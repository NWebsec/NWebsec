// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Web.Mvc;
using NWebsec.Core.Common.HttpHeaders.Configuration;
using NWebsec.Core.Web;
using NWebsec.Csp;
using NWebsec.Mvc.Common.Helpers;
using NWebsec.Mvc.Helpers;
using NWebsec.Mvc.HttpHeaders.Internals;

namespace NWebsec.Mvc.HttpHeaders
{

    /// <summary>
    /// Specifies whether the X-Robots-Tag header should be set in the HTTP response.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class XRobotsTagAttribute : HttpHeaderAttributeBase
    {
        private readonly XRobotsTagConfiguration _config;
        private readonly HeaderConfigurationOverrideHelper _headerConfigurationOverrideHelper;
        private readonly HeaderOverrideHelper _headerOverrideHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="XRobotsTagAttribute"/> class
        /// </summary>
        public XRobotsTagAttribute()
        {
            _config = new XRobotsTagConfiguration { Enabled = true };
            _headerConfigurationOverrideHelper = new HeaderConfigurationOverrideHelper();
            _headerOverrideHelper = new HeaderOverrideHelper(new CspReportHelper());
        }

        /// <summary>
        /// Gets or sets whether the X-Robots-Tag header should be set in the HTTP response. The default is true.
        /// </summary>
        public bool Enabled { get => _config.Enabled; set => _config.Enabled = value; }

        /// <summary>
        /// Gets of sets whether search engines are instructed to not index the page. The default is false.
        /// </summary>
        public bool NoIndex
        {
            get => _config.NoIndex;
            set => _config.NoIndex = value;
        }

        /// <summary>
        /// Gets of sets whether search engines are instructed to not follow links on the page. The default is false.
        /// </summary>
        public bool NoFollow
        {
            get => _config.NoFollow;
            set => _config.NoFollow = value;
        }

        /// <summary>
        /// Gets of sets whether search engines are instructed to not display a snippet for the page in search results. The default is false.
        /// </summary>
        public bool NoSnippet { get => _config.NoSnippet; set => _config.NoSnippet = value; }

        /// <summary>
        /// Gets of sets whether search engines are instructed to not offer a cached version of the page in search results. The default is false.
        /// </summary>
        public bool NoArchive { get => _config.NoArchive; set => _config.NoArchive = value; }

        /// <summary>
        /// Gets of sets whether search engines are instructed to not use information from the Open Directory Project for the page's title or snippet. The default is false.
        /// </summary>
        public bool NoOdp { get => _config.NoOdp; set => _config.NoOdp = value; }

        /// <summary>
        /// Gets of sets whether search engines are instructed to not offer translation of the page in search results (Google only). The default is false.
        /// </summary>
        public bool NoTranslate { get => _config.NoTranslate; set => _config.NoTranslate = value; }

        /// <summary>
        ///  Gets of sets whether search engines are instructed to not index images on the page (Google only). The default is false.
        /// </summary>
        public bool NoImageIndex { get => _config.NoImageIndex; set => _config.NoImageIndex = value; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            _headerConfigurationOverrideHelper.SetXRobotsTagHeaderOverride(new HttpContextWrapper(filterContext.HttpContext), _config);
            base.OnActionExecuting(filterContext);
        }

        public override void SetHttpHeadersOnActionExecuted(ActionExecutedContext filterContext)
        {
            _headerOverrideHelper.SetXRobotsTagHeader(new HttpContextWrapper(filterContext.HttpContext));
        }
    }
}
