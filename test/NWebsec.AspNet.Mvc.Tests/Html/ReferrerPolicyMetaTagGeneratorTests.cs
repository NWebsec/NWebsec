// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using NWebsec.Mvc.Html;
using Xunit;

namespace NWebsec.AspNet.Mvc.Tests.Html
{
    public class ReferrerPolicyMetaTagGeneratorTests
    {

        [Fact]
        public void Properties_ReturnsMetaTags()
        {
            var generator = new ReferrerPolicyMetaTagGenerator();

            Assert.Throws<NotImplementedException>(()=> generator.None);
            Assert.Throws<NotImplementedException>(()=> generator.NoneWhenDowngrade);


            Assert.Equal(@"<meta name=""referrer"" content=""no-referrer"" />", generator.NoReferrer.Tag.ToString());
            Assert.Equal(@"<meta name=""referrer"" content=""no-referrer-when-downgrade"" />", generator.NoReferrerWhenDowngrade.Tag.ToString());
            Assert.Equal(@"<meta name=""referrer"" content=""origin"" />", generator.Origin.Tag.ToString());
            Assert.Equal(@"<meta name=""referrer"" content=""origin-when-cross-origin"" />", generator.OriginWhenCrossOrigin.Tag.ToString());
            Assert.Equal(@"<meta name=""referrer"" content=""same-origin"" />", generator.SameOrigin.Tag.ToString());
            Assert.Equal(@"<meta name=""referrer"" content=""strict-origin"" />", generator.StrictOrigin.Tag.ToString());
            Assert.Equal(@"<meta name=""referrer"" content=""strict-origin-when-cross-origin"" />", generator.StrictOriginWhenCrossOrigin.Tag.ToString());
            Assert.Equal(@"<meta name=""referrer"" content=""unsafe-url"" />", generator.UnsafeUrl.Tag.ToString());
        }
    }
}