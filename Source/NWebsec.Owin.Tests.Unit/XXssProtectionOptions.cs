// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using NUnit.Framework;
using NWebsec.HttpHeaders;

namespace NWebsec.Owin.Tests.Unit
{
    [TestFixture]
    public class XXssProtectionOptionsTests
    {
        private XXssProtectionOptions _options;

        [SetUp]
        public void Setup()
        {
            _options = new XXssProtectionOptions();
        }

        [Test]
        public void Disabled_SetsDisabled()
        {
            _options.Disabled();

            Assert.AreEqual(XXssProtectionPolicy.FilterDisabled, _options.Policy);
            Assert.IsFalse(_options.BlockMode);
        }

        [Test]
        public void Enabled_SetsEnabled()
        {
            _options.Enabled();

            Assert.AreEqual(XXssProtectionPolicy.FilterEnabled, _options.Policy);
            Assert.IsFalse(_options.BlockMode);
        }

        [Test]
        public void EnabledWithBlockMode_SetsEnabledWithBlockMode()
        {
            _options.EnabledWithBlockMode();

            Assert.AreEqual(XXssProtectionPolicy.FilterEnabled, _options.Policy);
            Assert.IsTrue(_options.BlockMode);
        }
    }
}