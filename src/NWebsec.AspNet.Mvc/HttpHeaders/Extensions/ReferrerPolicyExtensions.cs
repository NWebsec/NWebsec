// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using ReferrerPolicyCore = NWebsec.Core.Common.HttpHeaders.ReferrerPolicy;

namespace NWebsec.Mvc.HttpHeaders.Extensions
{
    public static class ReferrerPolicyExtensions
    {

        public static ReferrerPolicyCore MapToCoreType(this ReferrerPolicy policy)
        {
            var result = (ReferrerPolicyCore) policy;
            if (!Enum.IsDefined(typeof(ReferrerPolicyCore), result))
            {
                throw new ArgumentOutOfRangeException(nameof(policy), $"The enum value {policy} was undefined in core enum type.");
            }

            return result;
        }
    }
}