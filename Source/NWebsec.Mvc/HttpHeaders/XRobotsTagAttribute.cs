// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Web.Mvc;
using NWebsec.HttpHeaders;
using NWebsec.Modules.Configuration;

namespace NWebsec.Mvc.HttpHeaders
{
    
    /// <summary>
    /// Specifies whether the X-Download-Options security header should be set in the HTTP response.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class XRobotsTagAttribute : ActionFilterAttribute
    {
        private readonly HttpHeaderHelper _headerHelper;

        /// <summary>
        /// Gets or sets whether the X-Robots-Tag header should be set in the HTTP response. The default is true.
        /// </summary>
        public bool Enabled { get; set; }
        /// <summary>
        /// Gets of sets whether search engines are instructed to not index the page. The default is false.
        /// </summary>
        public bool NoIndex { get; set; }
        /// <summary>
        /// Gets of sets whether search engines are instructed to not follow links on the page. The default is false.
        /// </summary>
        public bool NoFollow { get; set; }
        /// <summary>
        /// Gets of sets whether search engines are instructed to not display a snippet for the page in search results. The default is false.
        /// </summary>
        public bool NoSnippet { get; set; }
        /// <summary>
        /// Gets of sets whether search engines are instructed to not offer a cached version of the page in search results. The default is false.
        /// </summary>
        public bool NoArchive { get; set; }
        /// <summary>
        /// Gets of sets whether search engines are instructed to not use information from the Open Directory Project for the page's title or snippet. The default is false.
        /// </summary>
        public bool NoOdp { get; set; }
        /// <summary>
        /// Gets of sets whether search engines are instructed to not offer translation of the page in search results (Google only). The default is false.
        /// </summary>
        public bool NoTranslate { get; set; }
        /// <summary>
        ///  Gets of sets whether search engines are instructed to not index images on the page (Google only). The default is false.
        /// </summary>
        public bool NoImageIndex { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="XRobotsTagAttribute"/> class
        /// </summary>
        public XRobotsTagAttribute()
        {
            Enabled = true;
            _headerHelper = new HttpHeaderHelper();
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var xRobots = new XRobotsTagConfigurationElement
                              {
                                  Enabled = Enabled,
                                  NoIndex = NoIndex,
                                  NoFollow = NoFollow,
                                  NoSnippet = NoSnippet,
                                  NoArchive = NoArchive,
                                  NoOdp = NoOdp,
                                  NoTranslate = NoTranslate,
                                  NoImageIndex = NoImageIndex
                              };
            _headerHelper.SetXRobotsTagHeaderOverride(filterContext.HttpContext, xRobots);
            base.OnActionExecuting(filterContext);
        }
    }
}
