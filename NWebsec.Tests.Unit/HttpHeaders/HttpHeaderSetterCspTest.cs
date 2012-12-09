// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Web;
using Moq;
using NUnit.Framework;
using NWebsec.HttpHeaders;
using NWebsec.Modules.Configuration.Csp;

namespace nWebsec.Tests.Unit.HttpHeaders
{
    [TestFixture]
    public class HttpHeaderSetterCspTest
    {
        HttpHeaderSetter headerSetter;
        Mock<HttpRequestBase> mockRequest;
        Mock<HttpResponseBase> mockResponse;
        Mock<HttpContextBase> mockContext;
        private Mock<HttpCachePolicyBase> mockCachePolicy;

        private const string AppPath = "/MyApp";

        [SetUp]
        public void HeaderModuleTestInitialize()
        {
            mockContext = new Mock<HttpContextBase>();
            mockRequest = new Mock<HttpRequestBase>();
            mockResponse = new Mock<HttpResponseBase>();
            mockResponse.SetupAllProperties();
            mockRequest.SetupAllProperties();
            mockContext.SetupAllProperties();
            var mockHandler = new Mock<IHttpHandler>();

            var testUri = new Uri("http://localhost/NWebsecWebforms/");
            mockRequest.Setup(r => r.Url).Returns(testUri);
            mockRequest.Setup(r => r.ApplicationPath).Returns(AppPath);

            mockCachePolicy = new Mock<HttpCachePolicyBase>();
            mockResponse.Setup(x => x.Cache).Returns(mockCachePolicy.Object);

            mockContext.Setup(c => c.Request).Returns(mockRequest.Object);
            mockContext.Setup(c => c.Response).Returns(mockResponse.Object);
            mockContext.Setup(c => c.CurrentHandler).Returns(mockHandler.Object);

            headerSetter = new HttpHeaderSetter(mockContext.Object);

        }

        [Test]
        public void AddCspHeaders_DisabledInConfig_DoesNotAddCspHeader()
        {
            var cspConfig = new CspConfigurationElement { Enabled = false, DefaultSrc = { Self = true } };

            headerSetter.AddCspHeaders(cspConfig, false);

            mockResponse.Verify(x => x.AddHeader(It.IsAny<String>(), It.IsAny<String>()), Times.Never());
        }

        [Test]
        public void AddCspHeaders_EnabledButNoDirectivesOrReportUriEnabled_DoesNotAddCspHeader()
        {
            var cspConfig = new CspConfigurationElement { Enabled = true };

            headerSetter.AddCspHeaders(cspConfig, false);

            mockResponse.Verify(x => x.AddHeader(It.IsAny<String>(), It.IsAny<String>()), Times.Never());
        }

        [Test]
        public void AddCspHeaders_EnabledButNoDirectivesEnabledAndReportUriEnabled_DoesNotAddCspHeader()
        {
            var cspConfig = new CspConfigurationElement { Enabled = true };
            cspConfig.ReportUriDirective.Enabled = true;
            cspConfig.ReportUriDirective.EnableBuiltinHandler = true;

            headerSetter.AddCspHeaders(cspConfig, false);

            mockResponse.Verify(x => x.AddHeader(It.IsAny<String>(), It.IsAny<String>()), Times.Never());
        }

        [Test]
        public void AddCspHeaders_XCspEnabledInConfig_AddsXCspHeader()
        {
            var cspConfig = new CspConfigurationElement
            {
                Enabled = true,
                XContentSecurityPolicyHeader = true,
                XWebKitCspHeader = false,
                DefaultSrc = { Self = true }
            };

            headerSetter.AddCspHeaders(cspConfig, false);

            mockResponse.Verify(x => x.AddHeader("X-Content-Security-Policy", It.IsAny<String>()), Times.Once());
        }

        [Test]
        public void AddCspHeaders_XWebkitCspEnabledInConfig_AddsXWebkitCspHeader()
        {
            var cspConfig = new CspConfigurationElement
            {
                Enabled = true,
                XContentSecurityPolicyHeader = false,
                XWebKitCspHeader = true,
                DefaultSrc = { Self = true }
            };

            headerSetter.AddCspHeaders(cspConfig, false);

            mockResponse.Verify(x => x.AddHeader("X-WebKit-CSP", It.IsAny<String>()), Times.Once());
        }

        [Test]
        public void AddCspHeaders_XCspAndXWebkitCspEnabledInConfig_AddsXcspAndXWebkitCspHeader()
        {
            var cspConfig = new CspConfigurationElement { Enabled = true, XContentSecurityPolicyHeader = true, XWebKitCspHeader = true };
            cspConfig.DefaultSrc.Self = true;

            headerSetter.AddCspHeaders(cspConfig, false);

            mockResponse.Verify(x => x.AddHeader("X-Content-Security-Policy", It.IsAny<String>()), Times.Once());
            mockResponse.Verify(x => x.AddHeader("X-WebKit-CSP", It.IsAny<String>()), Times.Once());
        }

        [Test]
        public void AddCspHeaders_XCspReportOnlyEnabledInConfig_AddsXCspReportOnlyHeader()
        {
            var cspConfig = new CspConfigurationElement
            {
                Enabled = true,
                XContentSecurityPolicyHeader = true,
                XWebKitCspHeader = false,
                DefaultSrc = { Self = true }
            };

            headerSetter.AddCspHeaders(cspConfig, reportOnly: true);

            mockResponse.Verify(x => x.AddHeader("X-Content-Security-Policy-Report-Only", It.IsAny<String>()), Times.Once());
        }

        [Test]
        public void AddCspHeaders_XWebkitCspReportOnlyEnabledInConfig_AddsXWebkitCspReportOnlyHeader()
        {
            var cspConfig = new CspConfigurationElement
            {
                Enabled = true,
                XContentSecurityPolicyHeader = false,
                XWebKitCspHeader = true,
                DefaultSrc = { Self = true }
            };

            headerSetter.AddCspHeaders(cspConfig, true);

            mockResponse.Verify(x => x.AddHeader("X-WebKit-CSP-Report-Only", It.IsAny<String>()), Times.Once());
        }

        [Test]
        public void AddCspHeaders_XCspAndXWebkitCspReportOnlyEnabledInConfig_AddsXCspAndXWebkitCspReportOnlyHeader()
        {
            var cspConfig = new CspConfigurationElement
            {
                Enabled = true,
                XContentSecurityPolicyHeader = true,
                XWebKitCspHeader = true,
                DefaultSrc = { Self = true }
            };

            headerSetter.AddCspHeaders(cspConfig, true);

            mockResponse.Verify(x => x.AddHeader("X-Content-Security-Policy-Report-Only", It.IsAny<String>()), Times.Once());
            mockResponse.Verify(x => x.AddHeader("X-WebKit-CSP-Report-Only", It.IsAny<String>()), Times.Once());
        }

        [Test]
        public void AddCspHeaders_CspWithTwoDirectives_AddsCorrectCspHeader()
        {
            var cspConfig = new CspConfigurationElement
            {
                Enabled = true,
                DefaultSrc = { Self = true },
                ScriptSrc = { None = true }
            };

            headerSetter.AddCspHeaders(cspConfig, false);

            mockResponse.Verify(x => x.AddHeader("Content-Security-Policy", "default-src 'self'; script-src 'none'"), Times.Once());
        }

        [Test]
        public void AddCspHeaders_CspDirectiveWithTwoSources_AddsCorrectlyFormattedCspHeader()
        {
            var cspConfig = new CspConfigurationElement { Enabled = true, DefaultSrc = { Self = true } };
            cspConfig.DefaultSrc.Sources.Add(new CspSourceConfigurationElement() { Source = "nwebsec.codeplex.com" });

            headerSetter.AddCspHeaders(cspConfig, false);

            mockResponse.Verify(x => x.AddHeader("Content-Security-Policy", "default-src 'self' nwebsec.codeplex.com"), Times.Once());
        }

        [Test]
        public void AddCspHeaders_CspEnabledWithBuiltinReportUri_AddsCorrectCspHeader()
        {
            var cspConfig = new CspConfigurationElement
            {
                Enabled = true,
                DefaultSrc = { Self = true },
                ReportUriDirective = { EnableBuiltinHandler = true }
            };

            headerSetter.AddCspHeaders(cspConfig, false);

            var expectedReportUri = AppPath + HttpHeaderSetter.BuiltInReportUriHandler;
            mockResponse.Verify(x => x.AddHeader("Content-Security-Policy", "default-src 'self'; report-uri " + expectedReportUri), Times.Once());
        }

        [Test]
        public void AddCspHeaders_CspEnabledWithCustomReportUri_AddsCorrectCspHeader()
        {
            var cspConfig = new CspConfigurationElement
            {
                Enabled = true,
                DefaultSrc = { Self = true },
                ReportUriDirective = { ReportUri = "/CspReport" }
            };

            headerSetter.AddCspHeaders(cspConfig, false);

            mockResponse.Verify(x => x.AddHeader("Content-Security-Policy", "default-src 'self'; report-uri /CspReport"), Times.Once());
        }

        [Test]
        public void AddCspHeaders_CspEnabledWithTwoReportUris_AddsCorrectCspHeader()
        {
            var cspConfig = new CspConfigurationElement
                                {
                                    Enabled = true,
                                    DefaultSrc = { Self = true },
                                    ReportUriDirective = { EnableBuiltinHandler = true, ReportUri = "/CspReport" }
                                };

            headerSetter.AddCspHeaders(cspConfig, false);

            var expectedReportUri = AppPath + HttpHeaderSetter.BuiltInReportUriHandler + " /CspReport";
            mockResponse.Verify(x => x.AddHeader("Content-Security-Policy", "default-src 'self'; report-uri " + expectedReportUri), Times.Once());
        }

    }
}
