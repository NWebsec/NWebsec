// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using NUnit.Framework;

namespace NWebsec.Owin.Tests.Unit
{
    [TestFixture]
    public class CspReportUriDirectiveTests
    {
        private CspReportUriDirective _directive;

        [SetUp]
        public void Setup()
        {
            _directive = new CspReportUriDirective();
        }

        [Test]
        public void Uris_ValidUris_AddsReportUris()
        {
            _directive.Uris("/report", "/otherReport");

            var expected = new[] { "/report", "/otherReport" };
            CollectionAssert.AreEqual(expected, _directive.ReportUris);
        }

        [Test]
        public void Uris_NoUris_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() => _directive.Uris());
        }

        [Test]
        public void Uris_InvalidUris_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() => _directive.Uris("/report", "https://*.nwebsec.com/report"));
        }
    }
}