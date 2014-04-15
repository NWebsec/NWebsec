// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using NWebsec.HttpHeaders;

namespace NWebsec.Owin
{
    public static class WithXfo
    {
        public static XFrameOptions Options { get { return new XFrameOptions(); } }

        public static XFrameOptions DenyPolicy()
        {
            return new XFrameOptions { Policy = XFrameOptionsPolicy.Deny };
        }

        public static XFrameOptions SameOriginPolicy()
        {
            return new XFrameOptions { Policy = XFrameOptionsPolicy.SameOrigin };
        }
    }
}