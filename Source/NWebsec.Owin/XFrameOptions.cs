// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.ComponentModel;
using NWebsec.Core.HttpHeaders.Configuration;
using NWebsec.HttpHeaders;
using NWebsec.Owin.Fluent;

namespace NWebsec.Owin
{
    public class XFrameOptions : IXFrameOptionsConfiguration, IFluentInterface
    {
        internal XFrameOptions()
        {
            Policy = XFrameOptionsPolicy.Disabled;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
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