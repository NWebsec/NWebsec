// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using NUnit.Framework;

namespace NWebsec.Mvc.Tests.Unit.Helpers
{
    [TestFixture]
    public class HttpHeaderConfigurationHelperCspReportOnlyTest : HttpHeaderConfigurationHelperCspTestBase
    {
        protected override bool ReportOnly
        {
            get { return true; }
        }
    }
}