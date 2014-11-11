// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Web;
using Moq;
using NUnit.Framework;
using NWebsec.Core.HttpHeaders.Configuration;
using NWebsec.Mvc.Csp;
using NWebsec.Mvc.Helpers;
using NWebsec.Mvc.Tests.Unit.TestHelpers;

namespace NWebsec.Mvc.Tests.Unit.Helpers
{
    public class CspConfigurationOverrideHelperTests
    {
        protected HttpContextBase MockContext;
        protected CspConfigurationOverrideHelper CspConfigurationOverrideHelper;
        private Mock<IContextConfigurationHelper> _contextHelper;
        private Mock<ICspConfigMapper> _directiveConfigMapper;
        private Mock<ICspDirectiveOverrideHelper> _directiveOverrideHelper;

        [SetUp]
        public void Setup()
        {
            MockContext = new Mock<HttpContextBase>().Object;

            _contextHelper = new Mock<IContextConfigurationHelper>(MockBehavior.Strict);
            _directiveConfigMapper = new Mock<ICspConfigMapper>(MockBehavior.Strict);
            _directiveOverrideHelper = new Mock<ICspDirectiveOverrideHelper>(MockBehavior.Strict);

            CspConfigurationOverrideHelper = new CspConfigurationOverrideHelper(_contextHelper.Object, _directiveConfigMapper.Object, _directiveOverrideHelper.Object);
        }

        [Test]
        public void GetCspElementWithOverrides_NoOverride_ReturnsNull([Values(false, true)]bool reportOnly)
        {
            _contextHelper.Setup(h => h.GetCspConfigurationOverride(It.IsAny<HttpContextBase>(), reportOnly, true)).Returns((CspOverrideConfiguration)null);

            var overrideConfig = CspConfigurationOverrideHelper.GetCspConfigWithOverrides(MockContext, reportOnly);

            Assert.IsNull(overrideConfig);
        }

        [Test]
        public void GetCspElementWithOverrides_OverrideAndConfigFromContext_MergesConfigFromContextAndOverrides([Values(false, true)]bool reportOnly)
        {
            var cspConfig = new CspConfiguration();
            var overrideConfig = new CspOverrideConfiguration();
            _contextHelper.Setup(h => h.GetCspConfiguration(It.IsAny<HttpContextBase>(), reportOnly)).Returns(cspConfig);
            _contextHelper.Setup(h => h.GetCspConfigurationOverride(It.IsAny<HttpContextBase>(), reportOnly, true)).Returns(overrideConfig);
            _directiveConfigMapper.Setup(m => m.MergeConfiguration(cspConfig, It.IsAny<ICspConfiguration>()));
            _directiveConfigMapper.Setup(m => m.MergeOverrides(overrideConfig, It.IsAny<ICspConfiguration>()));

            var overrideElement = CspConfigurationOverrideHelper.GetCspConfigWithOverrides(MockContext, reportOnly);

            Assert.IsNotNull(overrideElement);
            _directiveConfigMapper.Verify(m => m.MergeConfiguration(cspConfig, It.IsAny<ICspConfiguration>()), Times.Once);
            _directiveConfigMapper.Verify(m => m.MergeOverrides(overrideConfig, It.IsAny<ICspConfiguration>()), Times.Once);
        }


        [Test]
        public void GetCspElementWithOverrides_OverrideAndNoConfigFromContext_MergesOverrides([Values(false, true)]bool reportOnly)
        {
            var overrideConfig = new CspOverrideConfiguration();
            _contextHelper.Setup(h => h.GetCspConfiguration(It.IsAny<HttpContextBase>(), reportOnly)).Returns((ICspConfiguration)null);
            _contextHelper.Setup(h => h.GetCspConfigurationOverride(It.IsAny<HttpContextBase>(), reportOnly, true)).Returns(overrideConfig);
            _directiveConfigMapper.Setup(m => m.MergeOverrides(overrideConfig, It.IsAny<ICspConfiguration>()));

            var overrideElement = CspConfigurationOverrideHelper.GetCspConfigWithOverrides(MockContext, reportOnly);

            Assert.IsNotNull(overrideElement);
            _directiveConfigMapper.Verify(m => m.MergeOverrides(overrideConfig, It.IsAny<ICspConfiguration>()), Times.Once);
        }

        [Test]
        public void SetCspHeaderOverride_OverridesWithHeaderEnabled_HeaderEnabled([Values(false, true)]bool reportOnly)
        {
            var overrideConfig = new CspOverrideConfiguration();
            _contextHelper.Setup(h => h.GetCspConfigurationOverride(It.IsAny<HttpContextBase>(), reportOnly, false)).Returns(overrideConfig);
            var cspOverride = new CspHeaderConfiguration { Enabled = true };

            CspConfigurationOverrideHelper.SetCspHeaderOverride(MockContext, cspOverride, reportOnly);

            Assert.IsTrue(overrideConfig.EnabledOverride);
            Assert.IsTrue(overrideConfig.Enabled);
        }


        [Test]
        public void SetCspReportUriOverride_ReportUriDisabledAndOverridden_ReturnsOverridenReportUri([Values(false, true)]bool reportOnly)
        {
            var overrideConfig = new CspOverrideConfiguration();
            _contextHelper.Setup(h => h.GetCspConfigurationOverride(It.IsAny<HttpContextBase>(), reportOnly, false)).Returns(overrideConfig);
            var reportUri = new CspReportUriDirectiveConfiguration { Enabled = true, EnableBuiltinHandler = true };

            CspConfigurationOverrideHelper.SetCspReportUriOverride(MockContext, reportUri, reportOnly);

            Assert.IsTrue(overrideConfig.ReportUriDirective.Enabled);
            Assert.IsTrue(overrideConfig.ReportUriDirective.EnableBuiltinHandler);
        }

        [Test]
        public void SetCspDirectiveOverride_NoCurrentOverride_ClonesConfigFromContextAndOverrides([Values(false, true)]bool reportOnly,
            [ValueSource(typeof(CspCommonDirectives), "Directives")] CspDirectives directive)
        {

            var contextConfig = new CspConfiguration();
            var overrideConfig = new CspOverrideConfiguration();
            //Returns CSP config from context
            _contextHelper.Setup(h => h.GetCspConfiguration(It.IsAny<HttpContextBase>(), reportOnly)).Returns(contextConfig);
            _contextHelper.Setup(h => h.GetCspConfigurationOverride(It.IsAny<HttpContextBase>(), reportOnly, false)).Returns(overrideConfig);
            //There's no override for directive
            _directiveConfigMapper.Setup(m => m.GetCspDirectiveConfig(overrideConfig, directive)).Returns((ICspDirectiveConfiguration)null);
            //Returns cloned directive config from context config
            var clonedContextDirective = new CspDirectiveConfiguration();
            _directiveConfigMapper.Setup(m => m.GetCspDirectiveConfigCloned(contextConfig, directive)).Returns(clonedContextDirective);
            //We need an override and a result.
            var directiveOverride = new CspDirectiveOverride();
            var directiveOverrideResult = new CspDirectiveConfiguration();
            _directiveOverrideHelper.Setup(h => h.GetOverridenCspDirectiveConfig(directiveOverride, clonedContextDirective)).Returns(directiveOverrideResult);
            //This should be called at the very end
            _directiveConfigMapper.Setup(m => m.SetCspDirectiveConfig(overrideConfig, directive, directiveOverrideResult));

            CspConfigurationOverrideHelper.SetCspDirectiveOverride(MockContext, directive, directiveOverride, reportOnly);

            //Verify that the override result was set on the override config.
            _directiveConfigMapper.Verify(m => m.SetCspDirectiveConfig(overrideConfig, directive, directiveOverrideResult), Times.Once);
        }

        [Test]
        public void SetCspDirectiveOverride_HasOverride_OverridesExistingOverride([Values(false, true)]bool reportOnly,
            [ValueSource(typeof(CspCommonDirectives), "Directives")] CspDirectives directive)
        {

            var overrideConfig = new CspOverrideConfiguration();
            _contextHelper.Setup(h => h.GetCspConfigurationOverride(It.IsAny<HttpContextBase>(), reportOnly, false)).Returns(overrideConfig);
            //There's an override for directive
            var currentDirectiveOverride = new CspDirectiveConfiguration();
            _directiveConfigMapper.Setup(m => m.GetCspDirectiveConfig(overrideConfig, directive)).Returns(currentDirectiveOverride);
            //We need an override and a result.
            var directiveOverride = new CspDirectiveOverride();
            var directiveOverrideResult = new CspDirectiveConfiguration();
            _directiveOverrideHelper.Setup(h => h.GetOverridenCspDirectiveConfig(directiveOverride, currentDirectiveOverride)).Returns(directiveOverrideResult);
            //This should be called at the very end
            _directiveConfigMapper.Setup(m => m.SetCspDirectiveConfig(overrideConfig, directive, directiveOverrideResult));

            CspConfigurationOverrideHelper.SetCspDirectiveOverride(MockContext, directive, directiveOverride, reportOnly);

            //Verify that the override result was set on the override config.
            _directiveConfigMapper.Verify(m => m.SetCspDirectiveConfig(overrideConfig, directive, directiveOverrideResult), Times.Once);
        }

        [Test]
        public void GetCspScriptNonce_ScriptNonceRequested_SetsNonceOnBothConfigs()
        {
            var overrideConfig = new CspOverrideConfiguration();
            var overrideConfigReportOnly = new CspOverrideConfiguration();
            _contextHelper.Setup(h => h.GetCspConfigurationOverride(It.IsAny<HttpContextBase>(), false, false)).Returns(overrideConfig);
            _contextHelper.Setup(h => h.GetCspConfigurationOverride(It.IsAny<HttpContextBase>(), true, false)).Returns(overrideConfigReportOnly);
            
            var nonce = CspConfigurationOverrideHelper.GetCspScriptNonce(MockContext);

            Assert.IsNotNull(overrideConfig.ScriptSrcDirective);
            Assert.IsNotNull(overrideConfigReportOnly.ScriptSrcDirective);
            Assert.AreEqual(nonce, overrideConfig.ScriptSrcDirective.Nonce);
            Assert.AreEqual(nonce, overrideConfigReportOnly.ScriptSrcDirective.Nonce);
        }

        [Test]
        public void GetCspStyleNonce_StyleNonceRequested_SetsNonceForBothConfigs()
        {
            var overrideConfig = new CspOverrideConfiguration();
            var overrideConfigReportOnly = new CspOverrideConfiguration();
            _contextHelper.Setup(h => h.GetCspConfigurationOverride(It.IsAny<HttpContextBase>(), false, false)).Returns(overrideConfig);
            _contextHelper.Setup(h => h.GetCspConfigurationOverride(It.IsAny<HttpContextBase>(), true, false)).Returns(overrideConfigReportOnly);

            var nonce = CspConfigurationOverrideHelper.GetCspStyleNonce(MockContext);

            Assert.IsNotNull(overrideConfig.StyleSrcDirective);
            Assert.IsNotNull(overrideConfigReportOnly.StyleSrcDirective);
            Assert.AreEqual(nonce, overrideConfig.StyleSrcDirective.Nonce);
            Assert.AreEqual(nonce, overrideConfigReportOnly.StyleSrcDirective.Nonce);
        }

        
        [Test]
        public void GetCspScriptNonce_NonceRequested_ReturnsSameNonceMultipleTimes()
        {
            var overrideConfig = new CspOverrideConfiguration();
            var overrideConfigReportOnly = new CspOverrideConfiguration();
            _contextHelper.Setup(h => h.GetCspConfigurationOverride(It.IsAny<HttpContextBase>(), false, false)).Returns(overrideConfig);
            _contextHelper.Setup(h => h.GetCspConfigurationOverride(It.IsAny<HttpContextBase>(), true, false)).Returns(overrideConfigReportOnly);

            var nonce = CspConfigurationOverrideHelper.GetCspScriptNonce(MockContext);

            Assert.IsNotNullOrEmpty(nonce);

            var secondNonce = CspConfigurationOverrideHelper.GetCspScriptNonce(MockContext);

            Assert.AreEqual(nonce, secondNonce);
        }

        [Test]
        public void GetCspStyleNonce_CspNonceRequested_ReturnsSameNonceMultipleTimes()
        {
            var overrideConfig = new CspOverrideConfiguration();
            var overrideConfigReportOnly = new CspOverrideConfiguration();
            _contextHelper.Setup(h => h.GetCspConfigurationOverride(It.IsAny<HttpContextBase>(), false, false)).Returns(overrideConfig);
            _contextHelper.Setup(h => h.GetCspConfigurationOverride(It.IsAny<HttpContextBase>(), true, false)).Returns(overrideConfigReportOnly);

            var nonce = CspConfigurationOverrideHelper.GetCspStyleNonce(MockContext);

            Assert.IsNotNullOrEmpty(nonce);

            var secondNonce = CspConfigurationOverrideHelper.GetCspStyleNonce(MockContext);

            Assert.AreEqual(nonce, secondNonce);
        }
    }
}