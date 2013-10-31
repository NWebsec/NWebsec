// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using NUnit.Framework;
using NWebsec.Modules.Configuration.Csp.Validation;

namespace NWebsec.Tests.Unit.Modules.Configuration.Csp
{
    [TestFixture]
    public class CspSourceValidatorTests
    {
        private CspSourceValidator _validator;

        [SetUp]
        public void Setup()
        {
            _validator = new CspSourceValidator();
        }

        [Test]
        public void Validate_Wildcard_NoException()
        {
            Assert.DoesNotThrow(() => _validator.Validate("*"));
        }

        [Test]
        public void Validate_SchemeOnly_NoException()
        {
            Assert.DoesNotThrow(() => _validator.Validate("https:"));
        }
        
        [Test]
        public void Validate_SchemeAndHost_NoException()
        {
            Assert.DoesNotThrow(() => _validator.Validate("https://www.nwebsec.com"));
        }

        [Test]
        public void Validate_SchemeHostAndPath_NoException()
        {
            Assert.DoesNotThrow(() => _validator.Validate("https://www.nwebsec.com/some/path"));
        }

        [Test]
        public void Validate_SchemeHostAndPort_NoException()
        {
            Assert.DoesNotThrow(() => _validator.Validate("https://www.nwebsec.com:8000"));
        }

        [Test]
        public void Validate_SchemeHostPortAndPath_NoException()
        {
            Assert.DoesNotThrow(() => _validator.Validate("https://www.nwebsec.com:8000/some/path"));
        }

        [Test]
        public void Validate_SchemeHostAndWildCardPort_NoException()
        {
            Assert.DoesNotThrow(() => _validator.Validate("https://www.nwebsec.com:*"));
        }

        [Test]
        public void Validate_SchemeHostWildCardPortAndPath_NoException()
        {
            Assert.DoesNotThrow(() => _validator.Validate("https://www.nwebsec.com:*/some/path"));
        }

        [Test]
        public void Validate_SchemeAndWildcardHost_NoException()
        {
            Assert.DoesNotThrow(() => _validator.Validate("https://*.nwebsec.com"));
        }

        [Test]
        public void Validate_SchemeWildcardHostAndPath_NoException()
        {
            Assert.DoesNotThrow(() => _validator.Validate("https://*.nwebsec.com/some/path"));
        }

        [Test]
        public void Validate_SchemeWildcardHostAndPort_NoException()
        {
            Assert.DoesNotThrow(() => _validator.Validate("https://*.nwebsec.com:8000"));
        }

        [Test]
        public void Validate_SchemeWildcardHostPortAndPath_NoException()
        {
            Assert.DoesNotThrow(() => _validator.Validate("https://*.nwebsec.com:8000/some/path"));
        }

        [Test]
        public void Validate_SchemeWildcardHostAndWildcardPort_NoException()
        {
            Assert.DoesNotThrow(() => _validator.Validate("https://*.nwebsec.com:*"));
        }

        [Test]
        public void Validate_SchemeWildcardHostWildcardPortAndPath_NoException()
        {
            Assert.DoesNotThrow(() => _validator.Validate("https://*.nwebsec.com:*/some/path"));
        }

        [Test]
        public void Validate_SimpleHost_NoException()
        {
            Assert.DoesNotThrow(() => _validator.Validate("myhost"));
        }

        [Test]
        public void Validate_Host_NoException()
        {
            Assert.DoesNotThrow(() => _validator.Validate("www.demo-nwebsec.com"));
        }

        [Test]
        public void Validate_HostAndPath_NoException()
        {
            Assert.DoesNotThrow(() => _validator.Validate("www.nwebsec.com/some/path"));
        }

        [Test]
        public void Validate_HostAndPort_NoException()
        {
            Assert.DoesNotThrow(() => _validator.Validate("www.nwebsec.com:8000"));
        }

        [Test]
        public void Validate_HostPortAndPath_NoException()
        {
            Assert.DoesNotThrow(() => _validator.Validate("www.nwebsec.com:8000/some/path"));
        }

        [Test]
        public void Validate_HostAndWildCardPort_NoException()
        {
            Assert.DoesNotThrow(() => _validator.Validate("www.nwebsec.com:*"));
        }

        [Test]
        public void Validate_HostWildCardPortAndPath_NoException()
        {
            Assert.DoesNotThrow(() => _validator.Validate("www.nwebsec.com:*/some/path"));
        }

        [Test]
        public void Validate_WildcardHost_NoException()
        {
            Assert.DoesNotThrow(() => _validator.Validate("*.nwebsec.com"));
        }

        [Test]
        public void Validate_WildcardHostAndPath_NoException()
        {
            Assert.DoesNotThrow(() => _validator.Validate("*.nwebsec.com/some/path"));
        }

        [Test]
        public void Validate_WildcardTld_NoException()
        {
            Assert.DoesNotThrow(() => _validator.Validate("*.com"));
        }

        [Test]
        public void Validate_WildcardHostAndPort_NoException()
        {
            Assert.DoesNotThrow(() => _validator.Validate("*.nwebsec.com:8000"));
        }

        [Test]
        public void Validate_WildcardHostPortAndPath_NoException()
        {
            Assert.DoesNotThrow(() => _validator.Validate("*.nwebsec.com:8000/some/path"));
        }

        [Test]
        public void Validate_WildcardHostAndWildcardPort_NoException()
        {
            Assert.DoesNotThrow(() => _validator.Validate("*.nwebsec.com:*"));
        }

        [Test]
        public void Validate_WildcardHostWildcardPortAndPath_NoException()
        {
            Assert.DoesNotThrow(() => _validator.Validate("*.nwebsec.com:*/some/path"));
        }

        [Test]
        public void Validate_InvalidScheme_ThrowsException()
        {
            Assert.Throws<InvalidCspSourceException>(() => _validator.Validate("0https:"));
        }

        [Test]
        public void Validate_InvalidSchemeHost_ThrowsException()
        {
            Assert.Throws<InvalidCspSourceException>(() => _validator.Validate("0https://www.nwebsec.com"));
        }

        [Test]
        public void Validate_WildcardTld_ThrowsException()
        {
            Assert.Throws<InvalidCspSourceException>(() => _validator.Validate("www.nwebsec.*"));
        }

        [Test]
        public void Validate_WildcardWithinHostname_ThrowsException()
        {
            Assert.Throws<InvalidCspSourceException>(() => _validator.Validate("www.*.com"));
        }

        [Test]
        public void Validate_SchemeAndWildcardWithinHostname_ThrowsException()
        {
            Assert.Throws<InvalidCspSourceException>(() => _validator.Validate("https://www.*.com"));
        }

        [Test]
        public void Validate_SchemeHostAndDoublePort_ThrowsException()
        {
            Assert.Throws<InvalidCspSourceException>(() => _validator.Validate("https://www.nwebsec.com:80:80"));
        }

        [Test]
        public void Validate_SchemeHostAndNegativePortNumber_ThrowsException()
        {
            Assert.Throws<InvalidCspSourceException>(() => _validator.Validate("https://www.nwebsec.com:-80"));
        }

        [Test]
        public void Validate_SchemeHostAndInvalidPortNumber_ThrowsException()
        {
            Assert.Throws<InvalidCspSourceException>(() => _validator.Validate("https://www.nwebsec.com:65536"));
        }
    }
}
