// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using NWebsec.Fluent;

namespace NWebsec.Owin
{
    /// <summary>
    /// Fluent interface to configure options for Http Strict Transport Security.
    /// </summary>
    public interface IFluentHstsOptions : IFluentInterface
    {
        /// <summary>
        /// Specifies the max age for the HSTS header.
        /// </summary>
        /// <param name="days">The number of days added to max age.</param>
        /// <param name="hours">The number of hours added to max age.</param>
        /// <param name="minutes">The number of minutes added to max age.</param>
        /// <param name="seconds">The number of seconds added to max age.</param>
        /// <returns>The current instance.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if a negative value was supplied in any of the parameters.</exception>
        IFluentHstsOptions MaxAge(int days = 0, int hours = 0, int minutes = 0, int seconds = 0);
        
        /// <summary>
        /// Enables the IncludeSubdomains directive in the Hsts header.
        /// </summary>
        /// <returns>The current instance.</returns>
        IFluentHstsOptions IncludeSubdomains();

        /// <summary>
        /// Enables the Preload directive in the HSTS header. MaxAge must be at least 18 weeks, and IncludeSubdomains must be enabled.
        /// </summary>
        /// <remarks>Read more about preloaded HSTS sites at <a href="http://www.chromium.org/sts">www.chromium.org/sts</a></remarks>
        /// <returns>The current instance.</returns>
        IFluentHstsOptions Preload();
    }
}