// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using NWebsec.Core.Common.HttpHeaders.Configuration;
using Xunit;

namespace NWebsec.Mvc.CommonProject.Tests.TestHelpers
{

    public class CspPluginTypesDirectiveConfigurationComparerTests
    {

        private readonly CspPluginTypesDirectiveConfigurationEqualityComparer _equalityComparer;

        public CspPluginTypesDirectiveConfigurationComparerTests()
        {
            _equalityComparer = new CspPluginTypesDirectiveConfigurationEqualityComparer();
        }

        [Fact]
        public void Equals_IdenticalDefaultConfigs_ReturnsTrue()
        {
            var result = _equalityComparer.Equals(new CspPluginTypesDirectiveConfiguration(), new CspPluginTypesDirectiveConfiguration());

            Assert.True(result);
        }

        [Fact]

        public void Equals_EnabledDiffers_ReturnsFalse()
        {
            var firstConfig = new CspPluginTypesDirectiveConfiguration { Enabled = false };
            var secondConfig = new CspPluginTypesDirectiveConfiguration { Enabled = true };

            Assert.False(_equalityComparer.Equals(firstConfig, secondConfig));
            Assert.False(_equalityComparer.Equals(secondConfig, firstConfig));
        }

        [Fact]

        public void Equals_CustomSourcesAreEqual_ReturnsTrue()
        {
            var firstConfig = new CspPluginTypesDirectiveConfiguration { MediaTypes = new[] { "application/pdf", "image/png" } };
            var secondConfig = new CspPluginTypesDirectiveConfiguration { MediaTypes = new[] { "application/pdf", "image/png" } };

            Assert.True(_equalityComparer.Equals(firstConfig, secondConfig));
        }

        [Fact]

        public void Equals_CustomSourcesDiffersInLength_ReturnsFalse()
        {
            var firstConfig = new CspPluginTypesDirectiveConfiguration { MediaTypes = new[] { "application/pdf" } };
            var secondConfig = new CspPluginTypesDirectiveConfiguration { MediaTypes = new[] { "application/pdf", "image/png" } };

            Assert.False(_equalityComparer.Equals(firstConfig, secondConfig));
            Assert.False(_equalityComparer.Equals(secondConfig, firstConfig));
        }

        [Fact]

        public void Equals_CustomSourcesDiffersInElements_ReturnsFalse()
        {
            var firstConfig = new CspPluginTypesDirectiveConfiguration { MediaTypes = new[] { "application/pdf", "image/png" } };
            var secondConfig = new CspPluginTypesDirectiveConfiguration { MediaTypes = new[] { "application/pdf", "video/mp4" } };

            Assert.False(_equalityComparer.Equals(firstConfig, secondConfig));
            Assert.False(_equalityComparer.Equals(secondConfig, firstConfig));
        }
    }
}