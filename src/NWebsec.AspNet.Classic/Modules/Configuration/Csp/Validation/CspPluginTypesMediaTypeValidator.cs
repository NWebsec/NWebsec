// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Configuration;
using NWebsec.Core.Common.HttpHeaders.Configuration.Validation;

namespace NWebsec.Modules.Configuration.Csp.Validation
{
    class CspPluginTypesMediaTypeValidator : ConfigurationValidatorBase
    {
        public override bool CanValidate(Type type)
        {
            return type == typeof(string);
        }

        public override void Validate(object value)
        {
            var mediaType = (string) value;
            
            if ("ConfigDefault".Equals(mediaType)) return;
            
            try
            {
                new Rfc2045MediaTypeValidator().Validate(mediaType);
            }
            catch (Exception e)
            {
                throw new ConfigurationErrorsException("Invalid media type " + value + ". Details: " + e.Message, e);
            }
        }
    }
}
