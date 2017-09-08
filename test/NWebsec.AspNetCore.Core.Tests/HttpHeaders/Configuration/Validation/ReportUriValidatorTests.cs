using NWebsec.AspNetCore.Core.HttpHeaders.Configuration.Validation;
using System;
using Xunit;

namespace NWebsec.AspNetCore.Core.Tests.HttpHeaders.Configuration.Validation
{
    public class ReportUriValidatorTests
    {
        private readonly ReportUriValidator _validator;

        public ReportUriValidatorTests()
        {
            _validator = new ReportUriValidator();
        }

        [Fact]
        public void ValidateReportUri_AbsoluteUriWithValidScheme_NoException()
        {
            _validator.ValidateReportUri("http://nwebsec.com/report");
            _validator.ValidateReportUri("https://nwebsec.com/report");
        }

        [Fact]
        public void ValidateReportUri_RelativeUri_ThrowsException()
        {
            Assert.Throws<Exception>(() => _validator.ValidateReportUri("/report"));
        }

        [Fact]
        public void ValidateReportUri_WrongScheme_ThrowsException()
        {
            Assert.Throws<Exception>(() => _validator.ValidateReportUri("ftp://nwebsec.com/report"));
        }
    }
}
