// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Configuration;

namespace NWebsec.Modules.Configuration.Csp.Validation
{
    public class CspDirectiveUnsafeInlineUnsafeEvalConfigurationElementValidator : CspDirectiveUnsafeInlineConfigurationElementValidator
    {
        public override bool CanValidate(Type type)
        {
            return type == typeof(CspDirectiveUnsafeInlineUnsafeEvalConfigurationElement);
        }

        public override void Validate(object value)
        {
            var element = (CspDirectiveUnsafeInlineUnsafeEvalConfigurationElement)value;
            ValidateElement(element);
        }

        internal override void ValidateElement<T>(CspDirectiveBaseConfigurationElement<T> element)
        {
            if (element.NoneSrc && element.UnsafeEvalSrc)
            {
                throw new ConfigurationErrorsException("Both \"None\" and \"UnsafeEval\" are enabled. \"NoneSrc\" cannot be combined with other sources");
            }

            if (element.NoneSrc && element.StrictDynamicSrc)
            {
                throw new ConfigurationErrorsException("Both \"None\" and \"StrictDynamic\" are enabled. \"NoneSrc\" cannot be combined with other sources");
            }

            base.ValidateElement(element);
        }
    }
}
