// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using NUnit.Framework;
using NWebsec.AspNetCore.Core.HttpHeaders;

namespace NWebsec.Middleware.Tests
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

            Assert.AreEqual(XfoPolicy.Deny, _options.Policy);
        }

        [Test]
        public void SameOrigin_SetsSameOrigin()
        {
            _options.SameOrigin();

            Assert.AreEqual(XfoPolicy.SameOrigin, _options.Policy);
        }
    }
}