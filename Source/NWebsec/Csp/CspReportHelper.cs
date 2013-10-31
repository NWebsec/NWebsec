// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Runtime.Serialization.Json;
using System.Web;

namespace NWebsec.Csp
{
    internal class CspReportHelper
    {
        private readonly ICspReportHandlerPathHelper _pathHelper;

        internal CspReportHelper()
        {
            _pathHelper = new CspReportHandlerPathHelper();
        }

        internal CspReportHelper(ICspReportHandlerPathHelper pathHelper)
        {
            _pathHelper = pathHelper;
        }

        internal string GetBuiltInCspReportHandlerRelativeUri()
        {
            var uri = new UriBuilder {Path = _pathHelper.GetBuiltinCspReportHandlerPath(), Query = "cspReport=true"};
            return uri.Uri.PathAndQuery;
        }

        internal bool IsRequestForBuiltInCspReportHandler(HttpRequestBase request)
        {
            return request.Path.Equals(_pathHelper.GetBuiltinCspReportHandlerPath())
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
