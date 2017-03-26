// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Collections;
using System.Collections.Generic;
using NWebsec.AspNetCore.Core.HttpHeaders;

namespace NWebsec.AspNetCore.Core.Tests.TestHelpers
{
    public class ReferrerPolicyStringsData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { ReferrerPolicy.NoReferrer, "no-referrer" };
            yield return new object[] { ReferrerPolicy.NoReferrerWhenDowngrade, "no-referrer-when-downgrade" };
            yield return new object[] { ReferrerPolicy.Origin, "origin" };
            yield return new object[] { ReferrerPolicy.OriginWhenCrossOrigin, "origin-when-cross-origin" };
            yield return new object[] { ReferrerPolicy.SameOrigin, "same-origin" };
            yield return new object[] { ReferrerPolicy.StrictOrigin, "strict-origin" };
            yield return new object[] { ReferrerPolicy.StrictOriginWhenCrossOrigin, "strict-origin-when-cross-origin" };
            yield return new object[] { ReferrerPolicy.UnsafeUrl, "unsafe-url" };
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
