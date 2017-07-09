// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using NWebsec.Core.Common.HttpHeaders.Configuration;
using NWebsec.Owin;
using Xunit;

namespace NWebsec.AspNet.Owin.Tests
{
    public class HstsOptionsTest
    {
        private readonly HstsOptions _options;

        public HstsOptionsTest()
        {
            _options = new HstsOptions();
        }

        [Fact]
        public void Ctor_HttpsOnly_DefaultTrue()
        {
            Assert.True(((IHstsConfiguration)_options).HttpsOnly);
        }

        [Fact]
        public void MaxAge_ValidMaxage_SetsMaxage()
        {
            _options.MaxAge(minutes: 30);

            Assert.True(new TimeSpan(0, 30, 0) == ((IHstsConfiguration)_options).MaxAge);
        }

        [Fact]
        public void MaxAge_ZeroMaxage_SetsMaxage()
        {
            _options.MaxAge();

            Assert.True(TimeSpan.Zero == ((IHstsConfiguration)_options).MaxAge);
        }

        [Fact]
        public void MaxAge_NegativeValues_ThrowsException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _options.MaxAge(0, 0, 0, -1));
            Assert.Throws<ArgumentOutOfRangeException>(() => _options.MaxAge(0, 0, -1));
            Assert.Throws<ArgumentOutOfRangeException>(() => _options.MaxAge(0, -1));
            Assert.Throws<ArgumentOutOfRangeException>(() => _options.MaxAge(-1));
        }

        [Fact]
        public void IncludeSubdomains_SetsIncludeSubdomains()
        {
            _options.IncludeSubdomains();

            Assert.True(((IHstsConfiguration)_options).IncludeSubdomains);
        }

        [Fact]
        public void AllResponses_DisablesHttpsOnly()
        {
            _options.AllResponses();

            Assert.False(((IHstsConfiguration)_options).HttpsOnly);
        }

        [Fact]
        public void HttpsOnly_SetsHttpsOnly()
        {
            _options.HttpsOnly();

            Assert.True(((IHstsConfiguration)_options).HttpsOnly);
        }

        [Fact]
        public void Preload_SetsPreload()
        {
            _options.Preload();

            Assert.True(((IHstsConfiguration)_options).Preload);
        }

        [Fact]
        public void UpgradeInsecureRequests_SetsUpgradeInsecureRequests()
        {
            _options.UpgradeInsecureRequests();

            Assert.True(((IHstsConfiguration)_options).UpgradeInsecureRequests);
        }
    }
}