// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Configuration;

namespace NWebsec.Modules.Configuration.Csp.Validation
{
    public class CspDirectiveBaseConfigurationElementValidator : ConfigurationValidatorBase
    {
        public override bool CanValidate(Type type)
        {
            return type == typeof(CspDirectiveBaseConfigurationElement<CspSourceConfigurationElement>);
        }

        public override void Validate(object value)
        {
            var element = (CspDirectiveBaseConfigurationElement<CspSourceConfigurationElement>)value;
            ValidateElement(element);
        }

        internal virtual void ValidateElement<T>(CspDirectiveBaseConfigurationElement<T> element) where T : CspSourceConfigurationElement, new()
        {

            if (element.NoneSrc && element.Sources.Count > 0)
            {
                throw new ConfigurationErrorsException("Other sources are not allowed when \"None\" is enabled. Disable \"None\" or remove other sources.");
            }

            if (element.NoneSrc && element.SelfSrc)
            {
                throw new ConfigurationErrorsException("Both \"None\" and \"Self\" are enabled. \"None\" cannot be combined with other sources");
            }
        }
    }
}
