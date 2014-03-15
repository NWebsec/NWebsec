// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Configuration;
using NWebsec.Core.HttpHeaders.Configuration.Validation;

namespace NWebsec.Modules.Configuration.Validation
{
    class XRobotsTagValidator : ConfigurationValidatorBase
    {

        public override bool CanValidate(Type type)
        {
            return type == typeof(XRobotsTagConfigurationElement);
        }

        public override void Validate(object value)
        {
            var xRobotsConfig = (XRobotsTagConfigurationElement)value;

            var validator = new XRobotsTagConfigurationValidator();

            try
            {
                validator.Validate(xRobotsConfig);
            }
            catch(Exception e)
            {
                throw new ConfigurationErrorsException("xRobotsTag configuration error.", e);
            }
        }
    }
}
