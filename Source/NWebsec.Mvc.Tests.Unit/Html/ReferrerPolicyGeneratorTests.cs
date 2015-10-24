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

            Assert.AreEqual(@"<meta name=""referrer"" content=""none"" />", generator.None.Tag.ToString());
            Assert.AreEqual(@"<meta name=""referrer"" content=""none-when-downgrade"" />", generator.NoneWhenDowngrade.Tag.ToString());
            Assert.AreEqual(@"<meta name=""referrer"" content=""origin"" />", generator.Origin.Tag.ToString());
            Assert.AreEqual(@"<meta name=""referrer"" content=""origin-when-crossorigin"" />", generator.OriginWhenCrossOrigin.Tag.ToString());
            Assert.AreEqual(@"<meta name=""referrer"" content=""unsafe-url"" />", generator.UnsafeUrl.Tag.ToString());
        }
    }
}