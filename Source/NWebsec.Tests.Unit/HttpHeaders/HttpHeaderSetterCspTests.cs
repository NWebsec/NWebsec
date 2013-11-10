// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Collections.Specialized;
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
        HttpHeaderSetter _headerSetter;
        Mock<IHandlerTypeHelper> _mockHandlerHelper;
        Mock<HttpRequestBase> _mockRequest;
        Mock<HttpResponseBase> _mockResponse;
        Mock<HttpContextBase> _mockContext;
        private Mock<HttpCachePolicyBase> _mockCachePolicy;
        private CspReportHelper _cspReportHelper;
        private NameValueCollection _responseHeaders;

        private const string AppPath = "/MyApp";

        [SetUp]
        public void HeaderModuleTestInitialize()
        {

            var testUri = new Uri("http://localhost/NWebsecWebforms/");
            _mockRequest = new Mock<HttpRequestBase>();
            _mockRequest.SetupAllProperties();
            _mockRequest.Setup(r => r.Url).Returns(testUri);
            _mockRequest.Setup(r => r.ApplicationPath).Returns(AppPath);

            _mockCachePolicy = new Mock<HttpCachePolicyBase>();
            _responseHeaders = new NameValueCollection();
            _mockResponse = new Mock<HttpResponseBase>();
            _mockResponse.SetupAllProperties();
            _mockResponse.Setup(x => x.Cache).Returns(_mockCachePolicy.Object);
            _mockResponse.Setup(r => r.Headers).Returns(_responseHeaders);

            _mockContext = new Mock<HttpContextBase>();
            _mockContext.SetupAllProperties();

            _mockContext.Setup(c => c.Request).Returns(_mockRequest.Object);
            _mockContext.Setup(c => c.Response).Returns(_mockResponse.Object);

            var mockReportPathHelper = new Mock<ICspReportHandlerPathHelper>();
            mockReportPathHelper.Setup(p => p.GetBuiltinCspReportHandlerPath()).Returns("/NWebsec");
            _cspReportHelper = new CspReportHelper(mockReportPathHelper.Object);

            _mockHandlerHelper = new Mock<IHandlerTypeHelper>();
            _headerSetter = new HttpHeaderSetter(_mockHandlerHelper.Object, _cspReportHelper);

        }

        [Test]
        public void AddCspHeaders_DisabledInConfig_DoesNotAddCspHeader()
        {
            var cspConfig = new CspConfigurationElement { Enabled = false, DefaultSrc = { Self = true } };

            _headerSetter.SetCspHeaders(_mockContext.Object, cspConfig, false);

            Assert.IsEmpty(_responseHeaders);
        }

        [Test]
        public void AddCspHeaders_EnabledInConfigAndRedirect_DoesNotAddCspHeader()
        {
            var cspConfig = new CspConfigurationElement { Enabled = true, DefaultSrc = { Self = true } };
            _mockResponse.Setup(r => r.StatusCode).Returns(302);

            _headerSetter.SetCspHeaders(_mockContext.Object, cspConfig, false);

            Assert.IsEmpty(_responseHeaders);
        }

        [Test]
        public void AddCspHeaders_EnabledInConfigAndStaticContentHandler_DoesNotAddCspHeader()
        {
            var cspConfig = new CspConfigurationElement { Enabled = true, DefaultSrc = { Self = true } };
            _mockHandlerHelper.Setup(h => h.IsStaticContentHandler(It.IsAny<HttpContextBase>())).Returns(true);

            _headerSetter.SetCspHeaders(_mockContext.Object, cspConfig, false);

            Assert.IsEmpty(_responseHeaders);
        }

        [Test]
        public void AddCspHeaders_EnabledInConfigAndUnmanagedHandler_DoesNotAddCspHeader()
        {
            var cspConfig = new CspConfigurationElement { Enabled = true, DefaultSrc = { Self = true } };
            _mockHandlerHelper.Setup(h => h.IsUnmanagedHandler(It.IsAny<HttpContextBase>())).Returns(true);

            _headerSetter.SetCspHeaders(_mockContext.Object, cspConfig, false);

            Assert.IsEmpty(_responseHeaders);
        }

        [Test]
        public void AddCspHeaders_DefaultConfig_DoesNotAddCspHeader()
        {
            var cspConfig = new CspConfigurationElement();

            _headerSetter.SetCspHeaders(_mockContext.Object, cspConfig, false);

            Assert.IsEmpty(_responseHeaders);
        }

        [Test]
        public void AddCspHeaders_EnabledButNoDirectivesOrReportUriEnabled_DoesNotAddCspHeader()
        {
            var cspConfig = new CspConfigurationElement { Enabled = true };

            _headerSetter.SetCspHeaders(_mockContext.Object, cspConfig, false);

            Assert.IsEmpty(_responseHeaders);
        }

        [Test]
        public void AddCspHeaders_EnabledButNoDirectivesEnabledAndReportUriEnabled_DoesNotAddCspHeader()
        {
            var cspConfig = new CspConfigurationElement { Enabled = true };
            cspConfig.ReportUriDirective.Enabled = true;
            cspConfig.ReportUriDirective.EnableBuiltinHandler = true;

            _headerSetter.SetCspHeaders(_mockContext.Object, cspConfig, false);

            Assert.IsEmpty(_responseHeaders);
        }

        [Test]
        public void AddCspHeaders_CspEnabledWithDefaultSrc_AddsCspHeaderWithDefaultSrc()
        {
            var cspConfig = new CspConfigurationElement
            {
                Enabled = true,
                DefaultSrc = { Self = true }
            };

            _headerSetter.SetCspHeaders(_mockContext.Object, cspConfig, false);

            Assert.AreEqual("default-src 'self'", _responseHeaders["Content-Security-Policy"]);
        }

        [Test]
        public void AddCspHeaders_CspEnabledWithScriptSrc_AddsCspHeaderWithScriptSrc()
        {
            var cspConfig = new CspConfigurationElement
            {
                Enabled = true,
                ScriptSrc = { UnsafeEval = true }
            };

            _headerSetter.SetCspHeaders(_mockContext.Object, cspConfig, false);

            Assert.AreEqual("script-src 'unsafe-eval'", _responseHeaders["Content-Security-Policy"]);
        }

        [Test]
        public void AddCspHeaders_CspEnabledWithObjectSrc_AddsCspHeaderWithObjectSrc()
        {
            var cspConfig = new CspConfigurationElement
            {
                Enabled = true,
                ObjectSrc = { Self = true }
            };

            _headerSetter.SetCspHeaders(_mockContext.Object, cspConfig, false);

            Assert.AreEqual("object-src 'self'", _responseHeaders["Content-Security-Policy"]);
        }

        [Test]
        public void AddCspHeaders_CspEnabledWithStyleSrc_AddsCspHeaderWithStyleSrc()
        {
            var cspConfig = new CspConfigurationElement
            {
                Enabled = true,
                StyleSrc = { UnsafeInline = true }
            };

            _headerSetter.SetCspHeaders(_mockContext.Object, cspConfig, false);

            Assert.AreEqual("style-src 'unsafe-inline'", _responseHeaders["Content-Security-Policy"]);
        }

        [Test]
        public void AddCspHeaders_CspEnabledWithImgSrc_AddsCspHeaderWithImgSrc()
        {
            var cspConfig = new CspConfigurationElement
            {
                Enabled = true,
                ImgSrc = { Self = true }
            };

            _headerSetter.SetCspHeaders(_mockContext.Object, cspConfig, false);

            Assert.AreEqual("img-src 'self'", _responseHeaders["Content-Security-Policy"]);
        }

        [Test]
        public void AddCspHeaders_CspEnabledWithMediaSrc_AddsCspHeaderWithMediaSrc()
        {
            var cspConfig = new CspConfigurationElement
            {
                Enabled = true,
                MediaSrc = { Self = true }
            };

            _headerSetter.SetCspHeaders(_mockContext.Object, cspConfig, false);

            Assert.AreEqual("media-src 'self'", _responseHeaders["Content-Security-Policy"]);
        }

        [Test]
        public void AddCspHeaders_CspEnabledWithFrameSrc_AddsCspHeaderWithFrameSrc()
        {
            var cspConfig = new CspConfigurationElement
            {
                Enabled = true,
                FrameSrc = { Self = true }
            };

            _headerSetter.SetCspHeaders(_mockContext.Object, cspConfig, false);

            Assert.AreEqual("frame-src 'self'", _responseHeaders["Content-Security-Policy"]);
        }

        [Test]
        public void AddCspHeaders_CspEnabledWithFontSrc_AddsCspHeaderWithFontSrc()
        {
            var cspConfig = new CspConfigurationElement
            {
                Enabled = true,
                FontSrc = { Self = true }
            };

            _headerSetter.SetCspHeaders(_mockContext.Object, cspConfig, false);

            Assert.AreEqual("font-src 'self'", _responseHeaders["Content-Security-Policy"]);
        }

        [Test]
        public void AddCspHeaders_CspEnabledWithConnectSrc_AddsCspHeaderWithConnectSrc()
        {
            var cspConfig = new CspConfigurationElement
            {
                Enabled = true,
                ConnectSrc = { Self = true }
            };

            _headerSetter.SetCspHeaders(_mockContext.Object, cspConfig, false);

            Assert.AreEqual("connect-src 'self'", _responseHeaders["Content-Security-Policy"]);
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

            _headerSetter.SetCspHeaders(_mockContext.Object, cspConfig, false);

            Assert.AreEqual("default-src 'self'; script-src 'none'", _responseHeaders["Content-Security-Policy"]);
        }

        [Test]
        public void AddCspHeaders_CspDirectiveWithTwoSources_AddsCorrectlyFormattedCspHeader()
        {
            var cspConfig = new CspConfigurationElement { Enabled = true, DefaultSrc = { Self = true } };
            cspConfig.DefaultSrc.Sources.Add(new CspSourceConfigurationElement { Source = "nwebsec.codeplex.com" });

            _headerSetter.SetCspHeaders(_mockContext.Object, cspConfig, false);

            Assert.AreEqual("default-src 'self' nwebsec.codeplex.com", _responseHeaders["Content-Security-Policy"]);
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

            _headerSetter.SetCspHeaders(_mockContext.Object, cspConfig, false);

            var expectedReportUri = _cspReportHelper.GetBuiltInCspReportHandlerRelativeUri();
            Assert.AreEqual("default-src 'self'; report-uri " + expectedReportUri, _responseHeaders["Content-Security-Policy"]);
        }

        [Test]
        public void AddCspHeaders_CspEnabledWithCustomReportUri_AddsCorrectCspHeader()
        {
            var cspConfig = new CspConfigurationElement
            {
                Enabled = true,
                DefaultSrc = { Self = true }
            };
            cspConfig.ReportUriDirective.ReportUris.Add(new ReportUriConfigurationElement { ReportUri = new Uri("/CspViolationReported", UriKind.Relative) });

            _headerSetter.SetCspHeaders(_mockContext.Object, cspConfig, false);

            Assert.AreEqual("default-src 'self'; report-uri /CspViolationReported", _responseHeaders["Content-Security-Policy"]);
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

            _headerSetter.SetCspHeaders(_mockContext.Object, cspConfig, false);

            var expectedReportUri = _cspReportHelper.GetBuiltInCspReportHandlerRelativeUri() + " /CspViolationReported";
            Assert.AreEqual("default-src 'self'; report-uri " + expectedReportUri, _responseHeaders["Content-Security-Policy"]);
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

            _headerSetter.SetCspHeaders(_mockContext.Object, cspConfig, false);

            Assert.IsNotNullOrEmpty(_responseHeaders["X-Content-Security-Policy"]);
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

            _headerSetter.SetCspHeaders(_mockContext.Object, cspConfig, false);

            Assert.IsNotNullOrEmpty(_responseHeaders["X-WebKit-CSP"]);
        }

        [Test]
        public void AddCspHeaders_XWebkitCspEnabledInConfigSafari5_DoesNotAddXWebkitCspHeader()
        {
            _mockRequest.Setup(r => r.UserAgent).Returns("Mozilla/5.0 (Macintosh; Intel Mac OS X 10_6_8) AppleWebKit/537.13+ (KHTML, like Gecko) Version/5.1.7 Safari/534.57.2");

            var cspConfig = new CspConfigurationElement
            {
                Enabled = true,
                XContentSecurityPolicyHeader = false,
                XWebKitCspHeader = true,
                DefaultSrc = { Self = true }
            };

            _headerSetter.SetCspHeaders(_mockContext.Object, cspConfig, false);

            Assert.IsEmpty(_responseHeaders);
        }

        [Test]
        public void AddCspHeaders_XCspAndXWebkitCspEnabledInConfig_AddsXcspAndXWebkitCspHeader()
        {
            var cspConfig = new CspConfigurationElement { Enabled = true, XContentSecurityPolicyHeader = true, XWebKitCspHeader = true };
            cspConfig.DefaultSrc.Self = true;

            _headerSetter.SetCspHeaders(_mockContext.Object, cspConfig, false);

            Assert.IsNotNullOrEmpty(_responseHeaders["X-Content-Security-Policy"]);
            Assert.IsNotNullOrEmpty(_responseHeaders["X-WebKit-CSP"]);
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

            _headerSetter.SetCspHeaders(_mockContext.Object, cspConfig, reportOnly: true);

            Assert.IsNotNullOrEmpty(_responseHeaders["X-Content-Security-Policy-Report-Only"]);
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

            _headerSetter.SetCspHeaders(_mockContext.Object, cspConfig, true);

            Assert.IsNotNullOrEmpty(_responseHeaders["X-WebKit-CSP-Report-Only"]);
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

            _headerSetter.SetCspHeaders(_mockContext.Object, cspConfig, true);

            Assert.IsNotNullOrEmpty(_responseHeaders["X-Content-Security-Policy-Report-Only"]);
            Assert.IsNotNullOrEmpty(_responseHeaders["X-WebKit-CSP-Report-Only"]);
        }
    }
}
