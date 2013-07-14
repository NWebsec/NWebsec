// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Configuration;

namespace NWebsec.SessionSecurity.Configuration.Validation
{
    public class SessionIDAuthenticationValidatorAttribute : ConfigurationValidatorAttribute
    {
        public override ConfigurationValidatorBase ValidatorInstance
        {
            get
            {
                return new SessionIDAuthenticationValidator();
            }
        }
    }
}