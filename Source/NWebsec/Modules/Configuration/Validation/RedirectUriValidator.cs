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
            var uri = (Uri) value;
            if (!uri.IsAbsoluteUri)
            {
                throw new ValidateRedirectConfigurationException("You must specify an absolute URI. Got: " + uri);    
            }
            if (!uri.PathAndQuery.Equals("/"))
            {
                throw new ValidateRedirectConfigurationException("You must specify an absolute URI without path or query. Got: " + uri);    
            }
            
        }
    }
}