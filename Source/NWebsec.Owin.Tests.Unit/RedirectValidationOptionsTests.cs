// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Linq;
using NUnit.Framework;

namespace NWebsec.Owin.Tests.Unit
{
    [TestFixture]
    public class RedirectValidationOptionsTests
    {
        private RedirectValidationOptions _options;

        [SetUp]
        public void Setup()
        {
            _options = new RedirectValidationOptions();
        }

        [Test]
        public void AllowedDestinations_ValidDestinations_SetsDestinations()
        {
            var destinations = new[] {"https://www.nwebsec.com/", "http://klings.org:81/SomePath"};
            _options.AllowedDestinations(destinations);

            Assert.IsTrue(destinations.SequenceEqual(_options.AllowedUris), String.Join(" ",destinations) + " / " + String.Join(" ", _options.AllowedUris));
        }

        [Test]
        public void AllowedDestinations_NoDestinations_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() => _options.AllowedDestinations());
        }

        [Test]
        public void AllowedDestinations_RelativeUriDestinations_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() => _options.AllowedDestinations("/relative/uri"));
        }

        [Test]
        public void AllowedDestinations_MalformedUriDestinations_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() => _options.AllowedDestinations("http:///example.com///hey"));
        }

        [Test]
        public void AllowSameHostRedirectsToHttps_Validports_NoException()
        {
            var allValidPorts = Enumerable.Range(1, 65535).ToArray();

            _options.AllowSameHostRedirectsToHttps(allValidPorts);

            Assert.IsTrue(_options.SameHostRedirectConfiguration.Enabled);
            Assert.AreSame(allValidPorts, _options.SameHostRedirectConfiguration.Ports);
            Assert.AreEqual(65535, allValidPorts.Last());
        }

        [Test]
        public void AllowSameHostRedirectsToHttps_LowerboundaryCheck_ThrowsException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _options.AllowSameHostRedirectsToHttps(0));
            Assert.IsFalse(_options.SameHostRedirectConfiguration.Enabled);
        }

        [Test]
        public void AllowSameHostRedirectsToHttps_UpperboundaryCheck_ThrowsException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _options.AllowSameHostRedirectsToHttps(65536));
            Assert.IsFalse(_options.SameHostRedirectConfiguration.Enabled);
        }
    }
}