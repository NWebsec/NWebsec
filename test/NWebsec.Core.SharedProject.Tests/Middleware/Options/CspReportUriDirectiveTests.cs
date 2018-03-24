// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using NWebsec.Core.Common.Middleware.Options;
using Xunit;

namespace NWebsec.Core.SharedProject.Tests.Middleware.Options
{
    public class CspReportUriDirectiveTests
    {
        private readonly CspReportUriDirective _directive;

        public CspReportUriDirectiveTests()
        {
            _directive = new CspReportUriDirective();
        }

        [Fact]
        public void Uris_ValidUris_AddsReportUris()
        {
            _directive.Uris("/report", "/otherReport");

            var expected = new[] { "/report", "/otherReport" };
            Assert.Equal(expected, _directive.ReportUris);
        }

        [Fact]
        public void Uris_NoUris_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() => _directive.Uris());
        }

        [Fact]
        public void Uris_InvalidUris_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() => _directive.Uris("/report", "https://*.nwebsec.com/report"));
        }
    }
}