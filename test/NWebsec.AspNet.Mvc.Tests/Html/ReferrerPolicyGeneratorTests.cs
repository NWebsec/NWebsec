// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using NWebsec.Mvc.Html;
using Xunit;

namespace NWebsec.AspNet.Mvc.Tests.Html
{
    public class ReferrerPolicyGeneratorTests
    {

        [Fact]
        public void Properties_ReturnsMetaTags()
        {
            var generator = new ReferrerPolicyGenerator();

            Assert.Equal(@"<meta name=""referrer"" content=""none"" />", generator.None.Tag.ToString());
            Assert.Equal(@"<meta name=""referrer"" content=""none-when-downgrade"" />", generator.NoneWhenDowngrade.Tag.ToString());
            Assert.Equal(@"<meta name=""referrer"" content=""origin"" />", generator.Origin.Tag.ToString());
            Assert.Equal(@"<meta name=""referrer"" content=""origin-when-crossorigin"" />", generator.OriginWhenCrossOrigin.Tag.ToString());
            Assert.Equal(@"<meta name=""referrer"" content=""unsafe-url"" />", generator.UnsafeUrl.Tag.ToString());
        }
    }
}