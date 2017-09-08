using NWebsec.AspNetCore.Core.HttpHeaders;
using NWebsec.AspNetCore.Core.HttpHeaders.Configuration;
using System;
using Xunit;

namespace NWebsec.AspNetCore.Core.Tests.HttpHeaders
{
    public partial class HeaderGeneratorTests
    {
        [Fact]
        public void CreateExpectCtResult_NegativeTimespanInConfig_ReturnsNull()
        {
            var expectCtConfig = new ExpectCtConfiguration { MaxAge = new TimeSpan(-1) };

            var result = _generator.CreateExpectCtResult(expectCtConfig);
            
            Assert.Null(result);
        }

        [Fact]
        public void CreateExpectCtResult_ZeroTimespanInConfig_ReturnsSetExpectCtResult()
        {
            var expectCtConfig = new ExpectCtConfiguration { MaxAge = new TimeSpan(0) };

            var result = _generator.CreateExpectCtResult(expectCtConfig);

            Assert.NotNull(result);
            Assert.Equal(HeaderResult.ResponseAction.Set, result.Action);
            Assert.Equal("Expect-CT", result.Name);
            Assert.Equal("max-age=0", result.Value);
        }

        [Fact]
        public void CreateExpectCtResult_24hInConfig_ReturnsSetExpectCtsResult()
        {
            var expectCtConfig = new ExpectCtConfiguration { MaxAge = new TimeSpan(24, 0, 0) };

            var result = _generator.CreateExpectCtResult(expectCtConfig);

            Assert.NotNull(result);
            Assert.Equal(HeaderResult.ResponseAction.Set, result.Action);
            Assert.Equal("Expect-CT", result.Name);
            Assert.Equal("max-age=86400", result.Value);
        }

        [Fact]
        public void CreateExpectCtResult_24hInConfigAndEnforce_ReturnsSetExpectCtResult()
        {
            var expectCtConfig = new ExpectCtConfiguration { MaxAge = new TimeSpan(24, 0, 0), Enforce = true };

            var result = _generator.CreateExpectCtResult(expectCtConfig);

            Assert.NotNull(result);
            Assert.Equal(HeaderResult.ResponseAction.Set, result.Action);
            Assert.Equal("Expect-CT", result.Name);
            Assert.Equal("enforce; max-age=86400", result.Value);
        }

        [Fact]
        public void CreateExpectCtResult_24hInConfigAndEnforceAndReportUri_ReturnsSetExpectCtResult()
        {
            var expectCtConfig = new ExpectCtConfiguration { MaxAge = new TimeSpan(24, 0, 0), Enforce = true, ReportUri = "https://localhost/" };

            var result = _generator.CreateExpectCtResult(expectCtConfig);

            Assert.NotNull(result);
            Assert.Equal(HeaderResult.ResponseAction.Set, result.Action);
            Assert.Equal("Expect-CT", result.Name);
            Assert.Equal("enforce; max-age=86400; report-uri=\"https://localhost/\"", result.Value);
        }

        [Fact]
        public void CreateExpectCtResult_24hInConfigAndReportUri_ReturnsSetExpectCtResult()
        {
            var expectCtConfig = new ExpectCtConfiguration { MaxAge = new TimeSpan(24, 0, 0), ReportUri = "https://localhost/" };

            var result = _generator.CreateExpectCtResult(expectCtConfig);

            Assert.NotNull(result);
            Assert.Equal(HeaderResult.ResponseAction.Set, result.Action);
            Assert.Equal("Expect-CT", result.Name);
            Assert.Equal("max-age=86400; report-uri=\"https://localhost/\"", result.Value);
        }
    }
}
