// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using NUnit.Framework;
using NWebsec.Core.HttpHeaders.Configuration.Validation;

namespace NWebsec.Core.Tests.Unit.HttpHeaders.Configuration.Validation
{
    [TestFixture]
    public class ReportUriValidatorTests
    {
        private ReportUriValidator _validator;

        [SetUp]
        public void Setup()
        {
            _validator = new ReportUriValidator();
        }

        [Test]
        public void ValidateString_RelativeUri_NoException()
        {
            Assert.DoesNotThrow(() => _validator.Validate("/Cspreport"));
        }

        [Test]
        public void ValidateString_AbsoluteUri_NoException()
        {
            Assert.DoesNotThrow(() => _validator.Validate("https://www.nwebsec.com/Cspreport"));
        }

        [Test]
        public void ValidateString_MalformedUri_ThrowsException()
        {
            Assert.Throws<InvalidCspReportUriException>(() => _validator.Validate("https://*.nwebsec.com/Cspreport"));
        }
    }
}
