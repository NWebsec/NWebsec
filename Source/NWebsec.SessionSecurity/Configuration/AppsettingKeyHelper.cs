// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Collections.Specialized;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Web.Configuration;
using NWebsec.SessionSecurity.Configuration.Validation;

namespace NWebsec.SessionSecurity.Configuration
{
    internal class AppsettingKeyHelper : IAppsettingKeyHelper
    {
        private readonly NameValueCollection _config;
        private ISessionAuthenticationKeyValidator _validator;

        internal AppsettingKeyHelper()
        {
            _validator = new SessionAuthenticationKeyValidator();
        }

        internal AppsettingKeyHelper(NameValueCollection config, ISessionAuthenticationKeyValidator validator)
        {
            _config = config;
            _validator = validator;
        }

        public byte[] GetKeyFromAppsetting(string name)
        {
            var cfg = _config ?? GetConfig();

            var values = cfg.GetValues(name);

            if (values == null)
            {
                throw new ApplicationException("Could not get session authentication key. The appsetting " + name +" did not exist.");
            }

            if (values.Length > 1)
            {
                throw new ApplicationException("Multiple values found for the appsetting " + name + ". Please configure only one.");
            }

            var validationKey = values[0];

            string failureReason;
            if (!_validator.IsValidKey(validationKey, out failureReason))
            {
                throw new ApplicationException("Invalid session authentication key in appsetting. " + failureReason);
            }

            var hexbinary = SoapHexBinary.Parse(validationKey);
            var key = hexbinary.Value;
            hexbinary.Value = new byte[0];
            return key;
        }

        private NameValueCollection GetConfig()
        {
            return WebConfigurationManager.AppSettings;
        }
    }
}