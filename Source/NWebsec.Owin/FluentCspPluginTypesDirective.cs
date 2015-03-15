// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using NWebsec.Core.HttpHeaders.Configuration;

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
            //TODO Unit tests and validation.
            if (mediaTypes == null)
            {
                throw new ArgumentNullException("mediaTypes");
            }

            if (mediaTypes.Length == 0)
            {
                throw new ArgumentException("One or more parameter values expected.","mediaTypes");
            }

            base.MediaTypes = mediaTypes ?? EmptyDirective;
        }
    }
}