// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Configuration;

namespace NWebsec.Modules.Configuration.Validation
{
    [Serializable]
    public class ValidateRedirectConfigurationException : ConfigurationErrorsException
    {
        public ValidateRedirectConfigurationException(string message) : base(message)
        {
            
        }
    }
}