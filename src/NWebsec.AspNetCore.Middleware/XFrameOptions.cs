// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using NWebsec.AspNetCore.Core.HttpHeaders;
using NWebsec.AspNetCore.Core.HttpHeaders.Configuration;

namespace NWebsec.Middleware
{
    public class XFrameOptions : IXFrameOptionsConfiguration, IFluentXFrameOptions
    {
        internal XFrameOptions()
        {
            Policy = XfoPolicy.Disabled;
        }

        public XfoPolicy Policy { get; set; }

        public void Deny()
        {
            Policy = XfoPolicy.Deny;
        }

        public void SameOrigin()
        {
            Policy = XfoPolicy.SameOrigin;
        }
    }
}