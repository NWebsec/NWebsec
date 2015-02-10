// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using NUnit.Framework;
using NWebsec.Core.HttpHeaders.Csp;

namespace NWebsec.Core.Tests.Unit.HttpHeaders.Csp
{
    [TestFixture]
    public class CspUriSourceTests
    {
        
        [Test]
        public void Parse_Wildcard_ReturnsResult()
        {
            var result = CspUriSource.Parse("*");

            Assert.AreEqual("*", result.ToString());
        }

        [Test]
        public void Parse_SchemeOnly_ReturnsResult()
        {
            var result = CspUriSource.Parse("https:");

            Assert.AreEqual("https:", result.ToString());
        }

        [Test]
        public void Parse_SchemeAndHost_ReturnsResult()
        {
            var result = CspUriSource.Parse("https://www.nwebsec.com");

            Assert.AreEqual("https://www.nwebsec.com", result.ToString());
        }

        [Test]
        public void Parse_SchemeAndHostUpperCase_ReturnsLowerCaseResult()
        {
            var result = CspUriSource.Parse("HTTPS://www.NWEBSEC.com");

            Assert.AreEqual("https://www.nwebsec.com", result.ToString());
        }

        [Test]
        public void Parse_SchemeAndIdnHost_ReturnsIdnResult()
        {
            var result = CspUriSource.Parse("https://www.üüüüüü.de");

            Assert.AreEqual("https://www.xn--tdaaaaaa.de", result.ToString());
        }

        [Test]
        public void Parse_SchemeHostAndPath_ReturnsResult()
        {
            var result = CspUriSource.Parse("https://www.nwebsec.com/some/path");

            Assert.AreEqual("https://www.nwebsec.com/some/path", result.ToString());
        }

        [Test]
        public void Parse_SchemeHostAndPathWithSpecialChars_ReturnsEncodedPathResult()
        {
            var result = CspUriSource.Parse("https://www.nwebsec.com/hello;hello,");

            Assert.AreEqual("https://www.nwebsec.com/hello%3Bhello%2C", result.ToString()); 
        }

        [Test]
        public void Parse_SchemeIdnHostAndPath_ReturnsIdnResult()
        {
            var result = CspUriSource.Parse("https://www.üüüüüü.de/some/path");

            Assert.AreEqual("https://www.xn--tdaaaaaa.de/some/path", result.ToString());
        }

        [Test]
        public void Parse_SchemeHostAndPort_ReturnsResult()
        {
            var result = CspUriSource.Parse("https://www.nwebsec.com:8000");

            Assert.AreEqual("https://www.nwebsec.com:8000", result.ToString());
        }

        [Test]
        public void Parse_SchemeIdnHostAndPort_ReturnsResult()
        {
            var result = CspUriSource.Parse("https://www.üüüüüü.com:8000");

            Assert.AreEqual("https://www.xn--tdaaaaaa.com:8000", result.ToString());
        }

        [Test]
        public void Parse_SchemeHostPortAndPath_ReturnsResult()
        {
            var result = CspUriSource.Parse("https://www.nwebsec.com:8000/some/Path");

            Assert.AreEqual("https://www.nwebsec.com:8000/some/Path", result.ToString());
        }

        [Test]
        public void Parse_SchemeIdnHostPortAndPath_ReturnsResult()
        {
            var result = CspUriSource.Parse("https://www.üüüüüü.com:8000/some/path");

            Assert.AreEqual("https://www.xn--tdaaaaaa.com:8000/some/path", result.ToString());
        }

        [Test]
        public void Parse_SchemeHostAndWildCardPort_ReturnsResult()
        {
            var result = CspUriSource.Parse("https://www.nwebsec.com:*");

            Assert.AreEqual("https://www.nwebsec.com:*", result.ToString());
        }

        [Test]
        public void Parse_SchemeIdnHostAndWildCardPort_ReturnsResult()
        {
            var result = CspUriSource.Parse("https://www.üüüüüü.com:*");

            Assert.AreEqual("https://www.xn--tdaaaaaa.com:*", result.ToString());
        }

        [Test]
        public void Parse_SchemeHostWildCardPortAndPath_ReturnsResult()
        {
            var result = CspUriSource.Parse("https://www.nwebsec.com:*/some/path");

            Assert.AreEqual("https://www.nwebsec.com:*/some/path", result.ToString());
        }

        [Test]
        public void Parse_SchemeIdnHostWildCardPortAndPath_ReturnsResult()
        {
            var result = CspUriSource.Parse("https://www.üüüüüü.com:*/some/path");

            Assert.AreEqual("https://www.xn--tdaaaaaa.com:*/some/path", result.ToString());
        }

        [Test]
        public void Parse_SchemeAndWildcardHost_ReturnsResult()
        {
            var result = CspUriSource.Parse("https://*.nwebsec.com");

            Assert.AreEqual("https://*.nwebsec.com", result.ToString());
        }

        [Test]
        public void Parse_SchemeAndWildcardIdnHost_ReturnsResult()
        {
            var result = CspUriSource.Parse("https://*.üüüüüü.com");

            Assert.AreEqual("https://*.xn--tdaaaaaa.com", result.ToString());
        }

        [Test]
        public void Parse_SchemeWildcardHostAndPath_ReturnsResult()
        {
            var result = CspUriSource.Parse("https://*.nwebsec.com/some/path");

            Assert.AreEqual("https://*.nwebsec.com/some/path", result.ToString());
        }

        [Test]
        public void Parse_SchemeWildcardIdnHostAndPath_ReturnsResult()
        {
            var result = CspUriSource.Parse("https://*.üüüüüü.com/some/path");

            Assert.AreEqual("https://*.xn--tdaaaaaa.com/some/path", result.ToString());
        }

        [Test]
        public void Parse_SchemeWildcardHostAndPort_ReturnsResult()
        {
            var result = CspUriSource.Parse("https://*.nwebsec.com:8000");

            Assert.AreEqual("https://*.nwebsec.com:8000", result.ToString());
        }

        [Test]
        public void Parse_SchemeWildcardIdnHostAndPort_ReturnsResult()
        {
            var result = CspUriSource.Parse("https://*.üüüüüü.com:8000");

            Assert.AreEqual("https://*.xn--tdaaaaaa.com:8000", result.ToString());
        }

        [Test]
        public void Parse_SchemeWildcardHostPortAndPath_ReturnsResult()
        {
            var result = CspUriSource.Parse("https://*.nwebsec.com:8000/some/path");

            Assert.AreEqual("https://*.nwebsec.com:8000/some/path", result.ToString());
        }

        [Test]
        public void Parse_SchemeWildcardIdnHostPortAndPath_ReturnsResult()
        {
            var result = CspUriSource.Parse("https://*.üüüüüü.com:8000/some/path");

            Assert.AreEqual("https://*.xn--tdaaaaaa.com:8000/some/path", result.ToString());
        }

        [Test]
        public void Parse_SchemeWildcardHostAndWildcardPort_ReturnsResult()
        {
            var result = CspUriSource.Parse("https://*.nwebsec.com:*");

            Assert.AreEqual("https://*.nwebsec.com:*", result.ToString());
        }

        [Test]
        public void Parse_SchemeWildcardIdnHostAndWildcardPort_ReturnsResult()
        {
            var result = CspUriSource.Parse("https://*.üüüüüü.com:*");

            Assert.AreEqual("https://*.xn--tdaaaaaa.com:*", result.ToString());
        }

        [Test]
        public void Parse_SchemeWildcardHostWildcardPortAndPath_ReturnsResult()
        {
            var result = CspUriSource.Parse("https://*.nwebsec.com:*/some/path");

            Assert.AreEqual("https://*.nwebsec.com:*/some/path", result.ToString());
        }

        [Test]
        public void Parse_SchemeWildcardIdnHostWildcardPortAndPath_ReturnsResult()
        {
            var result = CspUriSource.Parse("https://*.üüüüüü.com:*/some/path");

            Assert.AreEqual("https://*.xn--tdaaaaaa.com:*/some/path", result.ToString());
        }

        [Test]
        public void Parse_SimpleHost_ReturnsResult()
        {
            var result = CspUriSource.Parse("myhost");

            Assert.AreEqual("myhost", result.ToString());
        }

        [Test]
        public void Parse_Host_ReturnsResult()
        {
            var result = CspUriSource.Parse("www.demo-nwebsec.com");

            Assert.AreEqual("www.demo-nwebsec.com", result.ToString());
        }

        [Test]
        public void Parse_IdnHost_ReturnsResult()
        {
            var result = CspUriSource.Parse("www.üüüüüü.com");

            Assert.AreEqual("www.xn--tdaaaaaa.com", result.ToString());
        }

        [Test]
        public void Parse_HostAndPath_ReturnsResult()
        {
            var result = CspUriSource.Parse("www.nwebsec.com/some/path");

            Assert.AreEqual("www.nwebsec.com/some/path", result.ToString());
        }

        [Test]
        public void Parse_IdnHostAndPath_ReturnsResult()
        {
            var result = CspUriSource.Parse("www.üüüüüü.com/some/path");

            Assert.AreEqual("www.xn--tdaaaaaa.com/some/path", result.ToString());
        }

        [Test]
        public void Parse_HostAndPort_ReturnsResult()
        {
            var result = CspUriSource.Parse("www.nwebsec.com:8000");

            Assert.AreEqual("www.nwebsec.com:8000", result.ToString());
        }

        [Test]
        public void Parse_IdnHostAndPort_ReturnsResult()
        {
            var result = CspUriSource.Parse("www.üüüüüü.com:8000");

            Assert.AreEqual("www.xn--tdaaaaaa.com:8000", result.ToString());
        }

        [Test]
        public void Parse_HostPortAndPath_ReturnsResult()
        {
            var result = CspUriSource.Parse("www.nwebsec.com:8000/some/path");

            Assert.AreEqual("www.nwebsec.com:8000/some/path", result.ToString());
        }

        [Test]
        public void Parse_IdnHostPortAndPath_ReturnsResult()
        {
            var result = CspUriSource.Parse("www.üüüüüü.com:8000/some/path");

            Assert.AreEqual("www.xn--tdaaaaaa.com:8000/some/path", result.ToString());
        }

        [Test]
        public void Parse_HostAndWildCardPort_ReturnsResult()
        {
            var result = CspUriSource.Parse("www.nwebsec.com:*");

            Assert.AreEqual("www.nwebsec.com:*", result.ToString());
        }

        [Test]
        public void Parse_IdnHostAndWildCardPort_ReturnsResult()
        {
            var result = CspUriSource.Parse("www.üüüüüü.com:*");

            Assert.AreEqual("www.xn--tdaaaaaa.com:*", result.ToString());
        }

        [Test]
        public void Parse_HostWildCardPortAndPath_ReturnsResult()
        {
            var result = CspUriSource.Parse("www.nwebsec.com:*/some/path");

            Assert.AreEqual("www.nwebsec.com:*/some/path", result.ToString());
        }

        [Test]
        public void Parse_IDNHostWildCardPortAndPath_ReturnsResult()
        {
            var result = CspUriSource.Parse("www.üüüüüü.com:*/some/path");

            Assert.AreEqual("www.xn--tdaaaaaa.com:*/some/path", result.ToString());
        }

        [Test]
        public void Parse_WildcardHost_ReturnsResult()
        {
            var result = CspUriSource.Parse("*.nwebsec.com");

            Assert.AreEqual("*.nwebsec.com", result.ToString());
        }

        [Test]
        public void Parse_WildcardIdnHost_ReturnsResult()
        {
            var result = CspUriSource.Parse("*.üüüüüü.com");

            Assert.AreEqual("*.xn--tdaaaaaa.com", result.ToString());
        }

        [Test]
        public void Parse_WildcardHostAndPath_ReturnsResult()
        {
            var result = CspUriSource.Parse("*.nwebsec.com/some/path");

            Assert.AreEqual("*.nwebsec.com/some/path", result.ToString());
        }

        [Test]
        public void Parse_WildcardIdnHostAndPath_ReturnsResult()
        {
            var result = CspUriSource.Parse("*.üüüüüü.com/some/path");

            Assert.AreEqual("*.xn--tdaaaaaa.com/some/path", result.ToString());
        }

        [Test]
        public void Parse_WildcardTld_ReturnsResult()
        {
            var result = CspUriSource.Parse("*.com");

            Assert.AreEqual("*.com", result.ToString());
        }

        [Test]
        public void Parse_WildcardHostAndPort_ReturnsResult()
        {
            var result = CspUriSource.Parse("*.nwebsec.com:8000");

            Assert.AreEqual("*.nwebsec.com:8000", result.ToString());
        }

        [Test]
        public void Parse_WildcardIdnHostAndPort_ReturnsResult()
        {
            var result = CspUriSource.Parse("*.üüüüüü.com:8000");

            Assert.AreEqual("*.xn--tdaaaaaa.com:8000", result.ToString());
        }

        [Test]
        public void Parse_WildcardHostPortAndPath_ReturnsResult()
        {
            var result = CspUriSource.Parse("*.nwebsec.com:8000/some/path");

            Assert.AreEqual("*.nwebsec.com:8000/some/path", result.ToString());
        }

        [Test]
        public void Parse_WildcardIdnHostPortAndPath_ReturnsResult()
        {
            var result = CspUriSource.Parse("*.üüüüüü.com:8000/some/path");

            Assert.AreEqual("*.xn--tdaaaaaa.com:8000/some/path", result.ToString());
        }

        [Test]
        public void Parse_WildcardHostAndWildcardPort_ReturnsResult()
        {
            var result = CspUriSource.Parse("*.nwebsec.com:*");

            Assert.AreEqual("*.nwebsec.com:*", result.ToString());
        }

        [Test]
        public void Parse_WildcardIdnHostAndWildcardPort_ReturnsResult()
        {
            var result = CspUriSource.Parse("*.üüüüüü.com:*");

            Assert.AreEqual("*.xn--tdaaaaaa.com:*", result.ToString());
        }

        [Test]
        public void Parse_WildcardHostWildcardPortAndPath_ReturnsResult()
        {
            var result = CspUriSource.Parse("*.nwebsec.com:*/some/path");

            Assert.AreEqual("*.nwebsec.com:*/some/path", result.ToString());
        }

        [Test]
        public void Parse_WildcardIdnHostWildcardPortAndPath_ReturnsResult()
        {
            var result = CspUriSource.Parse("*.üüüüüü.com:*/some/path");

            Assert.AreEqual("*.xn--tdaaaaaa.com:*/some/path", result.ToString());
        }
        
        [Test]
        public void Parse_InvalidScheme_ThrowsException()
        {

            Assert.Throws<InvalidCspSourceException>(() => CspUriSource.Parse("0https:"));
        }

        [Test]
        public void Parse_InvalidSchemeHost_ThrowsException()
        {
            Assert.Throws<InvalidCspSourceException>(() => CspUriSource.Parse("0https://www.nwebsec.com"));
        }

        [Test]
        public void Parse_MissingScheme_ThrowsException()
        {
            Assert.Throws<InvalidCspSourceException>(() => CspUriSource.Parse("//www.nwebsec.*"));
        }

        [Test]
        public void Parse_WildcardTld_ThrowsException()
        {
            Assert.Throws<InvalidCspSourceException>(() => CspUriSource.Parse("www.nwebsec.*"));
        }

        [Test]
        public void Parse_WildcardWithinHostname_ThrowsException()
        {
            Assert.Throws<InvalidCspSourceException>(() => CspUriSource.Parse("www.*.com"));
        }

        [Test]
        public void Parse_SchemeAndWildcardWithinHostname_ThrowsException()
        {
            Assert.Throws<InvalidCspSourceException>(() => CspUriSource.Parse("https://www.*.com"));
        }

        [Test]
        public void Parse_SchemeHostAndDoublePort_ThrowsException()
        {
            Assert.Throws<InvalidCspSourceException>(() => CspUriSource.Parse("https://www.nwebsec.com:80:80"));
        }

        [Test]
        public void Parse_SchemeHostAndNegativePortNumber_ThrowsException()
        {
            Assert.Throws<InvalidCspSourceException>(() => CspUriSource.Parse("https://www.nwebsec.com:-80"));
        }

        [Test]
        public void Parse_SchemeHostAndInvalidPortNumber_ThrowsException()
        {
            Assert.Throws<InvalidCspSourceException>(() => CspUriSource.Parse("https://www.nwebsec.com:65536"));
        }
    }
}
