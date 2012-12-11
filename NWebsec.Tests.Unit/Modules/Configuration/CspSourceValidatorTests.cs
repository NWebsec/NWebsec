using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
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
        public void Validate_SchemeHostAndPort_NoException()
        {
            validator.Validate("https://www.nwebsec.com:8000");
        }

        [Test]
        public void Validate_SchemeHostAndWildCardPort_NoException()
        {
            validator.Validate("https://www.nwebsec.com:*");
        }

        [Test]
        public void Validate_SchemeAndWildcardHost_NoException()
        {
            validator.Validate("https://*.nwebsec.com");
        }

        [Test]
        public void Validate_SchemeAndWildcardHostAndPort_NoException()
        {
            validator.Validate("https://*.nwebsec.com:8000");
        }

        [Test]
        public void Validate_SchemeAndWildcardHostAndWildcardPort_NoException()
        {
            validator.Validate("https://*.nwebsec.com:*");
        }

        [Test]
        public void Validate_HostOnly_NoException()
        {
            validator.Validate("www.nwebsec.com");
        }

        [Test]
        public void Validate_AbsoluteUrl_NoException()
        {
            validator.Validate("https://www.nwebsec.com");
        }

        [Test]
        public void Validate_Wildcard_NoException()
        {
            validator.Validate("*");
        }

        [Test]
        public void Validate_WildcardHostname_NoException()
        {
            validator.Validate("*.nwebsec.com");
        }

        [Test]
        public void Validate_SchemeAndWildcardHostname_NoException()
        {
            validator.Validate("https://*.nwebsec.com");
        }

        [Test]
        public void Validate_SchemeAndWildcardHostnameWithPort_NoException()
        {
            validator.Validate("https://*.nwebsec.com:8000");
        }

        [Test]
        public void Validate_SchemeAndWildcardHostnameWithWildcardPort_NoException()
        {
            validator.Validate("https://*.nwebsec.com:*");
        }

        [Test]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void Validate_WildcardTld_ThrowsException()
        {
            validator.Validate("www.nwebsec.*");
        }

        [Test]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void Validate_WildcardWithinHostname_ThrowsException()
        {
            validator.Validate("www.*.com");
        }

        [Test]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void Validate_SchemeAndWildcardWithinHostname_ThrowsException()
        {
            validator.Validate("https://www.*.com");
        }

        
    }
}
