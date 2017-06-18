// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using NUnit.Framework;
using NWebsec.Core.Common.HttpHeaders.Configuration;

namespace NWebsec.Owin.Tests.Unit
{
    [TestFixture]
    public class HstsOptionsTest
    {
        private HstsOptions _options;

        [SetUp]
        public void Setup()
        {
            _options = new HstsOptions();
        }

        [Test]
        public void Ctor_HttpsOnly_DefaultTrue()
        {
            Assert.IsTrue(((IHstsConfiguration)_options).HttpsOnly);
        }

        [Test]
        public void MaxAge_ValidMaxage_SetsMaxage()
        {
            _options.MaxAge(minutes: 30);
            
            Assert.IsTrue(new TimeSpan(0,30,0) == ((IHstsConfiguration)_options).MaxAge);
        }

        [Test]
        public void MaxAge_ZeroMaxage_SetsMaxage()
        {
            _options.MaxAge();

            Assert.IsTrue(TimeSpan.Zero == ((IHstsConfiguration)_options).MaxAge);
        }

        [Test]
        public void MaxAge_NegativeValues_ThrowsException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _options.MaxAge(0, 0, 0, -1));
            Assert.Throws<ArgumentOutOfRangeException>(() => _options.MaxAge(0, 0, -1));
            Assert.Throws<ArgumentOutOfRangeException>(() => _options.MaxAge(0, -1));
            Assert.Throws<ArgumentOutOfRangeException>(() => _options.MaxAge(-1));
        }

        [Test]
        public void IncludeSubdomains_SetsIncludeSubdomains()
        {
            _options.IncludeSubdomains();

            Assert.IsTrue(((IHstsConfiguration)_options).IncludeSubdomains);
        }

        [Test]
        public void AllResponses_DisablesHttpsOnly()
        {
            _options.AllResponses();

            Assert.IsFalse(((IHstsConfiguration)_options).HttpsOnly);
        }

        [Test]
        public void HttpsOnly_SetsHttpsOnly()
        {
            _options.HttpsOnly();

            Assert.IsTrue(((IHstsConfiguration)_options).HttpsOnly);
        }

        [Test]
        public void Preload_SetsPreload()
        {
            _options.Preload();

            Assert.IsTrue(((IHstsConfiguration)_options).Preload);
        }

        [Test]
        public void UpgradeInsecureRequests_SetsUpgradeInsecureRequests()
        {
            _options.UpgradeInsecureRequests();

            Assert.IsTrue(((IHstsConfiguration)_options).UpgradeInsecureRequests);
        }
    }
}