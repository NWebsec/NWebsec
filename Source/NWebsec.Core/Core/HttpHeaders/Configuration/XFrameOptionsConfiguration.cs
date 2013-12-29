// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using NWebsec.HttpHeaders;

namespace NWebsec.Core.HttpHeaders.Configuration
{
    public class XFrameOptionsConfiguration : IXFrameOptionsConfiguration
    {
        public XFrameOptionsPolicy Policy { get; set; }
    }
}