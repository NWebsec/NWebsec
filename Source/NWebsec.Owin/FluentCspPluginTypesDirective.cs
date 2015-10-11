// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Linq;
using NWebsec.Core.HttpHeaders.Configuration;
using NWebsec.Core.HttpHeaders.Configuration.Validation;

namespace NWebsec.Owin
{
    public class FluentCspPluginTypesDirective : CspPluginTypesDirectiveConfiguration, IFluentCspPluginTypesDirective
    {
        public FluentCspPluginTypesDirective()
        {
            Enabled = true;
        }


        public void MediaTypes(params string[] mediaTypes)
        {
            if (mediaTypes == null)
            {
                throw new ArgumentNullException(nameof(mediaTypes));
            }

            if (mediaTypes.Length == 0)
            {
                throw new ArgumentException("One or more parameter values expected.", nameof(mediaTypes));
            }
            var validator = new Rfc2045MediaTypeValidator();
            var types = mediaTypes.Distinct().ToArray();

            foreach (var mediaType in types)
            {
                try
                {
                    validator.Validate(mediaType);
                }
                catch (Exception e)
                {
                    throw new ArgumentException("Invalid argument. Details: " + e.Message, e);
                }
            }

            base.MediaTypes = types;
        }
    }
}