// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;

namespace NWebsec.Core.HttpHeaders.Configuration.Validation
{
    public class HstsConfigurationValidator
    {
        public void Validate(IHstsConfiguration hstsConfig)
        {
            if (!hstsConfig.Preload) return;

            if (hstsConfig.MaxAge.TotalSeconds < 10886400 || !hstsConfig.IncludeSubdomains)
            {
                throw new Exception("HSTS max age must be at least 18 weeks and includesubdomains must be enabled to use the preload directive.");
            }
        } 
    }
}