// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Configuration;
using System.Linq;

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

            if (! (directive.None || directive.Self || !String.IsNullOrEmpty(directive.Source) || directive.Sources.Count > 0))
                return;

            if (directive.None && (!String.IsNullOrEmpty(directive.Source) || directive.Sources.Count > 0))
            {
                throw new ConfigurationErrorsException("Other sources are not allowed when \"None\" is enabled. Disable \"None\" or remove other sources.");
            }

            if (directive.None && directive.Self)
            {
                throw new ConfigurationErrorsException("Both \"None\" and \"Self\" are enabled. \"None\" cannot be combined with other sources");
            }

            if (!String.IsNullOrEmpty(directive.Source) && directive.Sources.GetAllKeys().Any(x => x.Equals(directive.Source)))
            {
                var ex = string.Format("The source is defined both in the source attribute and in the source list. Please define it only once. Offending source: {0}",
                                       directive.Source);
                throw new ConfigurationErrorsException(ex);
            }

        }

    }
}
