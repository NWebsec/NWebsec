// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using NUnit.Framework;
using NWebsec.AspNetCore.Core.HttpHeaders;
using NWebsec.AspNetCore.Core.HttpHeaders.Configuration;

namespace NWebsec.AspNetCore.Core.Tests.HttpHeaders
{
    public partial class HeaderGeneratorTests
    {
        private readonly string[] _dummyPins = { "sha256=\"firstpin\"", "sha256=\"secondpin\"" };


        [Test]
        public void CreateHpkpResult_NegativeTimespanInConfig_ReturnsNull([Values(false, true)]bool reportOnly)
        {
            var hpkpConfig = new HpkpConfiguration { MaxAge = new TimeSpan(-1), Pins = _dummyPins };

            Assert.IsNull(_generator.CreateHpkpResult(hpkpConfig, false));
            Assert.IsNull(_generator.CreateHpkpResult(hpkpConfig, true));
        }

        [Test]
        public void CreateHpkpResult_ZeroTimespanInConfig_ReturnsSetHpkpResultWithMaxAgeOnly([Values(false, true)]bool reportOnly)
        {
            var hpkpConfig = new HpkpConfiguration { MaxAge = TimeSpan.Zero, IncludeSubdomains = true ,Pins = _dummyPins, ReportUri = "https://nwebsec.com/report"};

            var result = _generator.CreateHpkpResult(hpkpConfig, reportOnly);

            Assert.IsNotNull(result);
            Assert.AreEqual(HeaderResult.ResponseAction.Set, result.Action);
            Assert.AreEqual(HpkpHeaderName(reportOnly), result.Name);
            Assert.AreEqual("max-age=0", result.Value);
        }

        [Test]
        public void CreateHpkpResult_24hInConfig_ReturnsSetHpkpResult([Values(false, true)]bool reportOnly)
        {
            var hpkpConfig = new HpkpConfiguration { MaxAge = new TimeSpan(24, 0, 0), Pins = _dummyPins };

            var result = _generator.CreateHpkpResult(hpkpConfig, reportOnly);

            Assert.IsNotNull(result);
            Assert.AreEqual(HeaderResult.ResponseAction.Set, result.Action);
            Assert.AreEqual(HpkpHeaderName(reportOnly), result.Name);
            Assert.AreEqual("max-age=86400;pin-sha256=\"firstpin\";pin-sha256=\"secondpin\"", result.Value);
        }

        [Test]
        public void CreateHpkpResult_24hAndIncludesubdomainsConfig_ReturnsSetHpkpIncludesubdomainsResult([Values(false, true)]bool reportOnly)
        {
            var hpkpConfig = new HpkpConfiguration { MaxAge = new TimeSpan(24, 0, 0), IncludeSubdomains = true, Pins = _dummyPins };

            var result = _generator.CreateHpkpResult(hpkpConfig, reportOnly);

            Assert.IsNotNull(result);
            Assert.AreEqual(HeaderResult.ResponseAction.Set, result.Action);
            Assert.AreEqual(HpkpHeaderName(reportOnly), result.Name);
            Assert.AreEqual("max-age=86400;includeSubdomains;pin-sha256=\"firstpin\";pin-sha256=\"secondpin\"", result.Value);
        }

        [Test]
        public void CreateHpkpResult_24hAndReportUri_ReturnsSetHpkpReportUriResult([Values(false, true)]bool reportOnly)
        {
            var hpkpConfig = new HpkpConfiguration { MaxAge = new TimeSpan(24, 0, 0), ReportUri = "https://nwebsec.com/report", Pins = _dummyPins };

            var result = _generator.CreateHpkpResult(hpkpConfig, reportOnly);

            Assert.IsNotNull(result);
            Assert.AreEqual(HeaderResult.ResponseAction.Set, result.Action);
            Assert.AreEqual(HpkpHeaderName(reportOnly), result.Name);
            Assert.AreEqual("max-age=86400;pin-sha256=\"firstpin\";pin-sha256=\"secondpin\";report-uri=\"https://nwebsec.com/report\"", result.Value);
        }

        [Test]
        public void CreateHpkpResult_24hIncludeSubdomainsAndReportUri_ReturnsSetHpkpSubdomainsReportUriResult([Values(false, true)]bool reportOnly)
        {
            var hpkpConfig = new HpkpConfiguration { MaxAge = new TimeSpan(24, 0, 0), IncludeSubdomains = true, ReportUri = "https://nwebsec.com/report", Pins = _dummyPins };

            var result = _generator.CreateHpkpResult(hpkpConfig, reportOnly);

            Assert.IsNotNull(result);
            Assert.AreEqual(HeaderResult.ResponseAction.Set, result.Action);
            Assert.AreEqual(HpkpHeaderName(reportOnly), result.Name);
            Assert.AreEqual("max-age=86400;includeSubdomains;pin-sha256=\"firstpin\";pin-sha256=\"secondpin\";report-uri=\"https://nwebsec.com/report\"", result.Value);
        }

        private static string HpkpHeaderName(bool reportOnly)
        {
            return reportOnly ? "Public-Key-Pins-Report-Only" : "Public-Key-Pins";
        }
    }
}