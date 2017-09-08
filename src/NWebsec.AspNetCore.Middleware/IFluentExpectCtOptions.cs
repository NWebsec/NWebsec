﻿using NWebsec.AspNetCore.Core.Fluent;
using System;

namespace NWebsec.AspNetCore.Middleware
{
    public interface IFluentExpectCtOptions : IFluentInterface
    {
        /// <summary>
        /// Specifies the max age for the ExpectCT header.
        /// </summary>
        /// <param name="days">The number of days added to max age.</param>
        /// <param name="hours">The number of hours added to max age.</param>
        /// <param name="minutes">The number of minutes added to max age.</param>
        /// <param name="seconds">The number of seconds added to max age.</param>
        /// <returns>The current instance.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if a negative value was supplied in any of the parameters.</exception>
        IFluentExpectCtOptions MaxAge(int days = 0, int hours = 0, int minutes = 0, int seconds = 0);

        /// <summary>
        /// Specifies a report URI where the browser can send ExpectCT reports.
        /// </summary>
        /// <param name="reportUri">The report URI, which is an absolute URI with scheme http or https.</param>
        /// <returns>The current instance.</returns>
        IFluentExpectCtOptions ReportUri(string reportUri);

        /// <summary>
        /// Specifies if the browser should enforce ExpectCT or just report on it.
        /// </summary>
        IFluentExpectCtOptions Enforce();
    }
}
