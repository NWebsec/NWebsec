// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using NWebsec.Core.HttpHeaders.Configuration;
using NWebsec.Core.HttpHeaders.Configuration.Validation;

namespace NWebsec.Owin
{
    public class FluentCspPluginTypesDirective : CspPluginTypesDirectiveConfiguration, IFluentCspPluginTypesDirective
    {
        private static readonly string[] EmptyDirective = new string[0];
        
        public FluentCspPluginTypesDirective()
        {
            Enabled = true;
        }


        public void MediaTypes(params string[] mediaTypes)
        {
            if (mediaTypes == null)
            {
                throw new ArgumentNullException("mediaTypes");
            }

            if (mediaTypes.Length == 0)
            {
                throw new ArgumentException("One or more parameter values expected.","mediaTypes");
            }
            var validator = new Rfc2045MediaTypeValidator();

            foreach (var mediaType in mediaTypes)
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

            base.MediaTypes = mediaTypes;
        }
    }
}