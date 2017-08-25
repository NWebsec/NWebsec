// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using NUnit.Framework;
using NWebsec.Mvc.Html;

namespace NWebsec.Mvc.Tests.Unit.Html
{
    [TestFixture]
    public class ReferrerPolicyGeneratorTests
    {

        [Test]
        public void Properties_ReturnsMetaTags()
        {
            var generator = new ReferrerPolicyGenerator();

            Assert.AreEqual(@"<meta name=""referrer"" content="""" />", generator.Empty.Tag.ToString());
            Assert.AreEqual(@"<meta name=""referrer"" content=""no-referrer"" />", generator.NoReferrer.Tag.ToString());
            Assert.AreEqual(@"<meta name=""referrer"" content=""no-referrer-when-downgrade"" />", generator.NoReferrerWhenDowngrade.Tag.ToString());
            Assert.AreEqual(@"<meta name=""referrer"" content=""same-origin"" />", generator.SameOrigin.Tag.ToString());
            Assert.AreEqual(@"<meta name=""referrer"" content=""origin"" />", generator.Origin.Tag.ToString());
            Assert.AreEqual(@"<meta name=""referrer"" content=""strict-origin"" />", generator.StrictOrigin.Tag.ToString());
            Assert.AreEqual(@"<meta name=""referrer"" content=""origin-when-cross-origin"" />", generator.OriginWhenCrossOrigin.Tag.ToString());
            Assert.AreEqual(@"<meta name=""referrer"" content=""unsafe-url"" />", generator.UnsafeUrl.Tag.ToString());
        }
    }
}