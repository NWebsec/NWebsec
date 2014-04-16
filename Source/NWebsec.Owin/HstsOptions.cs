// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;

namespace NWebsec.Owin
{
    public class HstsOptions : HstsOptionsConfiguration, IFluentHstsOptions
    {
        public IFluentHstsOptions MaxAge(int days = 0, int hours = 0, int minutes = 0, int seconds = 0)
        {
            base.MaxAge = new TimeSpan(days, hours, minutes, seconds);
            return this;
        }

        public new IFluentHstsOptions IncludeSubdomains()
        {
            base.IncludeSubdomains = true;
            return this;
        }
    }
}