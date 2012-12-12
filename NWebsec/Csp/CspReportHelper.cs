// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Runtime.Serialization.Json;
using System.Web;

namespace NWebsec.Csp
{
    internal class CspReportHelper
    {
        private readonly ICspReportHandlerPathHelper pathHelper;

        internal CspReportHelper()
        {
            pathHelper = new CspReportHandlerPathHelper();
        }

        internal CspReportHelper(ICspReportHandlerPathHelper pathHelper)
        {
            this.pathHelper = pathHelper;
        }

        internal string GetBuiltInCspReportHandlerRelativeUri()
        {
            var uri = new UriBuilder {Path = pathHelper.GetBuiltinCspReportHandlerPath(), Query = "cspReport=true"};
            return uri.Uri.ToString();
        }

        internal bool IsRequestForBuiltInCspReportHandler(HttpRequestBase request)
        {
            return request.Path.Equals(pathHelper.GetBuiltinCspReportHandlerPath())
                   && request.QueryString["cspReport"] != null
                   && request.QueryString["cspReport"].Equals("true");

        }

        internal CspViolationReport GetCspReportFromRequest(HttpRequestBase request)
        {
            
            var serializer = new DataContractJsonSerializer(typeof(CspViolationReport));
            var report = (CspViolationReport)serializer.ReadObject(request.InputStream);
            report.UserAgent = request.UserAgent;
            return report;
        }
    }
}
