// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Configuration;
using NUnit.Framework;
using NWebsec.Modules.Configuration.Csp.Validation;

namespace NWebsec.Tests.Unit.Modules.Configuration
{
    [TestFixture]
    public class CspSourceValidatorTests
    {
        private CspSourceValidator validator;

        [SetUp]
        public void Setup()
        {
            validator = new CspSourceValidator();
        }

        [Test]
        public void Validate_Wildcard_NoException()
        {
            validator.Validate("*");
        }

        [Test]
        public void Validate_SchemeOnly_NoException()
        {
            validator.Validate("https:");
        }
        
        [Test]
        public void Validate_SchemeAndHost_NoException()
        {
            validator.Validate("https://www.nwebsec.com");
        }

        [Test]
        public void Validate_SchemeHostAndPath_NoException()
        {
            validator.Validate("https://www.nwebsec.com/some/path");
        }

        [Test]
        public void Validate_SchemeHostAndPort_NoException()
        {
            validator.Validate("https://www.nwebsec.com:8000");
        }

        [Test]
        public void Validate_SchemeHostPortAndPath_NoException()
        {
            validator.Validate("https://www.nwebsec.com:8000/some/path");
        }

        [Test]
        public void Validate_SchemeHostAndWildCardPort_NoException()
        {
            validator.Validate("https://www.nwebsec.com:*");
        }

        [Test]
        public void Validate_SchemeHostWildCardPortAndPath_NoException()
        {
            validator.Validate("https://www.nwebsec.com:*/some/path");
        }

        [Test]
        public void Validate_SchemeAndWildcardHost_NoException()
        {
            validator.Validate("https://*.nwebsec.com");
        }

        [Test]
        public void Validate_SchemeWildcardHostAndPath_NoException()
        {
            validator.Validate("https://*.nwebsec.com/some/path");
        }

        [Test]
        public void Validate_SchemeWildcardHostAndPort_NoException()
        {
            validator.Validate("https://*.nwebsec.com:8000");
        }

        [Test]
        public void Validate_SchemeWildcardHostPortAndPath_NoException()
        {
            validator.Validate("https://*.nwebsec.com:8000/some/path");
        }

        [Test]
        public void Validate_SchemeWildcardHostAndWildcardPort_NoException()
        {
            validator.Validate("https://*.nwebsec.com:*");
        }

        [Test]
        public void Validate_SchemeWildcardHostWildcardPortAndPath_NoException()
        {
            validator.Validate("https://*.nwebsec.com:*/some/path");
        }

        [Test]
        public void Validate_SimpleHost_NoException()
        {
            validator.Validate("myhost");
        }

        [Test]
        public void Validate_Host_NoException()
        {
            validator.Validate("www.demo-nwebsec.com");
        }

        [Test]
        public void Validate_HostAndPath_NoException()
        {
            validator.Validate("www.nwebsec.com/some/path");
        }

        [Test]
        public void Validate_HostAndPort_NoException()
        {
            validator.Validate("www.nwebsec.com:8000");
        }

        [Test]
        public void Validate_HostPortAndPath_NoException()
        {
            validator.Validate("www.nwebsec.com:8000/some/path");
        }

        [Test]
        public void Validate_HostAndWildCardPort_NoException()
        {
            validator.Validate("www.nwebsec.com:*");
        }

        [Test]
        public void Validate_HostWildCardPortAndPath_NoException()
        {
            validator.Validate("www.nwebsec.com:*/some/path");
        }

        [Test]
        public void Validate_WildcardHost_NoException()
        {
            validator.Validate("*.nwebsec.com");
        }

        [Test]
        public void Validate_WildcardHostAndPath_NoException()
        {
            validator.Validate("*.nwebsec.com/some/path");
        }

        [Test]
        public void Validate_WildcardTld_NoException()
        {
            validator.Validate("*.com");
        }

        [Test]
        public void Validate_WildcardHostAndPort_NoException()
        {
            validator.Validate("*.nwebsec.com:8000");
        }

        [Test]
        public void Validate_WildcardHostPortAndPath_NoException()
        {
            validator.Validate("*.nwebsec.com:8000/some/path");
        }

        [Test]
        public void Validate_WildcardHostAndWildcardPort_NoException()
        {
            validator.Validate("*.nwebsec.com:*");
        }

        [Test]
        public void Validate_WildcardHostWildcardPortAndPath_NoException()
        {
            validator.Validate("*.nwebsec.com:*/some/path");
        }

        [Test]
        [ExpectedException(typeof(InvalidCspSourceException))]
        public void Validate_InvalidScheme_ThrowsException()
        {
            validator.Validate("0https:");
        }

        [Test]
        [ExpectedException(typeof(InvalidCspSourceException))]
        public void Validate_InvalidSchemeHost_ThrowsException()
        {
            validator.Validate("0https://www.nwebsec.com");
        }

        [Test]
        [ExpectedException(typeof(InvalidCspSourceException))]
        public void Validate_WildcardTld_ThrowsException()
        {
            validator.Validate("www.nwebsec.*");
        }

        [Test]
        [ExpectedException(typeof(InvalidCspSourceException))]
        public void Validate_WildcardWithinHostname_ThrowsException()
        {
            validator.Validate("www.*.com");
        }

        [Test]
        [ExpectedException(typeof(InvalidCspSourceException))]
        public void Validate_SchemeAndWildcardWithinHostname_ThrowsException()
        {
            validator.Validate("https://www.*.com");
        }

        [Test]
        [ExpectedException(typeof(InvalidCspSourceException))]
        public void Validate_SchemeHostAndDoublePort_ThrowsException()
        {
            validator.Validate("https://www.nwebsec.com:80:80");
        }
    }
}
