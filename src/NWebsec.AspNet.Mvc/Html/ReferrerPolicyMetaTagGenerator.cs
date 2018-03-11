// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;

namespace NWebsec.Mvc.Html
{
    public class ReferrerPolicyMetaTagGenerator : IReferrerPolicyClassic
    {
        public ReferrerPolicyTag None => throw new NotImplementedException();
        public ReferrerPolicyTag NoneWhenDowngrade => throw new NotImplementedException();

        public ReferrerPolicyTag NoReferrer { get; } = CreateTag("no-referrer");
        public ReferrerPolicyTag NoReferrerWhenDowngrade { get; } = CreateTag("no-referrer-when-downgrade");
        public ReferrerPolicyTag Origin { get; } = CreateTag("origin");
        public ReferrerPolicyTag OriginWhenCrossOrigin { get; } = CreateTag("origin-when-cross-origin");
        public ReferrerPolicyTag SameOrigin { get; } = CreateTag("same-origin");
        public ReferrerPolicyTag StrictOrigin { get; } = CreateTag("strict-origin");
        public ReferrerPolicyTag StrictOriginWhenCrossOrigin { get; } = CreateTag("strict-origin-when-cross-origin");
        public ReferrerPolicyTag UnsafeUrl { get; } = CreateTag("unsafe-url");

        private static ReferrerPolicyTag CreateTag(string value)
        {
            return new ReferrerPolicyTag($@"<meta name=""referrer"" content=""{value}"" />");
        }
    }
}