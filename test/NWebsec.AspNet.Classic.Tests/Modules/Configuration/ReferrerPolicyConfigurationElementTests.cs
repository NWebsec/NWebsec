// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using NWebsec.Core.Common.HttpHeaders;
using NWebsec.Modules.Configuration;
using Xunit;

namespace NWebsec.AspNet.Classic.Tests.Modules.Configuration
{
    public class ReferrerPolicyConfigurationElementTests
    {

        [Theory]
        [InlineData("no-referrer", ReferrerPolicy.NoReferrer)]
        [InlineData("no-referrer-when-downgrade", ReferrerPolicy.NoReferrerWhenDowngrade)]
        [InlineData("origin", ReferrerPolicy.Origin)]
        [InlineData("origin-when-cross-origin", ReferrerPolicy.OriginWhenCrossOrigin)]
        [InlineData("same-origin", ReferrerPolicy.SameOrigin)]
        [InlineData("strict-origin", ReferrerPolicy.StrictOrigin)]
        [InlineData("strict-origin-when-cross-origin", ReferrerPolicy.StrictOriginWhenCrossOrigin)]
        [InlineData("unsafe-url", ReferrerPolicy.UnsafeUrl)]
        public void GetPolicy_EnabledAndKnownConfigValue_GetsPolicy(string configValue, ReferrerPolicy expectedPolicy)
        {
            var config = new ReferrerPolicyConfigurationElement { Enabled = true, ConfigPolicy = configValue };

            Assert.Equal(expectedPolicy, config.Policy);
        }

        [Fact]
        public void GetPolicy_EnabledAndUnknownConfigValue_Throws()
        {
            var config = new ReferrerPolicyConfigurationElement { Enabled = true, ConfigPolicy = "yolo" };
            Assert.Throws<ArgumentException>(() => config.Policy);
        }

        [Fact]
        public void GetPolicy_Disabled_ReturnsDisabledPolicy()
        {
            var config = new ReferrerPolicyConfigurationElement
            {
                Enabled = false,
                ConfigPolicy = "no-referrer"
            };

            Assert.Equal(ReferrerPolicy.Disabled, config.Policy);
        }
    }
}
