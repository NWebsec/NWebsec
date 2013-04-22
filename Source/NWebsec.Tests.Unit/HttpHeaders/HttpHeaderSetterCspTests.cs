// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Web;
using Moq;
using NUnit.Framework;
using NWebsec.Csp;
using NWebsec.HttpHeaders;
using NWebsec.Modules.Configuration.Csp;

namespace NWebsec.Tests.Unit.HttpHeaders
{
    [TestFixture]
    public class HttpHeaderSetterCspTests
    {
        HttpHeaderSetter headerSetter;
        Mock<IHandlerTypeHelper> mockHandlerHelper;
        Mock<HttpRequestBase> mockRequest;
        Mock<HttpResponseBase> mockResponse;
        Mock<HttpContextBase> mockContext;
        private Mock<HttpCachePolicyBase> mockCachePolicy;
        private CspReportHelper cspReportHelper;

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
            mockHandlerHelper = new Mock<IHandlerTypeHelper>();

            var testUri = new Uri("http://localhost/NWebsecWebforms/");
            mockRequest.Setup(r => r.Url).Returns(testUri);
            mockRequest.Setup(r => r.ApplicationPath).Returns(AppPath);

            mockCachePolicy = new Mock<HttpCachePolicyBase>();
            mockResponse.Setup(x => x.Cache).Returns(mockCachePolicy.Object);

            mockContext.Setup(c => c.Request).Returns(mockRequest.Object);
            mockContext.Setup(c => c.Response).Returns(mockResponse.Object);
            
            var mockReportPathHelper = new Mock<ICspReportHandlerPathHelper>();
            mockReportPathHelper.Setup(p => p.GetBuiltinCspReportHandlerPath()).Returns("/NWebsec");
            cspReportHelper = new CspReportHelper(mockReportPathHelper.Object);

            headerSetter = new HttpHeaderSetter(mockHandlerHelper.Object, cspReportHelper);

        }

        [Test]
        public void AddCspHeaders_DisabledInConfig_DoesNotAddCspHeader()
        {
            var cspConfig = new CspConfigurationElement { Enabled = false, DefaultSrc = { Self = true } };

            headerSetter.AddCspHeaders(mockResponse.Object, cspConfig, false);

            mockResponse.Verify(x => x.AddHeader(It.IsAny<String>(), It.IsAny<String>()), Times.Never());
        }

        [Test]
        public void AddCspHeaders_DefaultConfig_DoesNotAddCspHeader()
        {
            var cspConfig = new CspConfigurationElement();

            headerSetter.AddCspHeaders(mockResponse.Object, cspConfig, false);

            mockResponse.Verify(x => x.AddHeader(It.IsAny<String>(), It.IsAny<String>()), Times.Never());
        }

        [Test]
        public void AddCspHeaders_EnabledButNoDirectivesOrReportUriEnabled_DoesNotAddCspHeader()
        {
            var cspConfig = new CspConfigurationElement { Enabled = true };

            headerSetter.AddCspHeaders(mockResponse.Object, cspConfig, false);

            mockResponse.Verify(x => x.AddHeader(It.IsAny<String>(), It.IsAny<String>()), Times.Never());
        }

        [Test]
        public void AddCspHeaders_EnabledButNoDirectivesEnabledAndReportUriEnabled_DoesNotAddCspHeader()
        {
            var cspConfig = new CspConfigurationElement { Enabled = true };
            cspConfig.ReportUriDirective.Enabled = true;
            cspConfig.ReportUriDirective.EnableBuiltinHandler = true;

            headerSetter.AddCspHeaders(mockResponse.Object, cspConfig, false);

            mockResponse.Verify(x => x.AddHeader(It.IsAny<String>(), It.IsAny<String>()), Times.Never());
        }

        [Test]
        public void AddCspHeaders_CspEnabledWithDefaultSrc_AddsCspHeaderWithDefaultSrc()
        {
            var cspConfig = new CspConfigurationElement
            {
                Enabled = true,
                DefaultSrc = { Self = true }
            };

            headerSetter.AddCspHeaders(mockResponse.Object, cspConfig, false);

            mockResponse.Verify(x => x.AddHeader("Content-Security-Policy", "default-src 'self'"), Times.Once());
        }

        [Test]
        public void AddCspHeaders_CspEnabledWithScriptSrc_AddsCspHeaderWithScriptSrc()
        {
            var cspConfig = new CspConfigurationElement
            {
                Enabled = true,
                ScriptSrc = { UnsafeEval = true }
            };

            headerSetter.AddCspHeaders(mockResponse.Object, cspConfig, false);

            mockResponse.Verify(x => x.AddHeader("Content-Security-Policy", "script-src 'unsafe-eval'"), Times.Once());
        }

        [Test]
        public void AddCspHeaders_CspEnabledWithObjectSrc_AddsCspHeaderWithObjectSrc()
        {
            var cspConfig = new CspConfigurationElement
            {
                Enabled = true,
                ObjectSrc = { Self = true }
            };

            headerSetter.AddCspHeaders(mockResponse.Object, cspConfig, false);

            mockResponse.Verify(x => x.AddHeader("Content-Security-Policy", "object-src 'self'"), Times.Once());
        }

        [Test]
        public void AddCspHeaders_CspEnabledWithStyleSrc_AddsCspHeaderWithStyleSrc()
        {
            var cspConfig = new CspConfigurationElement
            {
                Enabled = true,
                StyleSrc = { UnsafeInline = true }
            };

            headerSetter.AddCspHeaders(mockResponse.Object, cspConfig, false);

            mockResponse.Verify(x => x.AddHeader("Content-Security-Policy", "style-src 'unsafe-inline'"), Times.Once());
        }

        [Test]
        public void AddCspHeaders_CspEnabledWithImgSrc_AddsCspHeaderWithImgSrc()
        {
            var cspConfig = new CspConfigurationElement
            {
                Enabled = true,
                ImgSrc= { Self = true }
            };

            headerSetter.AddCspHeaders(mockResponse.Object, cspConfig, false);

            mockResponse.Verify(x => x.AddHeader("Content-Security-Policy", "img-src 'self'"), Times.Once());
        }

        [Test]
        public void AddCspHeaders_CspEnabledWithMediaSrc_AddsCspHeaderWithMediaSrc()
        {
            var cspConfig = new CspConfigurationElement
            {
                Enabled = true,
                MediaSrc = { Self = true }
            };

            headerSetter.AddCspHeaders(mockResponse.Object, cspConfig, false);

            mockResponse.Verify(x => x.AddHeader("Content-Security-Policy", "media-src 'self'"), Times.Once());
        }

        [Test]
        public void AddCspHeaders_CspEnabledWithFrameSrc_AddsCspHeaderWithFrameSrc()
        {
            var cspConfig = new CspConfigurationElement
            {
                Enabled = true,
                FrameSrc = { Self = true }
            };

            headerSetter.AddCspHeaders(mockResponse.Object, cspConfig, false);

            mockResponse.Verify(x => x.AddHeader("Content-Security-Policy", "frame-src 'self'"), Times.Once());
        }

        [Test]
        public void AddCspHeaders_CspEnabledWithFontSrc_AddsCspHeaderWithFontSrc()
        {
            var cspConfig = new CspConfigurationElement
            {
                Enabled = true,
                FontSrc = { Self = true }
            };

            headerSetter.AddCspHeaders(mockResponse.Object, cspConfig, false);

            mockResponse.Verify(x => x.AddHeader("Content-Security-Policy", "font-src 'self'"), Times.Once());
        }

        [Test]
        public void AddCspHeaders_CspEnabledWithConnectSrc_AddsCspHeaderWithConnectSrc()
        {
            var cspConfig = new CspConfigurationElement
            {
                Enabled = true,
                ConnectSrc = { Self = true }
            };

            headerSetter.AddCspHeaders(mockResponse.Object, cspConfig, false);

            mockResponse.Verify(x => x.AddHeader("Content-Security-Policy", "connect-src 'self'"), Times.Once());
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

            headerSetter.AddCspHeaders(mockResponse.Object, cspConfig, false);

            mockResponse.Verify(x => x.AddHeader("Content-Security-Policy", "default-src 'self'; script-src 'none'"), Times.Once());
        }

        [Test]
        public void AddCspHeaders_CspDirectiveWithTwoSources_AddsCorrectlyFormattedCspHeader()
        {
            var cspConfig = new CspConfigurationElement { Enabled = true, DefaultSrc = { Self = true } };
            cspConfig.DefaultSrc.Sources.Add(new CspSourceConfigurationElement() { Source = "nwebsec.codeplex.com" });

            headerSetter.AddCspHeaders(mockResponse.Object, cspConfig, false);

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

            headerSetter.AddCspHeaders(mockResponse.Object, cspConfig, false);

            var expectedReportUri = cspReportHelper.GetBuiltInCspReportHandlerRelativeUri();
            mockResponse.Verify(x => x.AddHeader("Content-Security-Policy", "default-src 'self'; report-uri " + expectedReportUri), Times.Once());
        }

        [Test]
        public void AddCspHeaders_CspEnabledWithCustomReportUri_AddsCorrectCspHeader()
        {
            var cspConfig = new CspConfigurationElement
            {
                Enabled = true,
                DefaultSrc = { Self = true }
            };
            cspConfig.ReportUriDirective.ReportUris.Add(new ReportUriConfigurationElement() { ReportUri = new Uri("/CspViolationReported", UriKind.Relative) });

            headerSetter.AddCspHeaders(mockResponse.Object, cspConfig, false);

            mockResponse.Verify(x => x.AddHeader("Content-Security-Policy", "default-src 'self'; report-uri /CspViolationReported"), Times.Once());
        }

        [Test]
        public void AddCspHeaders_CspEnabledWithTwoReportUris_AddsCorrectCspHeader()
        {
            var cspConfig = new CspConfigurationElement
                                {
                                    Enabled = true,
                                    DefaultSrc = { Self = true },
                                    ReportUriDirective = { EnableBuiltinHandler = true }
                                };
            cspConfig.ReportUriDirective.ReportUris.Add(new ReportUriConfigurationElement { ReportUri = new Uri("/CspViolationReported", UriKind.Relative) });

            headerSetter.AddCspHeaders(mockResponse.Object, cspConfig, false);

            var expectedReportUri = cspReportHelper.GetBuiltInCspReportHandlerRelativeUri() + " /CspViolationReported";
            mockResponse.Verify(x => x.AddHeader("Content-Security-Policy", "default-src 'self'; report-uri " + expectedReportUri), Times.Once());
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

            headerSetter.AddCspHeaders(mockResponse.Object, cspConfig, false);

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

            headerSetter.AddCspHeaders(mockResponse.Object, cspConfig, false);

            mockResponse.Verify(x => x.AddHeader("X-WebKit-CSP", It.IsAny<String>()), Times.Once());
        }

        [Test]
        public void AddCspHeaders_XCspAndXWebkitCspEnabledInConfig_AddsXcspAndXWebkitCspHeader()
        {
            var cspConfig = new CspConfigurationElement { Enabled = true, XContentSecurityPolicyHeader = true, XWebKitCspHeader = true };
            cspConfig.DefaultSrc.Self = true;

            headerSetter.AddCspHeaders(mockResponse.Object, cspConfig, false);

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

            headerSetter.AddCspHeaders(mockResponse.Object, cspConfig, reportOnly: true);

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

            headerSetter.AddCspHeaders(mockResponse.Object, cspConfig, true);

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

            headerSetter.AddCspHeaders(mockResponse.Object, cspConfig, true);

            mockResponse.Verify(x => x.AddHeader("X-Content-Security-Policy-Report-Only", It.IsAny<String>()), Times.Once());
            mockResponse.Verify(x => x.AddHeader("X-WebKit-CSP-Report-Only", It.IsAny<String>()), Times.Once());
        }

    }
}
