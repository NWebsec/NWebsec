// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using NWebsec.Core.HttpHeaders.Configuration;
using NWebsec.HttpHeaders;

namespace NWebsec.Owin
{
    public class XFrameOptions : IXFrameOptionsConfiguration
    {
        public XFrameOptionsPolicy Policy { get; set; }
    }
}