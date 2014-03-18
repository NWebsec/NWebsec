// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Runtime.Serialization.Json;
using System.Web;

namespace NWebsec.Csp
{
    public class CspReportHelper : ICspReportHelper
    {
        private readonly ICspReportHandlerPathHelper _pathHelper;

        public CspReportHelper()
        {
            _pathHelper = new CspReportHandlerPathHelper();
        }

        internal CspReportHelper(ICspReportHandlerPathHelper pathHelper)
        {
            _pathHelper = pathHelper;
        }

        public string GetBuiltInCspReportHandlerRelativeUri()
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

        internal bool TryGetCspReportFromRequest(HttpRequestBase request, out CspViolationReport violationReport)
        {
            violationReport = null;
            var serializer = new DataContractJsonSerializer(typeof(CspViolationReport));
            try
            {
                violationReport = (CspViolationReport) serializer.ReadObject(request.InputStream);
                violationReport.UserAgent = request.UserAgent;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
