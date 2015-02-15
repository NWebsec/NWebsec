// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Configuration;

namespace NWebsec.Modules.Configuration.Csp.Validation
{
    public class CspDirectiveBaseConfigurationElementValidator : ConfigurationValidatorBase
    {
        public override bool CanValidate(Type type)
        {
            return type == typeof(CspDirectiveBaseConfigurationElement);
        }

        public override void Validate(object value)
        {
            var directive = (CspDirectiveBaseConfigurationElement)value;

            if (!(directive.NoneSrc || directive.SelfSrc || directive.Sources.Count > 0))
                return;

            if (directive.NoneSrc && directive.Sources.Count > 0)
            {
                throw new ConfigurationErrorsException("Other sources are not allowed when \"None\" is enabled. Disable \"None\" or remove other sources.");
            }

            if (directive.NoneSrc && directive.SelfSrc)
            {
                throw new ConfigurationErrorsException("Both \"None\" and \"Self\" are enabled. \"None\" cannot be combined with other sources");
            }
        }
    }
}
