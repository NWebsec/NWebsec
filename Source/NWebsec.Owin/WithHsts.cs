// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;

namespace NWebsec.Owin
{
    public static class WithHsts
    {
        public static HstsOptionsWithMaxage MaxAge(int days = 0, int hours = 0, int minutes = 0, int seconds = 0)
        {
            var maxAge = new TimeSpan(days, hours, minutes, seconds);
            return new HstsOptionsWithMaxage(maxAge);
        }
    }
}