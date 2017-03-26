// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using NWebsec.AspNetCore.Core.HttpHeaders;
using NWebsec.AspNetCore.Core.HttpHeaders.Configuration;

namespace NWebsec.AspNetCore.Middleware
{
    public class ReferrerPolicyOptions : IReferrerPolicyConfiguration, IFluentReferrerPolicyOptions
    {
        internal ReferrerPolicyOptions()
        {
            Policy = ReferrerPolicy.Disabled;
        }

        public ReferrerPolicy Policy { get; set; }

        public void NoReferrer()
        {
            Policy = ReferrerPolicy.NoReferrer;
        }

        public void NoReferrerWhenDowngrade()
        {
            Policy = ReferrerPolicy.NoReferrerWhenDowngrade;
        }

        public void SameOrigin()
        {
            Policy = ReferrerPolicy.SameOrigin;
        }

        public void Origin()
        {
            Policy = ReferrerPolicy.Origin;
        }

        public void StrictOrigin()
        {
            Policy = ReferrerPolicy.StrictOrigin;
        }

        public void OriginWhenCrossOrigin()
        {
            Policy = ReferrerPolicy.OriginWhenCrossOrigin;
        }

        public void StrictOriginWhenCrossOrigin()
        {
            Policy = ReferrerPolicy.StrictOriginWhenCrossOrigin;
        }

        public void UnsafeUrl()
        {
            Policy = ReferrerPolicy.UnsafeUrl;
        }
    }
}