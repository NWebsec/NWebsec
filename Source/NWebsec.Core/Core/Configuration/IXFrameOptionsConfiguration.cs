// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using NWebsec.HttpHeaders;

namespace NWebsec.Core.Configuration
{
    public interface IXFrameOptionsConfiguration
    {
        XFrameOptionsPolicy Policy { get; set; }
    }
}