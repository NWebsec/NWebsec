// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using NUnit.Framework;
using NWebsec.Core.HttpHeaders.Configuration;

namespace NWebsec.Mvc.Tests.TestHelpers
{
    [TestFixture]
    public class CspDirectiveConfigurationComparerTests
    {
        private CspDirectiveConfigurationComparer _comparer;

        [SetUp]
        public void Setup()
        {
            _comparer = new CspDirectiveConfigurationComparer();
        }
        [Test]
        public void Compare_IdenticalDefaultConfigs_ReturnsZero()
        {
            var result = _comparer.Compare(new CspDirectiveConfiguration(), new CspDirectiveConfiguration());

            Assert.AreEqual(0, result);
        }

        [Test]

        public void Compare_EnabledDiffers_ReturnsNonzero()
        {
            var firstConfig = new CspDirectiveConfiguration { Enabled = false };
            var secondConfig = new CspDirectiveConfiguration { Enabled = true };

            Assert.AreEqual(-1, _comparer.Compare(firstConfig, secondConfig));
            Assert.AreEqual(1, _comparer.Compare(secondConfig, firstConfig));
        }

        [Test]

        public void Compare_NoneSrcDiffers_ReturnsNonzero()
        {
            var firstConfig = new CspDirectiveConfiguration { NoneSrc = false };
            var secondConfig = new CspDirectiveConfiguration { NoneSrc = true };

            Assert.AreEqual(-1, _comparer.Compare(firstConfig, secondConfig));
            Assert.AreEqual(1, _comparer.Compare(secondConfig, firstConfig));
        }

        [Test]

        public void Compare_SelfSrcDiffers_ReturnsNonzero()
        {
            var firstConfig = new CspDirectiveConfiguration { SelfSrc = false };
            var secondConfig = new CspDirectiveConfiguration { SelfSrc = true };

            Assert.AreEqual(-1, _comparer.Compare(firstConfig, secondConfig));
            Assert.AreEqual(1, _comparer.Compare(secondConfig, firstConfig));
        }

        [Test]

        public void Compare_UnsafeEvalSrcDiffers_ReturnsNonzero()
        {
            var firstConfig = new CspDirectiveConfiguration { UnsafeEvalSrc = false };
            var secondConfig = new CspDirectiveConfiguration { UnsafeEvalSrc = true };

            Assert.AreEqual(-1, _comparer.Compare(firstConfig, secondConfig));
            Assert.AreEqual(1, _comparer.Compare(secondConfig, firstConfig));
        }

        [Test]

        public void Compare_UnsafeInlineSrcDiffers_ReturnsNonzero()
        {
            var firstConfig = new CspDirectiveConfiguration { UnsafeInlineSrc = false };
            var secondConfig = new CspDirectiveConfiguration { UnsafeInlineSrc = true };

            Assert.AreEqual(-1, _comparer.Compare(firstConfig, secondConfig));
            Assert.AreEqual(1, _comparer.Compare(secondConfig, firstConfig));
        }

        [Test]

        public void Compare_NonceDiffers_ReturnsNonzero()
        {
            var firstConfig = new CspDirectiveConfiguration { Nonce = "a" };
            var secondConfig = new CspDirectiveConfiguration { Nonce = "b" };

            Assert.AreEqual(-1, _comparer.Compare(firstConfig, secondConfig));
            Assert.AreEqual(1, _comparer.Compare(secondConfig, firstConfig));
        }

        [Test]

        public void Compare_CustomSourcesAreEqual_ReturnsZero()
        {
            var firstConfig = new CspDirectiveConfiguration { CustomSources = new[] { "a", "b" } };
            var secondConfig = new CspDirectiveConfiguration { CustomSources = new[] { "a", "b" } };

            Assert.AreEqual(0, _comparer.Compare(firstConfig, secondConfig));
        }

        [Test]

        public void Compare_CustomSourcesDiffersInLength_ReturnsNonzero()
        {
            var firstConfig = new CspDirectiveConfiguration { CustomSources = new[] { "a" } };
            var secondConfig = new CspDirectiveConfiguration { CustomSources = new[] { "a", "b" } };

            Assert.AreEqual(-1, _comparer.Compare(firstConfig, secondConfig));
            Assert.AreEqual(1, _comparer.Compare(secondConfig, firstConfig));
        }

        [Test]

        public void Compare_CustomSourcesDiffersInElements_ReturnsNonzero()
        {
            var firstConfig = new CspDirectiveConfiguration { CustomSources = new[] { "a", "b" } };
            var secondConfig = new CspDirectiveConfiguration { CustomSources = new[] { "a", "c" } };

            Assert.AreEqual(-1, _comparer.Compare(firstConfig, secondConfig));
            Assert.AreEqual(1, _comparer.Compare(secondConfig, firstConfig));
        }
    }
}