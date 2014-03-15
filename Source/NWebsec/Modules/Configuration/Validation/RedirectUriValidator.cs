// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Configuration;

namespace NWebsec.Modules.Configuration.Validation
{
    public class RedirectUriValidator : ConfigurationValidatorBase
    {

        public override bool CanValidate(Type type)
        {
            return type == typeof(Uri);
        }

        public override void Validate(object value)
        {
            var uri = (Uri)value;
            if (!uri.IsAbsoluteUri)
            {
                throw new RedirectValidationConfigurationException("You must specify an absolute URI. Got: " + uri);
            }

            if (!String.IsNullOrEmpty(uri.Query))
            {
                throw new RedirectValidationConfigurationException("You must specify an absolute URI without a query string. Got: " + uri);
            }
        }
    }
}