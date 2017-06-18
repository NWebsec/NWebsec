// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using NWebsec.Csp;
using Xunit;

namespace NWebsec.AspNet.Classic.Tests.Csp
{
    public class CspViolationReportTests
    {

        [Fact]
        public void ToString_ReturnsFormattedString()
        {
            var reportDetails = new CspReportDetails
            {
                BlockedUri = "blockeduri",
                ColumnNumber = "columnnumber",
                DocumentUri = "documenturi",
                EffectiveDirective = "effectivedirective",
                LineNumber = "linenumber",
                OriginalPolicy = "originalpolicy",
                Referrer = "referrer",
                ScriptSample = "scriptsample",
                SourceFile = "sourcefile",
                StatusCode = "statuscode",
                ViolatedDirective = "violateddirective"
            };

            var violationReport = new CspViolationReport
            {
                Details = reportDetails,
                UserAgent = "useragent"
            };

            const string expectedResult =
@"DocumentUri=""documenturi""
EffectiveDirective=""effectivedirective""
ViolatedDirective=""violateddirective""
OriginalPolicy=""originalpolicy""
BlockedUri=""blockeduri""
UserAgent=""useragent""
Referrer=""referrer""
StatusCode=""statuscode""
SourceFile=""sourcefile""
LineNumber=""linenumber""
ColumnNumber=""columnnumber""
ScriptSample=""scriptsample""";

            var result = violationReport.ToString();

            Assert.Equal(expectedResult, result);
        }
    }
}