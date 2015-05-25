// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using NUnit.Framework;
using NWebsec.Core.HttpHeaders.Configuration;

namespace NWebsec.Mvc.Tests.Unit.TestHelpers
{
    [TestFixture]
    public class CspPluginTypesDirectiveConfigurationComparerTests
    {

        private CspPluginTypesDirectiveConfigurationComparer _comparer;

        [SetUp]
        public void Setup()
        {
            _comparer = new CspPluginTypesDirectiveConfigurationComparer();
        }
        [Test]
        public void Compare_IdenticalDefaultConfigs_ReturnsZero()
        {
            var result = _comparer.Compare(new CspPluginTypesDirectiveConfiguration(), new CspPluginTypesDirectiveConfiguration());

            Assert.AreEqual(0, result);
        }

        [Test]

        public void Compare_EnabledDiffers_ReturnsNonzero()
        {
            var firstConfig = new CspPluginTypesDirectiveConfiguration { Enabled = false };
            var secondConfig = new CspPluginTypesDirectiveConfiguration { Enabled = true };

            Assert.AreEqual(-1, _comparer.Compare(firstConfig, secondConfig));
            Assert.AreEqual(1, _comparer.Compare(secondConfig, firstConfig));
        }

        [Test]

        public void Compare_CustomSourcesAreEqual_ReturnsZero()
        {
            var firstConfig = new CspPluginTypesDirectiveConfiguration { MediaTypes = new[] { "application/pdf", "image/png" } };
            var secondConfig = new CspPluginTypesDirectiveConfiguration { MediaTypes = new[] { "application/pdf", "image/png" } };

            Assert.AreEqual(0, _comparer.Compare(firstConfig, secondConfig));
        }

        [Test]

        public void Compare_CustomSourcesDiffersInLength_ReturnsNonzero()
        {
            var firstConfig = new CspPluginTypesDirectiveConfiguration { MediaTypes = new[] { "application/pdf" } };
            var secondConfig = new CspPluginTypesDirectiveConfiguration { MediaTypes = new[] { "application/pdf", "image/png" } };

            Assert.Less(_comparer.Compare(firstConfig, secondConfig), 0);
            Assert.Greater(_comparer.Compare(secondConfig, firstConfig), 0);
        }

        [Test]

        public void Compare_CustomSourcesDiffersInElements_ReturnsNonzero()
        {
            var firstConfig = new CspPluginTypesDirectiveConfiguration { MediaTypes = new[] { "application/pdf", "image/png" } };
            var secondConfig = new CspPluginTypesDirectiveConfiguration { MediaTypes = new[] { "application/pdf", "video/mp4" } };

            Assert.Less(_comparer.Compare(firstConfig, secondConfig), 0);
            Assert.Greater(_comparer.Compare(secondConfig, firstConfig), 0);
        }
    }
}