// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Configuration;

namespace NWebsec.Modules.Configuration.Validation
{
    class XRobotsTagValidator: ConfigurationValidatorBase
    {
        
        public override bool CanValidate(Type type)
        {
            return type == typeof(XRobotsTagConfigurationElement);
        }

        public override void Validate(object value)
        {
            var xRobotsConfig = (XRobotsTagConfigurationElement) value;
            if (!xRobotsConfig.Enabled) return;

            if (xRobotsConfig.NoArchive ||
                xRobotsConfig.NoFollow ||
                xRobotsConfig.NoImageIndex ||
                xRobotsConfig.NoIndex ||
                xRobotsConfig.NoOdp ||
                xRobotsConfig.NoSnippet ||
                xRobotsConfig.NoTranslate) return;
            throw new XRobotsTagException("One or more directives must be enabled when header is enabled. Enable directives or disable header.");
        }
    }

    [Serializable]
    public class XRobotsTagException : ConfigurationErrorsException
    {
        public XRobotsTagException(string s)
            : base(s)
        {
        }
    }
}
