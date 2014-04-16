// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.ComponentModel;
using NWebsec.Core.HttpHeaders.Configuration;

namespace NWebsec.Owin
{
    public class HstsOptionsConfiguration : IHstsConfiguration
    {
        internal HstsOptionsConfiguration()
        {
            MaxAge = TimeSpan.Zero;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public TimeSpan MaxAge { get; set; }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool IncludeSubdomains { get; set; }
    }
}