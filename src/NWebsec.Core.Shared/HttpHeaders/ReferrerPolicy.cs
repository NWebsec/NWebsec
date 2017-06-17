// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

namespace NWebsec.AspNetCore.Core.HttpHeaders
{
    public enum ReferrerPolicy
    {
        Disabled,
        NoReferrer,
        NoReferrerWhenDowngrade,
        SameOrigin,
        Origin,
        StrictOrigin,
        OriginWhenCrossOrigin,
        StrictOriginWhenCrossOrigin,
        UnsafeUrl
    }
}