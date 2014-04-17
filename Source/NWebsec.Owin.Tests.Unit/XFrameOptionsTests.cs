// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using NUnit.Framework;
using NWebsec.HttpHeaders;

namespace NWebsec.Owin.Tests.Unit
{
    [TestFixture]
    public class XFrameOptionsTests
    {
        private XFrameOptions _options;

        [SetUp]
        public void Setup()
        {
            _options = new XFrameOptions();
        }

        [Test]
        public void Deny_SetsDeny()
        {
            _options.Deny();

            Assert.AreEqual(XFrameOptionsPolicy.Deny, _options.Policy);
        }

        [Test]
        public void SameOrigin_SetsSameOrigin()
        {
            _options.SameOrigin();

            Assert.AreEqual(XFrameOptionsPolicy.SameOrigin, _options.Policy);
        }
    }
}