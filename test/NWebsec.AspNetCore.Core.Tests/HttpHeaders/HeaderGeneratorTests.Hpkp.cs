// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Xunit;
using NWebsec.AspNetCore.Core.HttpHeaders;
using NWebsec.AspNetCore.Core.HttpHeaders.Configuration;

namespace NWebsec.AspNetCore.Core.Tests.HttpHeaders
{
    public partial class HeaderGeneratorTests
    {
        public static readonly IEnumerable<object> ReportOnly = new TheoryData<bool> { false, true };

        private readonly string[] _dummyPins = { "sha256=\"firstpin\"", "sha256=\"secondpin\"" };

        [Theory]
        [MemberData(nameof(ReportOnly))]
        public void CreateHpkpResult_NegativeTimespanInConfig_ReturnsNull(bool reportOnly)
        {
            var hpkpConfig = new HpkpConfiguration { MaxAge = new TimeSpan(-1), Pins = _dummyPins };

            Assert.Null(_generator.CreateHpkpResult(hpkpConfig, false));
            Assert.Null(_generator.CreateHpkpResult(hpkpConfig, true));
        }

        [Theory]
        [MemberData(nameof(ReportOnly))]
        public void CreateHpkpResult_ZeroTimespanInConfig_ReturnsSetHpkpResultWithMaxAgeOnly(bool reportOnly)
        {
            var hpkpConfig = new HpkpConfiguration { MaxAge = TimeSpan.Zero, IncludeSubdomains = true, Pins = _dummyPins, ReportUri = "https://nwebsec.com/report" };

            var result = _generator.CreateHpkpResult(hpkpConfig, reportOnly);

            Assert.NotNull(result);
            Assert.Equal(HeaderResult.ResponseAction.Set, result.Action);
            Assert.Equal(HpkpHeaderName(reportOnly), result.Name);
            Assert.Equal("max-age=0", result.Value);
        }

        [Theory]
        [MemberData(nameof(ReportOnly))]
        public void CreateHpkpResult_24hInConfig_ReturnsSetHpkpResult(bool reportOnly)
        {
            var hpkpConfig = new HpkpConfiguration { MaxAge = new TimeSpan(24, 0, 0), Pins = _dummyPins };

            var result = _generator.CreateHpkpResult(hpkpConfig, reportOnly);

            Assert.NotNull(result);
            Assert.Equal(HeaderResult.ResponseAction.Set, result.Action);
            Assert.Equal(HpkpHeaderName(reportOnly), result.Name);
            Assert.Equal("max-age=86400;pin-sha256=\"firstpin\";pin-sha256=\"secondpin\"", result.Value);
        }

        [Theory]
        [MemberData(nameof(ReportOnly))]
        public void CreateHpkpResult_24hAndIncludesubdomainsConfig_ReturnsSetHpkpIncludesubdomainsResult(bool reportOnly)
        {
            var hpkpConfig = new HpkpConfiguration { MaxAge = new TimeSpan(24, 0, 0), IncludeSubdomains = true, Pins = _dummyPins };

            var result = _generator.CreateHpkpResult(hpkpConfig, reportOnly);

            Assert.NotNull(result);
            Assert.Equal(HeaderResult.ResponseAction.Set, result.Action);
            Assert.Equal(HpkpHeaderName(reportOnly), result.Name);
            Assert.Equal("max-age=86400;includeSubDomains;pin-sha256=\"firstpin\";pin-sha256=\"secondpin\"", result.Value);
        }

        [Theory]
        [MemberData(nameof(ReportOnly))]
        public void CreateHpkpResult_24hAndReportUri_ReturnsSetHpkpReportUriResult(bool reportOnly)
        {
            var hpkpConfig = new HpkpConfiguration { MaxAge = new TimeSpan(24, 0, 0), ReportUri = "https://nwebsec.com/report", Pins = _dummyPins };

            var result = _generator.CreateHpkpResult(hpkpConfig, reportOnly);

            Assert.NotNull(result);
            Assert.Equal(HeaderResult.ResponseAction.Set, result.Action);
            Assert.Equal(HpkpHeaderName(reportOnly), result.Name);
            Assert.Equal("max-age=86400;pin-sha256=\"firstpin\";pin-sha256=\"secondpin\";report-uri=\"https://nwebsec.com/report\"", result.Value);
        }

        [Theory]
        [MemberData(nameof(ReportOnly))]
        public void CreateHpkpResult_24hIncludeSubdomainsAndReportUri_ReturnsSetHpkpSubdomainsReportUriResult(bool reportOnly)
        {
            var hpkpConfig = new HpkpConfiguration { MaxAge = new TimeSpan(24, 0, 0), IncludeSubdomains = true, ReportUri = "https://nwebsec.com/report", Pins = _dummyPins };

            var result = _generator.CreateHpkpResult(hpkpConfig, reportOnly);

            Assert.NotNull(result);
            Assert.Equal(HeaderResult.ResponseAction.Set, result.Action);
            Assert.Equal(HpkpHeaderName(reportOnly), result.Name);
            Assert.Equal("max-age=86400;includeSubDomains;pin-sha256=\"firstpin\";pin-sha256=\"secondpin\";report-uri=\"https://nwebsec.com/report\"", result.Value);
        }

        private static string HpkpHeaderName(bool reportOnly)
        {
            return reportOnly ? "Public-Key-Pins-Report-Only" : "Public-Key-Pins";
        }
    }
}