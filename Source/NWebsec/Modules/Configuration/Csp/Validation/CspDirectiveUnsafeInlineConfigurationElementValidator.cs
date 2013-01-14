// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Configuration;

namespace NWebsec.Modules.Configuration.Csp.Validation
{
    public class CspDirectiveUnsafeInlineConfigurationElementValidator : CspDirectiveBaseConfigurationElementValidator
    {
        public override bool CanValidate(Type type)
        {
            return type == typeof(CspDirectiveUnsafeInlineConfigurationElement);
        }

        public override void Validate(object value)
        {
            var element = (CspDirectiveUnsafeInlineConfigurationElement)value;

            if (!element.UnsafeInline) return;

            if (element.None && element.UnsafeInline)
                throw new ConfigurationErrorsException("Both \"None\" and \"UnsafeInline\" are enabled. \"None\" cannot be combined with other sources");
            base.Validate(value);
        }
    }
}
