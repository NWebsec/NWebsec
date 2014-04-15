// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using NWebsec.Core.HttpHeaders.Configuration;
using NWebsec.Owin.Fluent;

namespace NWebsec.Owin
{

    //public interface IHstsOptions : IFluentInterface
    //{

    //    HstsOptions WithMaxAge(int days = 0, int hours = 0, int minutes = 0, int seconds = 0);
    //    HstsOptions IncludeSubdomains();
    //}
    public interface IHstsOptionsWithMaxAge { }

    public class HstsOptions : IHstsConfiguration, IFluentInterface, IHstsOptionsWithMaxAge
    {
        /// <summary>
        /// Creates a new instance. Use the fluent API to configure object.
        /// </summary>
        public HstsOptions()
        {

        }
        public TimeSpan MaxAge { get; set; }
        public bool IncludeSubdomains { get; set; }


        public HstsOptionsWithMaxage SetMaxAge(int days = 0, int hours = 0, int minutes = 0, int seconds = 0)
        {
            var maxAge = new TimeSpan(days, hours, minutes, seconds);
            return new HstsOptionsWithMaxage(maxAge);
        }
    }

    public class HstsOptionsWithMaxage : HstsOptions
    {
        internal HstsOptionsWithMaxage(TimeSpan maxAge)
        {
            MaxAge = maxAge;
        }

        public new HstsOptionsWithMaxage IncludeSubdomains()
        {
            base.IncludeSubdomains = true;
            return this;
        }

    }
}