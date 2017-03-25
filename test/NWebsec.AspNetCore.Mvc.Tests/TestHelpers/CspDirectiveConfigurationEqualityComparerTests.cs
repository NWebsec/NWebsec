// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using Xunit;
using NWebsec.AspNetCore.Core.HttpHeaders.Configuration;

namespace NWebsec.AspNetCore.Mvc.Tests.TestHelpers
{
    public class CspDirectiveConfigurationEqualityComparerTests
    {
        private readonly CspDirectiveConfigurationEqualityComparer _equalityComparer;

        public CspDirectiveConfigurationEqualityComparerTests()
        {
            _equalityComparer = new CspDirectiveConfigurationEqualityComparer();
        }

        [Fact]
        public void Equals_IdenticalDefaultConfigs_ReturnsTrue()
        {
            var result = _equalityComparer.Equals(new CspDirectiveConfiguration(), new CspDirectiveConfiguration());

            Assert.True(result);
        }

        [Fact]
        public void Equals_EnabledDiffers_ReturnsFalse()
        {
            var firstConfig = new CspDirectiveConfiguration { Enabled = false };
            var secondConfig = new CspDirectiveConfiguration { Enabled = true };

            Assert.False(_equalityComparer.Equals(firstConfig, secondConfig));
            Assert.False(_equalityComparer.Equals(secondConfig, firstConfig));
        }

        [Fact]
        public void Equals_NoneSrcDiffers_ReturnsFalse()
        {
            var firstConfig = new CspDirectiveConfiguration { NoneSrc = false };
            var secondConfig = new CspDirectiveConfiguration { NoneSrc = true };

            Assert.False(_equalityComparer.Equals(firstConfig, secondConfig));
            Assert.False(_equalityComparer.Equals(secondConfig, firstConfig));
        }

        [Fact]
        public void Equals_SelfSrcDiffers_ReturnsFalse()
        {
            var firstConfig = new CspDirectiveConfiguration { SelfSrc = false };
            var secondConfig = new CspDirectiveConfiguration { SelfSrc = true };

            Assert.False(_equalityComparer.Equals(firstConfig, secondConfig));
            Assert.False(_equalityComparer.Equals(secondConfig, firstConfig));
        }

        [Fact]
        public void Equals_UnsafeInlineSrcDiffers_ReturnsFalse()
        {
            var firstConfig = new CspDirectiveConfiguration { UnsafeInlineSrc = false };
            var secondConfig = new CspDirectiveConfiguration { UnsafeInlineSrc = true };

            Assert.False(_equalityComparer.Equals(firstConfig, secondConfig));
            Assert.False(_equalityComparer.Equals(secondConfig, firstConfig));
        }

        [Fact]
        public void Equals_UnsafeEvalSrcDiffers_ReturnsFalse()
        {
            var firstConfig = new CspDirectiveConfiguration { UnsafeEvalSrc = false };
            var secondConfig = new CspDirectiveConfiguration { UnsafeEvalSrc = true };

            Assert.False(_equalityComparer.Equals(firstConfig, secondConfig));
            Assert.False(_equalityComparer.Equals(secondConfig, firstConfig));
        }

        [Fact]
        public void Equals_StrictDynamicSrcDiffers_ReturnsFalse()
        {
            var firstConfig = new CspDirectiveConfiguration { StrictDynamicSrc = false };
            var secondConfig = new CspDirectiveConfiguration { StrictDynamicSrc = true };

            Assert.False(_equalityComparer.Equals(firstConfig, secondConfig));
            Assert.False(_equalityComparer.Equals(secondConfig, firstConfig));
        }

        [Fact]
        public void Equals_NonceDiffers_ReturnsFalse()
        {
            var firstConfig = new CspDirectiveConfiguration { Nonce = "a" };
            var secondConfig = new CspDirectiveConfiguration { Nonce = "b" };

            Assert.False(_equalityComparer.Equals(firstConfig, secondConfig));
            Assert.False(_equalityComparer.Equals(secondConfig, firstConfig));
        }

        [Fact]
        public void Equals_CustomSourcesAreEqual_ReturnsTrue()
        {
            var firstConfig = new CspDirectiveConfiguration { CustomSources = new[] { "a", "b" } };
            var secondConfig = new CspDirectiveConfiguration { CustomSources = new[] { "a", "b" } };

            Assert.True(_equalityComparer.Equals(firstConfig, secondConfig));
        }

        [Fact]
        public void Equals_CustomSourcesDiffersInLength_ReturnsFalse()
        {
            var firstConfig = new CspDirectiveConfiguration { CustomSources = new[] { "a" } };
            var secondConfig = new CspDirectiveConfiguration { CustomSources = new[] { "a", "b" } };

            Assert.False(_equalityComparer.Equals(firstConfig, secondConfig));
            Assert.False(_equalityComparer.Equals(secondConfig, firstConfig));
        }

        [Fact]
        public void Equals_CustomSourcesDiffersInElements_ReturnsFalse()
        {
            var firstConfig = new CspDirectiveConfiguration { CustomSources = new[] { "a", "b" } };
            var secondConfig = new CspDirectiveConfiguration { CustomSources = new[] { "a", "c" } };

            Assert.False(_equalityComparer.Equals(firstConfig, secondConfig));
            Assert.False(_equalityComparer.Equals(secondConfig, firstConfig));
        }
    }
}