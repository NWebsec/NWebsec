// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using NWebsec.Annotations;
using NWebsec.AspNetCore.Core.HttpHeaders;

namespace NWebsec.AspNetCore.Core.Extensions
{
    public static class ReferrerPolicyExtensions
    {
        [NotNull]
        public static string GetPolicyString(this ReferrerPolicy policy)
        {
            switch (policy)
            {
                case ReferrerPolicy.Disabled:
                    throw new ArgumentOutOfRangeException(nameof(policy), "Disabled is not an actual referrer policy.");
                case ReferrerPolicy.NoReferrer:
                    return "no-referrer";
                case ReferrerPolicy.NoReferrerWhenDowngrade:
                    return "no-referrer-when-downgrade";
                case ReferrerPolicy.SameOrigin:
                    return "same-origin";
                case ReferrerPolicy.Origin:
                    return "origin";
                case ReferrerPolicy.StrictOrigin:
                    return "strict-origin";
                case ReferrerPolicy.OriginWhenCrossOrigin:
                    return "origin-when-cross-origin";
                case ReferrerPolicy.StrictOriginWhenCrossOrigin:
                    return "strict-origin-when-cross-origin";
                case ReferrerPolicy.UnsafeUrl:
                    return "unsafe-url";
                default:
                    throw new ArgumentOutOfRangeException(nameof(policy), $"Unexpected value: {policy}");
            }
        }
    }
}