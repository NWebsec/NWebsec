// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using NUnit.Framework;
using NWebsec.Core.HttpHeaders.Configuration.Validation;

namespace NWebsec.Core.Tests.Unit.Core.HttpHeaders.Configuration.Validation
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
        public void ValidateUri_RelativeUri_NoException()
        {
            var relativeUri = new Uri("/Cspreport", UriKind.Relative);

            Assert.DoesNotThrow(() => _validator.Validate(relativeUri));
        }

        public void ValidateString_RelativeUri_NoException()
        {
            Assert.DoesNotThrow(() => _validator.Validate("/Cspreport"));
        }

        [Test]
        public void ValidateUri_AbsoluteUri_ThrowsException()
        {
            var relativeUri = new Uri("https://www.nwebsec.com/Cspreport");

            Assert.Throws<InvalidCspReportUriException>(() => _validator.Validate(relativeUri));
        }

        [Test]
        public void ValidateString_AbsoluteUri_ThrowsException()
        {
            Assert.Throws<InvalidCspReportUriException>(() => _validator.Validate("https://www.nwebsec.com/Cspreport"));
        }
    }
}
