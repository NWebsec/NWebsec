// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;

namespace NWebsec.Core.Configuration
{
    public interface IHstsConfiguration
    {
        TimeSpan MaxAge { get; set; }

        bool IncludeSubdomains { get; set; }

    }
}