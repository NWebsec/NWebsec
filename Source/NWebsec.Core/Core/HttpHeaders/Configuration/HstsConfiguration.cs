// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;

namespace NWebsec.Core.HttpHeaders.Configuration
{
    public class HstsConfiguration : IHstsConfiguration
    {
        public TimeSpan MaxAge { get; set; }
        public bool IncludeSubdomains { get; set; }
    }
}