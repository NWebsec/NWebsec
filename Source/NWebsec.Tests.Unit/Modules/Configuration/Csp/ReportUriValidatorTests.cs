// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using NUnit.Framework;
using NWebsec.Modules.Configuration.Csp.Validation;

namespace NWebsec.Tests.Unit.Modules.Configuration.Csp
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
        public void Validate_RelativeUri_NoException()
        {
            var relativeUri = new Uri("/Cspreport", UriKind.Relative);

            Assert.DoesNotThrow(() => _validator.Validate(relativeUri));
        }

        [Test]
        public void Validate_AbsoluteUri_ThrowsException()
        {
            var relativeUri = new Uri("https://www.nwebsec.com/Cspreport");

            Assert.Throws<InvalidCspReportUriException>(() => _validator.Validate(relativeUri));
        }
    }
}
