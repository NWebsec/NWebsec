#region License
/*
Copyright (c) 2012, André N. Klingsheim
All rights reserved.

Redistribution and use in source and binary forms, with or without
modification, are permitted provided that the following conditions are met:

* Redistributions of source code must retain the above copyright notice, this
  list of conditions and the following disclaimer.

* Redistributions in binary form must reproduce the above copyright notice,
  this list of conditions and the following disclaimer in the documentation
  and/or other materials provided with the distribution.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.
IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT,
INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING,
BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE
OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED
OF THE POSSIBILITY OF SUCH DAMAGE.
*/
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using NWebsec.Modules.Configuration;
using NWebsec.Modules.Configuration.Csp;

namespace NWebsec.HttpHeaders
{
    public class HttpHeaderHelper
    {
        private HttpContextBase context;

        public HttpHeaderHelper(HttpContextBase context)
        {
            this.context = context;
        }

        internal void FixHeadersFromConfig()
        {
            var headerConfig = GetConfig();
            var headerList = GetHeaderListFromContextItems();
            var headerSetter = new HttpHeaderSetter(context.Response);


            headerSetter.AddXFrameoptionsHeader(
                (XFrameOptionsConfigurationElement)headerList[HttpHeadersConstants.XFrameOptionsHeader]
                ?? headerConfig.SecurityHttpHeaders.XFrameOptions);


            headerSetter.AddHstsHeader(
                (HstsConfigurationElement)headerList[HttpHeadersConstants.StrictTransportSecurityHeader]
                ?? headerConfig.SecurityHttpHeaders.Hsts);


            headerSetter.AddXContentTypeOptionsHeader(
                (SimpleBooleanConfigurationElement)headerList[HttpHeadersConstants.XContentTypeOptionsHeader]
                ?? headerConfig.SecurityHttpHeaders.XContentTypeOptions);


            headerSetter.AddXDownloadOptionsHeader(
                (SimpleBooleanConfigurationElement)headerList[HttpHeadersConstants.XDownloadOptionsHeader]
                ?? headerConfig.SecurityHttpHeaders.XDownloadOptions);


            headerSetter.AddXXssProtectionHeader(
                (XXssProtectionConfigurationElement)headerList[HttpHeadersConstants.XXssProtectionHeader]
                ?? headerConfig.SecurityHttpHeaders.XXssProtection);

            //TODO fix stuff here.
            headerSetter.SuppressVersionHeaders(
                (SuppressVersionHeadersConfigurationElement)headerList["suppressVersionHeaders"]
                ?? headerConfig.suppressVersionHeaders);


            headerSetter.AddXCspHeaders(GetCspElementWithOverrides(false));
            headerSetter.AddXCspHeaders(GetCspElementWithOverrides(true));

        }

        public void SetXFrameoptionsOverride(XFrameOptionsConfigurationElement xFrameOptionsConfig) { }

        public void SetHstsOverride(HstsConfigurationElement hstsConfig) { }

        public void SetXContentTypeOptionsOverride(SimpleBooleanConfigurationElement xContentTypeOptionsConfig) { }

        public void SetXDownloadOptionsOverride(SimpleBooleanConfigurationElement xDownloadOptionsConfig) { }

        public void SetXXssProtectionOverride(XXssProtectionConfigurationElement xXssProtectionConfig) { }

        public void SetXCspOverride(XContentSecurityPolicyConfigurationElement xContentSecurityPolicyConfig) { }

        public void SetXCspReportOnlyOverride(XContentSecurityPolicyConfigurationElement xContentSecurityPolicyReportConfig) { }

        public void SetSuppressVersionHeadersOverride(SuppressVersionHeadersConfigurationElement suppressVersionHeadersConfig) { }

        public void SetContentSecurityPolicyDirectiveOverride(string directive, string sources, bool reportOnly)
        {


            if (sources.StartsWith(" ") || sources.EndsWith(" "))
                throw new ApplicationException("Sources must not contain leading or trailing whitespace: " + sources);
            if (sources.Contains("  "))
                throw new ApplicationException("Sources must be separated by exactly one whitespace: " + sources);

            var sourceList = sources.Split(' ');

            var element = new CspDirectiveConfigurationElement() { Name = directive };

            foreach (var source in sourceList)
            {
                element.Sources.Add(new CspSourceConfigurationElement() { Source = source });
            }


        }

        private IDictionary<String, CspDirectiveConfigurationElement> GetCspDirectiveOverides(bool reportOnly)
        {
            var headerkey = (reportOnly
                                 ? HttpHeadersConstants.XContentSecurityPolicyReportOnlyHeader
                                 : HttpHeadersConstants.XContentSecurityPolicyHeader);

            var headerList = GetHeaderListFromContextItems();

            if (headerList[headerkey] == null)
                headerList[headerkey] = new Dictionary<String, CspDirectiveConfigurationElement>();
            return (IDictionary<String, CspDirectiveConfigurationElement>)headerList[headerkey];
        }
        private XContentSecurityPolicyConfigurationElement GetCspElementWithOverrides(bool reportOnlyElement)
        {
            var config = GetConfig().SecurityHttpHeaders.ExperimentalHeaders;
            var cspConfig = (reportOnlyElement
                                             ? config.XContentSecurityPolicyReportOnly
                                             : config.XContentSecurityPolicy);

            var headerKey = HttpHeadersConstants.XContentSecurityPolicyHeader + (reportOnlyElement ? "ReportOnly" : String.Empty);
            var headerList = GetHeaderListFromContextItems();
            if (headerList[headerKey] == null)
                return cspConfig;

            var overriddenConfig = (XContentSecurityPolicyConfigurationElement)cspConfig.Clone();

            var overrides = (IDictionary<String, CspDirectiveConfigurationElement>)headerList[headerKey];

            foreach (KeyValuePair<String, CspDirectiveConfigurationElement> directive in overrides)
            {
                if (String.IsNullOrEmpty(directive.Value.Source) && directive.Value.Sources.Count == 0)
                {
                    overriddenConfig.Directives.Remove(directive.Value);
                    break;
                }

                if (overriddenConfig.Directives.IndexOf(directive.Value) > 0)
                    overriddenConfig.Directives.Remove(directive.Value);

                overriddenConfig.Directives.Add(directive.Value);
            }

            if (!(overriddenConfig.XContentSecurityPolicyHeader || overriddenConfig.XWebKitCspHeader))
                overriddenConfig.XContentSecurityPolicyHeader = overriddenConfig.XWebKitCspHeader = true;

            return overriddenConfig;
        }

        private IDictionary<string, object> GetHeaderListFromContextItems()
        {
            if (context.Items["nwebsecheaderoverride"] == null)
            {
                context.Items["nwebsecheaderoverride"] = new Dictionary<string, object>();
            }
            return (IDictionary<string, object>)context.Items["nwebsecheaderoverride"];
        }

        internal static HttpHeaderConfigurationSection GetConfig()
        {
            return (HttpHeaderConfigurationSection)(ConfigurationManager.GetSection("nwebsec/httpHeaderModule")) ?? new HttpHeaderConfigurationSection();
        }
    }
}
