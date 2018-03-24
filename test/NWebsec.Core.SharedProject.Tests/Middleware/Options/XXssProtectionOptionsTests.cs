// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using NWebsec.Core.Common.HttpHeaders;
using NWebsec.Core.Common.Middleware.Options;
using Xunit;

namespace NWebsec.Core.SharedProject.Tests.Middleware.Options
{
    public class XXssProtectionOptionsTests
    {
        private readonly XXssProtectionOptions _options;

        public XXssProtectionOptionsTests()
        {
            _options = new XXssProtectionOptions();
        }

        [Fact]
        public void Disabled_SetsDisabled()
        {
            _options.Disabled();

            Assert.Equal(XXssPolicy.FilterDisabled, _options.Policy);
            Assert.False(_options.BlockMode);
        }

        [Fact]
        public void Enabled_SetsEnabled()
        {
            _options.Enabled();

            Assert.Equal(XXssPolicy.FilterEnabled, _options.Policy);
            Assert.False(_options.BlockMode);
        }

        [Fact]
        public void EnabledWithBlockMode_SetsEnabledWithBlockMode()
        {
            _options.EnabledWithBlockMode();

            Assert.Equal(XXssPolicy.FilterEnabled, _options.Policy);
            Assert.True(_options.BlockMode);
        }
    }
}