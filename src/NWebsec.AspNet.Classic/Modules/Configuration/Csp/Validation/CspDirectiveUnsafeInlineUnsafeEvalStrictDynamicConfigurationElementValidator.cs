// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Configuration;

namespace NWebsec.Modules.Configuration.Csp.Validation
{
    public class CspDirectiveUnsafeInlineUnsafeEvalStrictDynamicConfigurationElementValidator : CspDirectiveUnsafeInlineUnsafeEvalConfigurationElementValidator
    {
        public override bool CanValidate(Type type)
        {
            return type == typeof(CspDirectiveUnsafeInlineUnsafeEvalStrictDynamicConfigurationElement);
        }

        public override void Validate(object value)
        {
            var element = (CspDirectiveUnsafeInlineUnsafeEvalStrictDynamicConfigurationElement)value;

            if (!element.StrictDynamicSrc) return;

            if (element.NoneSrc && element.StrictDynamicSrc)
                throw new ConfigurationErrorsException("Both \"None\" and \"StrictDynamic\" are enabled. \"NoneSrc\" cannot be combined with other sources");
            base.Validate(value);
        }
    }
}
