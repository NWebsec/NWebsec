// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using NUnit.Framework;
using NWebsec.Modules.Configuration.Csp.Validation;

namespace NWebsec.Tests.Unit.Modules.Configuration
{
    [TestFixture]
    public class ReportUriValidatorTests
    {
        private ReportUriValidator validator;

        [SetUp]
        public void Setup()
        {
            validator = new ReportUriValidator();
        }

        [Test]
        public void Validate_RelativeUri_NoException()
        {
            var relativeUri = new Uri("/Cspreport", UriKind.Relative);

            Assert.DoesNotThrow(() => validator.Validate(relativeUri));
        }

        [Test]
        public void Validate_AbsoluteUri_ThrowsException()
        {
            var relativeUri = new Uri("https://www.nwebsec.com/Cspreport");

            Assert.Throws<InvalidCspReportUriException>(() => validator.Validate(relativeUri));
        }
    }
}
