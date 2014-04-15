// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.ComponentModel;
using NWebsec.Core.HttpHeaders.Configuration;
using NWebsec.Owin.Fluent;

namespace NWebsec.Owin
{
    public class HstsOptionsConfiguration : IHstsConfiguration, IFluentInterface
    {
        public HstsOptionsConfiguration()
        {
            MaxAge = TimeSpan.Zero;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public TimeSpan MaxAge { get; set; }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool IncludeSubdomains { get; set; }
    }

    public class HstsOptions : HstsOptionsConfiguration
    {
        public HstsOptions MaxAge(int days = 0, int hours = 0, int minutes = 0, int seconds = 0)
        {
            base.MaxAge = new TimeSpan(days, hours, minutes, seconds);
            return this;
        }

        public new HstsOptions IncludeSubdomains()
        {
            base.IncludeSubdomains = true;
            return this;
        }
    }
}