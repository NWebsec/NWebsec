// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Web;
using Moq;
using NUnit.Framework;
using NWebsec.Core;
using NWebsec.Core.HttpHeaders.Configuration;
using NWebsec.Mvc.Helpers;

namespace NWebsec.Mvc.Tests.Unit.Helpers
{
    [TestFixture]
    public class ContextConfigurationHelperTests
    {
        private NWebsecContext _systemWebContext;
        private NWebsecContext _owinContext;
        private HttpContextBase _mockContext;
        private ContextConfigurationHelper _contextHelper;

        [SetUp]
        public void Setup()
        {
            _systemWebContext = new NWebsecContext();
            _owinContext = new NWebsecContext();
            
            var mockContext = new Mock<HttpContextBase>();
            mockContext.Setup(c => c.Items["nwebsec.Context"]).Returns(_systemWebContext);

            _mockContext = mockContext.Object;

            _contextHelper = new ContextConfigurationHelper();
        }

        private void SetupOwinContext()
        {
            var owinEnv = new Dictionary<string, object>();
            owinEnv["nwebsec.Context"] = _owinContext;
           Mock.Get(_mockContext).Setup(c => c.Items["owin.Environment"]).Returns(owinEnv);

        }

        [Test]
        public void GetXRobotsTagConfiguration_NoOwinContext_ReturnsSystemWebConfig()
        {
            var config = new XRobotsTagConfiguration();
            _systemWebContext.XRobotsTag = config;

            var result = _contextHelper.GetXRobotsTagConfiguration(_mockContext);

            Assert.AreSame(config, result);
        }

        [Test]
        public void GetXRobotsTagConfiguration_OwinContextWithoutConfig_ReturnsSystemWebConfig()
        {
            SetupOwinContext();
            var config = new XRobotsTagConfiguration();
            _systemWebContext.XRobotsTag = config;

            var result = _contextHelper.GetXRobotsTagConfiguration(_mockContext);

            Assert.AreSame(config, result);
        }

        [Test]
        public void GetXRobotsTagConfiguration_HasOwinConfig_ReturnsOwinConfig()
        {
            SetupOwinContext();
            var config = new XRobotsTagConfiguration();
            _owinContext.XRobotsTag = config;

            var result = _contextHelper.GetXRobotsTagConfiguration(_mockContext);

            Assert.AreSame(config, result);
        }

        [Test]
        public void GetXFrameOptionsConfiguration_NoOwinContext_ReturnsSystemWebConfig()
        {
            var config = new XFrameOptionsConfiguration();
            _systemWebContext.XFrameOptions = config;

            var result = _contextHelper.GetXFrameOptionsConfiguration(_mockContext);

            Assert.AreSame(config, result);
        }

        [Test]
        public void GetXFrameOptionsConfiguration_OwinContextWithoutConfig_ReturnsSystemWebConfig()
        {
            SetupOwinContext();
            var config = new XFrameOptionsConfiguration();
            _systemWebContext.XFrameOptions = config;

            var result = _contextHelper.GetXFrameOptionsConfiguration(_mockContext);

            Assert.AreSame(config, result);
        }

        [Test]
        public void GetXFrameOptionsConfiguration_HasOwinConfig_ReturnsOwinConfig()
        {
            SetupOwinContext();
            var config = new XFrameOptionsConfiguration();
            _owinContext.XFrameOptions = config;

            var result = _contextHelper.GetXFrameOptionsConfiguration(_mockContext);

            Assert.AreSame(config, result);
        }

        [Test]
        public void GetXContentTypeOptionsConfiguration_NoOwinContext_ReturnsSystemWebConfig()
        {
            var config = new SimpleBooleanConfiguration();
            _systemWebContext.XContentTypeOptions = config;

            var result = _contextHelper.GetXContentTypeOptionsConfiguration(_mockContext);

            Assert.AreSame(config, result);
        }

        [Test]
        public void GetXContentTypeOptionsConfiguration_OwinContextWithoutConfig_ReturnsSystemWebConfig()
        {
            SetupOwinContext();
            var config = new SimpleBooleanConfiguration();
            _systemWebContext.XContentTypeOptions = config;

            var result = _contextHelper.GetXContentTypeOptionsConfiguration(_mockContext);

            Assert.AreSame(config, result);
        }

        [Test]
        public void GetXContentTypeOptionsConfiguration_HasOwinConfig_ReturnsOwinConfig()
        {
            SetupOwinContext();
            var config = new SimpleBooleanConfiguration();
            _owinContext.XContentTypeOptions = config;

            var result = _contextHelper.GetXContentTypeOptionsConfiguration(_mockContext);

            Assert.AreSame(config, result);
        }
        
        [Test]
        public void GetXDownloadOptionsConfiguration_NoOwinContext_ReturnsSystemWebConfig()
        {
            var config = new SimpleBooleanConfiguration();
            _systemWebContext.XDownloadOptions = config;

            var result = _contextHelper.GetXDownloadOptionsConfiguration(_mockContext);

            Assert.AreSame(config, result);
        }

        [Test]
        public void GetXDownloadOptionsConfiguration_OwinContextWithoutConfig_ReturnsSystemWebConfig()
        {
            SetupOwinContext();
            var config = new SimpleBooleanConfiguration();
            _systemWebContext.XDownloadOptions = config;

            var result = _contextHelper.GetXDownloadOptionsConfiguration(_mockContext);

            Assert.AreSame(config, result);
        }

        [Test]
        public void GetXDownloadOptionsConfiguration_HasOwinConfig_ReturnsOwinConfig()
        {
            SetupOwinContext();
            var config = new SimpleBooleanConfiguration();
            _owinContext.XDownloadOptions = config;

            var result = _contextHelper.GetXDownloadOptionsConfiguration(_mockContext);

            Assert.AreSame(config, result);
        }

        [Test]
        public void GetXXssProtectionConfiguration_NoOwinContext_ReturnsSystemWebConfig()
        {
            var config = new XXssProtectionConfiguration();
            _systemWebContext.XXssProtection = config;

            var result = _contextHelper.GetXXssProtectionConfiguration(_mockContext);

            Assert.AreSame(config, result);
        }

        [Test]
        public void GetXXssProtectionConfiguration_OwinContextWithoutConfig_ReturnsSystemWebConfig()
        {
            SetupOwinContext();
            var config = new XXssProtectionConfiguration();
            _systemWebContext.XXssProtection = config;

            var result = _contextHelper.GetXXssProtectionConfiguration(_mockContext);

            Assert.AreSame(config, result);
        }

        [Test]
        public void GetXXssProtectionConfiguration_HasOwinConfig_ReturnsOwinConfig()
        {
            SetupOwinContext();
            var config = new XXssProtectionConfiguration();
            _owinContext.XXssProtection = config;

            var result = _contextHelper.GetXXssProtectionConfiguration(_mockContext);

            Assert.AreSame(config, result);
        }

        [Test]
        public void GetCspConfiguration_NoOwinContext_ReturnsSystemWebConfig()
        {
            var config = new CspConfiguration();
            _systemWebContext.Csp = config;

            var result = _contextHelper.GetCspConfiguration(_mockContext);

            Assert.AreSame(config, result);
        }

        [Test]
        public void GetCspConfiguration_OwinContextWithoutConfig_ReturnsSystemWebConfig()
        {
            SetupOwinContext();
            var config = new CspConfiguration();
            _systemWebContext.Csp = config;

            var result = _contextHelper.GetCspConfiguration(_mockContext);

            Assert.AreSame(config, result);
        }

        [Test]
        public void GetCspConfiguration_HasOwinConfig_ReturnsOwinConfig()
        {
            SetupOwinContext();
            var config = new CspConfiguration();
            _owinContext.Csp = config;

            var result = _contextHelper.GetCspConfiguration(_mockContext);

            Assert.AreSame(config, result);
        }
         
        [Test]
        public void GetCspReportonlyConfiguration_NoOwinContext_ReturnsSystemWebConfig()
        {
            var config = new CspConfiguration();
            _systemWebContext.CspReportOnly = config;

            var result = _contextHelper.GetCspReportonlyConfiguration(_mockContext);

            Assert.AreSame(config, result);
        }

        [Test]
        public void GetCspReportonlyConfiguration_OwinContextWithoutConfig_ReturnsSystemWebConfig()
        {
            SetupOwinContext();
            var config = new CspConfiguration();
            _systemWebContext.CspReportOnly = config;

            var result = _contextHelper.GetCspReportonlyConfiguration(_mockContext);

            Assert.AreSame(config, result);
        }

        [Test]
        public void GetCspReportonlyConfiguration_HasOwinConfig_ReturnsOwinConfig()
        {
            SetupOwinContext();
            var config = new CspConfiguration();
            _owinContext.CspReportOnly = config;

            var result = _contextHelper.GetCspReportonlyConfiguration(_mockContext);

            Assert.AreSame(config, result);
        }
    }
}