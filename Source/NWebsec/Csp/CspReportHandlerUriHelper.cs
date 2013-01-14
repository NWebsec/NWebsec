// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Web;

namespace NWebsec.Csp
{
    public interface ICspReportHandlerPathHelper
    {
        string GetBuiltinCspReportHandlerPath();
    }

    public class CspReportHandlerPathHelper : ICspReportHandlerPathHelper
    {
        public string GetBuiltinCspReportHandlerPath()
        {
            return VirtualPathUtility.ToAbsolute("~/WebResource.axd");
        }
    }
}
