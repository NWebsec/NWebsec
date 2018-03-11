// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Configuration;
using NWebsec.Core.Common.HttpHeaders;
using NWebsec.Core.Common.HttpHeaders.Configuration;

namespace NWebsec.Modules.Configuration
{
    public class ReferrerPolicyConfigurationElement : ConfigurationElement, IReferrerPolicyConfiguration
    {
        [ConfigurationProperty("enabled", IsRequired = true, DefaultValue = false)]
        public bool Enabled
        {
            get => (bool)this["enabled"];
            set => this["enabled"] = value;
        }

        [ConfigurationProperty("policy", IsRequired = true)]
        public string ConfigPolicy
        {

            get => (string)this["policy"];
            set => this["policy"] = value;
        }

        public ReferrerPolicy Policy
        {
            get => Enabled ? GetPolicy() : ReferrerPolicy.Disabled;
            set => throw new NotImplementedException();
        }

        private ReferrerPolicy GetPolicy()
        {
            switch (ConfigPolicy)
            {
                case "no-referrer":
                    return ReferrerPolicy.NoReferrer;
                case "no-referrer-when-downgrade":
                    return ReferrerPolicy.NoReferrerWhenDowngrade;
                case "origin":
                    return ReferrerPolicy.Origin;
                case "origin-when-cross-origin":
                    return ReferrerPolicy.OriginWhenCrossOrigin;
                case "same-origin":
                    return ReferrerPolicy.SameOrigin;
                case "strict-origin":
                    return ReferrerPolicy.StrictOrigin;
                case "strict-origin-when-cross-origin":
                    return ReferrerPolicy.StrictOriginWhenCrossOrigin;
                case "unsafe-url":
                    return ReferrerPolicy.UnsafeUrl;
                default:
                    throw new ArgumentException("No one should ever see this.");
            }
        }
    }
}