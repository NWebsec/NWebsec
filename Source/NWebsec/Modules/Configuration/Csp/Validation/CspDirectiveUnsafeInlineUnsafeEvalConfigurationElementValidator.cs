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

            if (!element.UnsafeEval) return;

            if (element.None && element.UnsafeEval)
                throw new ConfigurationErrorsException("Both \"None\" and \"UnsafeEval\" are enabled. \"None\" cannot be combined with other sources");
            base.Validate(value);
        }
    }
}
