using NWebsec.AspNetCore.Core.HttpHeaders.Configuration.Validation;
using System;

namespace NWebsec.AspNetCore.Middleware
{
    public class ExpectCtOptions :ExpectCtOptionsConfiguration, IFluentExpectCtOptions
    {
        private readonly ReportUriValidator _validator;

        public ExpectCtOptions()
        {
            _validator = new ReportUriValidator();
        }

        public new IFluentExpectCtOptions MaxAge(int days = 0, int hours = 0, int minutes = 0, int seconds = 0)
        {
            if (days < 0) throw new ArgumentOutOfRangeException(nameof(days), "Value must be equal to or larger than 0.");
            if (hours < 0) throw new ArgumentOutOfRangeException(nameof(hours), "Value must be equal to or larger than 0.");
            if (minutes < 0) throw new ArgumentOutOfRangeException(nameof(minutes), "Value must be equal to or larger than 0.");
            if (seconds < 0) throw new ArgumentOutOfRangeException(nameof(seconds), "Value must be equal to or larger than 0.");

            base.MaxAge = new TimeSpan(days, hours, minutes, seconds);
            return this;
        }

        public new IFluentExpectCtOptions Enforce()
        {
            base.Enforce = true;
            return this;
        }

        public new IFluentExpectCtOptions ReportUri(string reportUri)
        {
            try
            {
                _validator.ValidateReportUri(reportUri);
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message, nameof(reportUri));
            }

            base.ReportUri = reportUri;
            return this;
        }
    }
}
