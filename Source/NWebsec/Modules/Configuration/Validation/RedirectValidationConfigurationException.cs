// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Configuration;

namespace NWebsec.Modules.Configuration.Validation
{
    [Serializable]
    public class RedirectValidationConfigurationException : ConfigurationErrorsException
    {
        public RedirectValidationConfigurationException(string message) : base(message)
        {
            
        }
    }
}