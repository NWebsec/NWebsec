// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using NWebsec.Core.Common.Csp;

namespace NWebsec.AspNetCore.Core.Helpers
{
    public class CspReportHelper : ICspReportHelper
    {
        public string GetBuiltInCspReportHandlerRelativeUri() => "/cspreport";
    }
}