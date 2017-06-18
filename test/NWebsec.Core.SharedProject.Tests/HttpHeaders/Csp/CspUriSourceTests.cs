// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using NWebsec.Core.Common.HttpHeaders.Csp;
using Xunit;

namespace NWebsec.Core.SharedProject.Tests.HttpHeaders.Csp
{
    public class CspUriSourceTests
    {

        [Fact]
        public void EncodeUri_RelativePlainUri_ReturnsPlainUri()
        {
            const string expectedUri = "/CspReport";

            var result = CspUriSource.EncodeUri(new Uri(expectedUri, UriKind.Relative));

            Assert.Equal(expectedUri, result);
        }

        [Fact]
        public void EncodeUri_RelativeUriNeedsEncoding_ReturnsEncodedUri()
        {
            const string originalUri = "/CspReport,;";
            const string expectedUri = "/CspReport%2C%3B";

            var result = CspUriSource.EncodeUri(new Uri(originalUri, UriKind.Relative));

            Assert.Equal(expectedUri, result);
        }

        [Fact]
        public void EncodeUri_RelativeUriWithNoneAsciiChars_ReturnsEncodedUri()
        {
            const string originalUri = "/CspReport/André?a=b";
            const string expectedUri = "/CspReport/Andr%C3%A9?a=b";

            var result = CspUriSource.EncodeUri(new Uri(originalUri, UriKind.Relative));

            Assert.Equal(expectedUri, result);
        }

        [Fact]
        public void EncodeUri_AbsolutePlainUri_ReturnsPlainUri()
        {
            const string expectedUri = "https://report.nwebsec.com/CspReport";

            var result = CspUriSource.EncodeUri(new Uri(expectedUri, UriKind.Absolute));

            Assert.Equal(expectedUri, result);
        }

        [Fact]
        public void EncodeUri_AbsoluteUriNeedsEncoding_ReturnsEncodedUri()
        {
            const string originalUri = "https://üüüüüü.de/CspReport,;/andré?a=b";
            const string expectedUri = "https://xn--tdaaaaaa.de/CspReport%2C%3B/andr%C3%A9?a=b";

            var result = CspUriSource.EncodeUri(new Uri(originalUri, UriKind.Absolute));

            Assert.Equal(expectedUri, result);
        }

        [Fact]
        public void Parse_Wildcard_ReturnsResult()
        {
            var result = CspUriSource.Parse("*");

            Assert.Equal("*", result.ToString());
        }

        [Theory]
        [InlineData("https:")]
        [InlineData("data:")]
        public void Parse_SchemeOnly_ReturnsResult(string scheme)
        {
            var result = CspUriSource.Parse(scheme);

            Assert.Equal(scheme, result.ToString());
        }

        [Fact]
        public void Parse_SchemeAndHost_ReturnsResult()
        {
            var result = CspUriSource.Parse("https://www.nwebsec.com");

            Assert.Equal("https://www.nwebsec.com", result.ToString());
        }

        [Fact]
        public void Parse_SchemeAndHostUpperCase_ReturnsLowerCaseResult()
        {
            var result = CspUriSource.Parse("HTTPS://www.NWEBSEC.com");

            Assert.Equal("https://www.nwebsec.com", result.ToString());
        }

        [Fact]
        public void Parse_SchemeAndIdnHost_ReturnsIdnResult()
        {
            var result = CspUriSource.Parse("https://www.üüüüüü.de");

            Assert.Equal("https://www.xn--tdaaaaaa.de", result.ToString());
        }

        [Fact]
        public void Parse_SchemeHostAndPath_ReturnsResult()
        {
            var result = CspUriSource.Parse("https://www.nwebsec.com/some/path");

            Assert.Equal("https://www.nwebsec.com/some/path", result.ToString());
        }

        [Fact]
        public void Parse_SchemeHostAndPathWithSpecialChars_ReturnsEncodedPathResult()
        {
            var result = CspUriSource.Parse("https://www.nwebsec.com/hello;hello,");

            Assert.Equal("https://www.nwebsec.com/hello%3Bhello%2C", result.ToString());
        }

        [Fact]
        public void Parse_SchemeHostAndPathWithNonAsciiChars_ReturnsEncodedPathResult()
        {
            var result = CspUriSource.Parse("https://www.nwebsec.com/André");

            Assert.Equal("https://www.nwebsec.com/Andr%C3%A9", result.ToString());
        }

        [Fact]
        public void Parse_SchemeHostPathAndQueryWithNonAsciiChars_ReturnsEncodedPathResult()
        {
            var result = CspUriSource.Parse("https://www.nwebsec.com/André?a=b");

            Assert.Equal("https://www.nwebsec.com/Andr%C3%A9?a=b", result.ToString());
        }

        [Fact]
        public void Parse_SchemeIdnHostAndPath_ReturnsIdnResult()
        {
            var result = CspUriSource.Parse("https://www.üüüüüü.de/some/path");

            Assert.Equal("https://www.xn--tdaaaaaa.de/some/path", result.ToString());
        }

        [Fact]
        public void Parse_SchemeHostAndPort_ReturnsResult()
        {
            var result = CspUriSource.Parse("https://www.nwebsec.com:8000");

            Assert.Equal("https://www.nwebsec.com:8000", result.ToString());
        }

        [Fact]
        public void Parse_SchemeIdnHostAndPort_ReturnsResult()
        {
            var result = CspUriSource.Parse("https://www.üüüüüü.com:8000");

            Assert.Equal("https://www.xn--tdaaaaaa.com:8000", result.ToString());
        }

        [Fact]
        public void Parse_SchemeHostPortAndPath_ReturnsResult()
        {
            var result = CspUriSource.Parse("https://www.nwebsec.com:8000/some/Path");

            Assert.Equal("https://www.nwebsec.com:8000/some/Path", result.ToString());
        }

        [Fact]
        public void Parse_SchemeIdnHostPortAndPath_ReturnsResult()
        {
            var result = CspUriSource.Parse("https://www.üüüüüü.com:8000/some/path");

            Assert.Equal("https://www.xn--tdaaaaaa.com:8000/some/path", result.ToString());
        }

        [Fact]
        public void Parse_SchemeHostAndWildCardPort_ReturnsResult()
        {
            var result = CspUriSource.Parse("https://www.nwebsec.com:*");

            Assert.Equal("https://www.nwebsec.com:*", result.ToString());
        }

        [Fact]
        public void Parse_SchemeIdnHostAndWildCardPort_ReturnsResult()
        {
            var result = CspUriSource.Parse("https://www.üüüüüü.com:*");

            Assert.Equal("https://www.xn--tdaaaaaa.com:*", result.ToString());
        }

        [Fact]
        public void Parse_SchemeHostWildCardPortAndPath_ReturnsResult()
        {
            var result = CspUriSource.Parse("https://www.nwebsec.com:*/some/path");

            Assert.Equal("https://www.nwebsec.com:*/some/path", result.ToString());
        }

        [Fact]
        public void Parse_SchemeIdnHostWildCardPortAndPath_ReturnsResult()
        {
            var result = CspUriSource.Parse("https://www.üüüüüü.com:*/some/path");

            Assert.Equal("https://www.xn--tdaaaaaa.com:*/some/path", result.ToString());
        }

        [Fact]
        public void Parse_SchemeAndWildcardHost_ReturnsResult()
        {
            var result = CspUriSource.Parse("https://*.nwebsec.com");

            Assert.Equal("https://*.nwebsec.com", result.ToString());
        }

        [Fact]
        public void Parse_SchemeAndWildcardHostWithDash_ReturnsResult()
        {
            var result = CspUriSource.Parse("https://*.www-nwebsec.com");

            Assert.Equal("https://*.www-nwebsec.com", result.ToString());
        }

        [Fact]
        public void Parse_SchemeAndWildcardSubHostWithDash_ReturnsResult()
        {
            var result = CspUriSource.Parse("https://*.w-w.nwebsec.com");

            Assert.Equal("https://*.w-w.nwebsec.com", result.ToString());
        }

        [Fact]
        public void Parse_SchemeAndWildcardIdnHost_ReturnsResult()
        {
            var result = CspUriSource.Parse("https://*.üüüüüü.com");

            Assert.Equal("https://*.xn--tdaaaaaa.com", result.ToString());
        }

        [Fact]
        public void Parse_SchemeWildcardHostAndPath_ReturnsResult()
        {
            var result = CspUriSource.Parse("https://*.nwebsec.com/some/path");

            Assert.Equal("https://*.nwebsec.com/some/path", result.ToString());
        }

        [Fact]
        public void Parse_SchemeWildcardIdnHostAndPath_ReturnsResult()
        {
            var result = CspUriSource.Parse("https://*.üüüüüü.com/some/path");

            Assert.Equal("https://*.xn--tdaaaaaa.com/some/path", result.ToString());
        }

        [Fact]
        public void Parse_SchemeWildcardHostAndPort_ReturnsResult()
        {
            var result = CspUriSource.Parse("https://*.nwebsec.com:8000");

            Assert.Equal("https://*.nwebsec.com:8000", result.ToString());
        }

        [Fact]
        public void Parse_SchemeWildcardIdnHostAndPort_ReturnsResult()
        {
            var result = CspUriSource.Parse("https://*.üüüüüü.com:8000");

            Assert.Equal("https://*.xn--tdaaaaaa.com:8000", result.ToString());
        }

        [Fact]
        public void Parse_SchemeWildcardHostPortAndPath_ReturnsResult()
        {
            var result = CspUriSource.Parse("https://*.nwebsec.com:8000/some/path");

            Assert.Equal("https://*.nwebsec.com:8000/some/path", result.ToString());
        }

        [Fact]
        public void Parse_SchemeWildcardIdnHostPortAndPath_ReturnsResult()
        {
            var result = CspUriSource.Parse("https://*.üüüüüü.com:8000/some/path");

            Assert.Equal("https://*.xn--tdaaaaaa.com:8000/some/path", result.ToString());
        }

        [Fact]
        public void Parse_SchemeWildcardHostAndWildcardPort_ReturnsResult()
        {
            var result = CspUriSource.Parse("https://*.nwebsec.com:*");

            Assert.Equal("https://*.nwebsec.com:*", result.ToString());
        }

        [Fact]
        public void Parse_SchemeWildcardIdnHostAndWildcardPort_ReturnsResult()
        {
            var result = CspUriSource.Parse("https://*.üüüüüü.com:*");

            Assert.Equal("https://*.xn--tdaaaaaa.com:*", result.ToString());
        }

        [Fact]
        public void Parse_SchemeWildcardHostWildcardPortAndPath_ReturnsResult()
        {
            var result = CspUriSource.Parse("https://*.nwebsec.com:*/some/path");

            Assert.Equal("https://*.nwebsec.com:*/some/path", result.ToString());
        }

        [Fact]
        public void Parse_SchemeWildcardIdnHostWildcardPortAndPath_ReturnsResult()
        {
            var result = CspUriSource.Parse("https://*.üüüüüü.com:*/some/path");

            Assert.Equal("https://*.xn--tdaaaaaa.com:*/some/path", result.ToString());
        }

        [Fact]
        public void Parse_SimpleHost_ReturnsResult()
        {
            var result = CspUriSource.Parse("myhost");

            Assert.Equal("myhost", result.ToString());
        }

        [Fact]
        public void Parse_Host_ReturnsResult()
        {
            var result = CspUriSource.Parse("www.demo-nwebsec.com");

            Assert.Equal("www.demo-nwebsec.com", result.ToString());
        }

        [Fact]
        public void Parse_IdnHost_ReturnsResult()
        {
            var result = CspUriSource.Parse("www.üüüüüü.com");

            Assert.Equal("www.xn--tdaaaaaa.com", result.ToString());
        }

        [Fact]
        public void Parse_HostAndPath_ReturnsResult()
        {
            var result = CspUriSource.Parse("www.nwebsec.com/some/path");

            Assert.Equal("www.nwebsec.com/some/path", result.ToString());
        }

        [Fact]
        public void Parse_IdnHostAndPath_ReturnsResult()
        {
            var result = CspUriSource.Parse("www.üüüüüü.com/some/path");

            Assert.Equal("www.xn--tdaaaaaa.com/some/path", result.ToString());
        }

        [Fact]
        public void Parse_HostAndPort_ReturnsResult()
        {
            var result = CspUriSource.Parse("www.nwebsec.com:8000");

            Assert.Equal("www.nwebsec.com:8000", result.ToString());
        }

        [Fact]
        public void Parse_IdnHostAndPort_ReturnsResult()
        {
            var result = CspUriSource.Parse("www.üüüüüü.com:8000");

            Assert.Equal("www.xn--tdaaaaaa.com:8000", result.ToString());
        }

        [Fact]
        public void Parse_HostPortAndPath_ReturnsResult()
        {
            var result = CspUriSource.Parse("www.nwebsec.com:8000/some/path");

            Assert.Equal("www.nwebsec.com:8000/some/path", result.ToString());
        }

        [Fact]
        public void Parse_IdnHostPortAndPath_ReturnsResult()
        {
            var result = CspUriSource.Parse("www.üüüüüü.com:8000/some/path");

            Assert.Equal("www.xn--tdaaaaaa.com:8000/some/path", result.ToString());
        }

        [Fact]
        public void Parse_HostAndWildCardPort_ReturnsResult()
        {
            var result = CspUriSource.Parse("www.nwebsec.com:*");

            Assert.Equal("www.nwebsec.com:*", result.ToString());
        }

        [Fact]
        public void Parse_IdnHostAndWildCardPort_ReturnsResult()
        {
            var result = CspUriSource.Parse("www.üüüüüü.com:*");

            Assert.Equal("www.xn--tdaaaaaa.com:*", result.ToString());
        }

        [Fact]
        public void Parse_HostWildCardPortAndPath_ReturnsResult()
        {
            var result = CspUriSource.Parse("www.nwebsec.com:*/some/path");

            Assert.Equal("www.nwebsec.com:*/some/path", result.ToString());
        }

        [Fact]
        public void Parse_IDNHostWildCardPortAndPath_ReturnsResult()
        {
            var result = CspUriSource.Parse("www.üüüüüü.com:*/some/path");

            Assert.Equal("www.xn--tdaaaaaa.com:*/some/path", result.ToString());
        }

        [Fact]
        public void Parse_WildcardHost_ReturnsResult()
        {
            var result = CspUriSource.Parse("*.nwebsec.com");

            Assert.Equal("*.nwebsec.com", result.ToString());
        }

        [Fact]
        public void Parse_WildcardIdnHost_ReturnsResult()
        {
            var result = CspUriSource.Parse("*.üüüüüü.com");

            Assert.Equal("*.xn--tdaaaaaa.com", result.ToString());
        }

        [Fact]
        public void Parse_WildcardHostAndPath_ReturnsResult()
        {
            var result = CspUriSource.Parse("*.nwebsec.com/some/path");

            Assert.Equal("*.nwebsec.com/some/path", result.ToString());
        }

        [Fact]
        public void Parse_WildcardIdnHostAndPath_ReturnsResult()
        {
            var result = CspUriSource.Parse("*.üüüüüü.com/some/path");

            Assert.Equal("*.xn--tdaaaaaa.com/some/path", result.ToString());
        }

        [Fact]
        public void Parse_WildcardTld_ReturnsResult()
        {
            var result = CspUriSource.Parse("*.com");

            Assert.Equal("*.com", result.ToString());
        }

        [Fact]
        public void Parse_WildcardHostAndPort_ReturnsResult()
        {
            var result = CspUriSource.Parse("*.nwebsec.com:8000");

            Assert.Equal("*.nwebsec.com:8000", result.ToString());
        }

        [Fact]
        public void Parse_WildcardIdnHostAndPort_ReturnsResult()
        {
            var result = CspUriSource.Parse("*.üüüüüü.com:8000");

            Assert.Equal("*.xn--tdaaaaaa.com:8000", result.ToString());
        }

        [Fact]
        public void Parse_WildcardHostPortAndPath_ReturnsResult()
        {
            var result = CspUriSource.Parse("*.nwebsec.com:8000/some/path");

            Assert.Equal("*.nwebsec.com:8000/some/path", result.ToString());
        }

        [Fact]
        public void Parse_WildcardIdnHostPortAndPath_ReturnsResult()
        {
            var result = CspUriSource.Parse("*.üüüüüü.com:8000/some/path");

            Assert.Equal("*.xn--tdaaaaaa.com:8000/some/path", result.ToString());
        }

        [Fact]
        public void Parse_WildcardHostAndWildcardPort_ReturnsResult()
        {
            var result = CspUriSource.Parse("*.nwebsec.com:*");

            Assert.Equal("*.nwebsec.com:*", result.ToString());
        }

        [Fact]
        public void Parse_WildcardIdnHostAndWildcardPort_ReturnsResult()
        {
            var result = CspUriSource.Parse("*.üüüüüü.com:*");

            Assert.Equal("*.xn--tdaaaaaa.com:*", result.ToString());
        }

        [Fact]
        public void Parse_WildcardHostWildcardPortAndPath_ReturnsResult()
        {
            var result = CspUriSource.Parse("*.nwebsec.com:*/some/path");

            Assert.Equal("*.nwebsec.com:*/some/path", result.ToString());
        }

        [Fact]
        public void Parse_WildcardIdnHostWildcardPortAndPath_ReturnsResult()
        {
            var result = CspUriSource.Parse("*.üüüüüü.com:*/some/path");

            Assert.Equal("*.xn--tdaaaaaa.com:*/some/path", result.ToString());
        }

        [Fact]
        public void Parse_InvalidScheme_ThrowsException()
        {

            Assert.Throws<InvalidCspSourceException>(() => CspUriSource.Parse("0https:"));
        }

        [Fact]
        public void Parse_InvalidSchemeHost_ThrowsException()
        {
            Assert.Throws<InvalidCspSourceException>(() => CspUriSource.Parse("0https://www.nwebsec.com"));
        }

        [Fact]
        public void Parse_MissingScheme_ThrowsException()
        {
            Assert.Throws<InvalidCspSourceException>(() => CspUriSource.Parse("//www.nwebsec.*"));
        }

        [Fact]
        public void Parse_WildcardTld_ThrowsException()
        {
            Assert.Throws<InvalidCspSourceException>(() => CspUriSource.Parse("www.nwebsec.*"));
        }

        [Fact]
        public void Parse_WildcardWithinHostname_ThrowsException()
        {
            Assert.Throws<InvalidCspSourceException>(() => CspUriSource.Parse("www.*.com"));
        }

        [Fact]
        public void Parse_SchemeAndWildcardWithinHostname_ThrowsException()
        {
            Assert.Throws<InvalidCspSourceException>(() => CspUriSource.Parse("https://www.*.com"));
        }

        [Fact]
        public void Parse_SchemeHostAndDoublePort_ThrowsException()
        {
            Assert.Throws<InvalidCspSourceException>(() => CspUriSource.Parse("https://www.nwebsec.com:80:80"));
        }

        [Fact]
        public void Parse_SchemeHostAndNegativePortNumber_ThrowsException()
        {
            Assert.Throws<InvalidCspSourceException>(() => CspUriSource.Parse("https://www.nwebsec.com:-80"));
        }

        [Fact]
        public void Parse_SchemeHostAndInvalidPortNumber_ThrowsException()
        {
            Assert.Throws<InvalidCspSourceException>(() => CspUriSource.Parse("https://www.nwebsec.com:65536"));
        }
    }
}
