// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using NWebsec.Core.Common.Middleware.Options;
using Xunit;

namespace NWebsec.Core.SharedProject.Tests.Middleware.Options
{
    public class HpkpOptionsTest //TODO add missing tests
    {
        private readonly HpkpOptions _options;

        public HpkpOptionsTest()
        {
            _options = new HpkpOptions();
        }

        [Fact]
        public void Ctor_HttpsOnly_DefaultTrue()
        {
            Assert.True(_options.Config.HttpsOnly);
        }

        [Fact]
        public void MaxAge_ValidMaxage_SetsMaxage()
        {
            _options.MaxAge(minutes: 30);

            Assert.True(new TimeSpan(0, 30, 0) == _options.Config.MaxAge);
        }

        [Fact]
        public void MaxAge_ZeroMaxage_SetsMaxage()
        {
            _options.MaxAge();

            Assert.True(TimeSpan.Zero == _options.Config.MaxAge);
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

            Assert.True(_options.Config.IncludeSubdomains);
        }

        [Fact]
        public void AllResponses_DisablesHttpsOnly()
        {
            _options.AllResponses();

            Assert.False(_options.Config.HttpsOnly);
        }

        [Fact]
        public void HttpsOnly_SetsHttpsOnly()
        {
            _options.HttpsOnly();

            Assert.True(_options.Config.HttpsOnly);
        }
    }
}