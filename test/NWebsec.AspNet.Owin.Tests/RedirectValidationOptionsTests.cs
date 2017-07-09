// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Linq;
using NWebsec.Owin;
using Xunit;

namespace NWebsec.AspNet.Owin.Tests
{
    public class RedirectValidationOptionsTests
    {
        private readonly RedirectValidationOptions _options;

        public RedirectValidationOptionsTests()
        {
            _options = new RedirectValidationOptions();
        }

        [Fact]
        public void AllowedDestinations_ValidDestinations_SetsDestinations()
        {
            var destinations = new[] { "https://www.nwebsec.com/", "http://klings.org:81/SomePath" };
            _options.AllowedDestinations(destinations);

            Assert.True(destinations.SequenceEqual(_options.AllowedUris), String.Join(" ", destinations) + " / " + String.Join(" ", _options.AllowedUris));
        }

        [Fact]
        public void AllowedDestinations_NoDestinations_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() => _options.AllowedDestinations());
        }

        [Fact]
        public void AllowedDestinations_RelativeUriDestinations_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() => _options.AllowedDestinations("/relative/uri"));
        }

        [Fact]
        public void AllowedDestinations_MalformedUriDestinations_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() => _options.AllowedDestinations("http:///example.com///hey"));
        }

        [Fact]
        public void AllowSameHostRedirectsToHttps_Validports_NoException()
        {
            var allValidPorts = Enumerable.Range(1, 65535).ToArray();

            _options.AllowSameHostRedirectsToHttps(allValidPorts);

            Assert.True(_options.SameHostRedirectConfiguration.Enabled);
            Assert.Same(allValidPorts, _options.SameHostRedirectConfiguration.Ports);
            Assert.Equal(65535, allValidPorts.Last());
        }

        [Fact]
        public void AllowSameHostRedirectsToHttps_LowerboundaryCheck_ThrowsException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _options.AllowSameHostRedirectsToHttps(0));
            Assert.False(_options.SameHostRedirectConfiguration.Enabled);
        }

        [Fact]
        public void AllowSameHostRedirectsToHttps_UpperboundaryCheck_ThrowsException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _options.AllowSameHostRedirectsToHttps(65536));
            Assert.False(_options.SameHostRedirectConfiguration.Enabled);
        }
    }
}