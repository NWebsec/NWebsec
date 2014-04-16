// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using NWebsec.Core.HttpHeaders.Configuration;
using NWebsec.HttpHeaders;

namespace NWebsec.Owin
{
    public class XFrameOptions : IXFrameOptionsConfiguration, IFluentXFrameOptions
    {
        internal XFrameOptions()
        {
            Policy = XFrameOptionsPolicy.Disabled;
        }

        public XFrameOptionsPolicy Policy { get; set; }

        public void Deny()
        {
            Policy = XFrameOptionsPolicy.Deny;
        }

        public void SameOrigin()
        {
            Policy = XFrameOptionsPolicy.SameOrigin;
        }
    }
}