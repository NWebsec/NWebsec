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
        public void SetCspSandboxOverride_NoCurrentOverride_ClonesConfigFromContextAndOverrides([Values(false, true)]bool reportOnly)
        {

            var contextConfig = new CspConfiguration();
            var overrideConfig = new CspOverrideConfiguration();
            //Returns CSP config from context
            _contextHelper.Setup(h => h.GetCspConfiguration(It.IsAny<HttpContextBase>(), reportOnly)).Returns(contextConfig);
            _contextHelper.Setup(h => h.GetCspConfigurationOverride(It.IsAny<HttpContextBase>(), reportOnly, false)).Returns(overrideConfig);
            //There's no override for directive
            //_directiveConfigMapper.Setup(m => m.GetCspDirectiveConfig(overrideConfig, directive)).Returns((ICspDirectiveConfiguration)null);
            //Returns cloned directive config from context config
            var clonedContextDirective = new CspSandboxDirectiveConfiguration();
            _directiveConfigMapper.Setup(m => m.GetCspSandboxConfigCloned(contextConfig)).Returns(clonedContextDirective);
            //We need an override and a result.
            var directiveOverride = new CspSandboxOverride();
            var directiveOverrideResult = new CspSandboxDirectiveConfiguration();
            _directiveOverrideHelper.Setup(h => h.GetOverridenCspSandboxConfig(directiveOverride, clonedContextDirective)).Returns(directiveOverrideResult);
            //This should be called at the very end
            //_directiveConfigMapper.Setup(m => m.SetCspDirectiveConfig(overrideConfig, directive, directiveOverrideResult));

            CspConfigurationOverrideHelper.SetCspSandboxOverride(MockContext, directiveOverride, reportOnly);

            //Verify that the override result was set on the override config.
            Assert.AreSame(directiveOverrideResult, overrideConfig.SandboxDirective);
            //_directiveConfigMapper.Verify(m => m.SetCspDirectiveConfig(overrideConfig, directiveOverrideResult), Times.Once);
        }

        [Test]
        public void SetCspSandboxOverride_HasOverride_OverridesExistingOverride([Values(false, true)]bool reportOnly)
        {
            //There's an override for directive
            var currentDirectiveOverride = new CspSandboxDirectiveConfiguration();
            var overrideConfig = new CspOverrideConfiguration {SandboxDirective = currentDirectiveOverride};
            _contextHelper.Setup(h => h.GetCspConfigurationOverride(It.IsAny<HttpContextBase>(), reportOnly, false)).Returns(overrideConfig);
            //_directiveConfigMapper.Setup(m => m.GetCspDirectiveConfig(overrideConfig, directive)).Returns(currentDirectiveOverride);
            //We need an override and a result.
            var directiveOverride = new CspSandboxOverride();
            var directiveOverrideResult = new CspSandboxDirectiveConfiguration();
            _directiveOverrideHelper.Setup(h => h.GetOverridenCspSandboxConfig(directiveOverride, currentDirectiveOverride)).Returns(directiveOverrideResult);
            //This should be called at the very end
            //_directiveConfigMapper.Setup(m => m.SetCspDirectiveConfig(overrideConfig, directive, directiveOverrideResult));

            CspConfigurationOverrideHelper.SetCspSandboxOverride(MockContext, directiveOverride, reportOnly);

            //Verify that the override result was set on the override config.
            Assert.AreSame(directiveOverrideResult, overrideConfig.SandboxDirective);
            //_directiveConfigMapper.Verify(m => m.SetCspDirectiveConfig(overrideConfig, directive, directiveOverrideResult), Times.Once);
        }

        [Test]
        public void GetCspScriptNonce_ScriptNonceRequestedNoOverrides_ClonesBaseConfigAndOverridesNonce()
        {
            var cspConfig = new CspConfiguration();
            var cspConfigReportOnly = new CspConfiguration();
            var overrideConfig = new CspOverrideConfiguration();
            var overrideConfigReportOnly = new CspOverrideConfiguration();
            var clonedCspDirective = new CspDirectiveConfiguration();
            var clonedCspReportOnlyDirective = new CspDirectiveConfiguration();
            _contextHelper.Setup(h => h.GetCspConfiguration(It.IsAny<HttpContextBase>(), false)).Returns(cspConfig);
            _contextHelper.Setup(h => h.GetCspConfiguration(It.IsAny<HttpContextBase>(), true)).Returns(cspConfigReportOnly);
            _contextHelper.Setup(h => h.GetCspConfigurationOverride(It.IsAny<HttpContextBase>(), false, false)).Returns(overrideConfig);
            _contextHelper.Setup(h => h.GetCspConfigurationOverride(It.IsAny<HttpContextBase>(), true, false)).Returns(overrideConfigReportOnly);
            //No overrides
            _directiveConfigMapper.Setup(m => m.GetCspDirectiveConfig(overrideConfig, CspDirectives.ScriptSrc)).Returns((ICspDirectiveConfiguration)null);
            _directiveConfigMapper.Setup(m => m.GetCspDirectiveConfig(overrideConfigReportOnly, CspDirectives.ScriptSrc)).Returns((ICspDirectiveConfiguration)null);
            _directiveConfigMapper.Setup(m => m.GetCspDirectiveConfigCloned(cspConfig, CspDirectives.ScriptSrc)).Returns(clonedCspDirective);
            _directiveConfigMapper.Setup(m => m.GetCspDirectiveConfigCloned(cspConfigReportOnly, CspDirectives.ScriptSrc)).Returns(clonedCspReportOnlyDirective);
            _directiveConfigMapper.Setup(m => m.SetCspDirectiveConfig(overrideConfig, CspDirectives.ScriptSrc, clonedCspDirective));
            _directiveConfigMapper.Setup(m => m.SetCspDirectiveConfig(overrideConfigReportOnly, CspDirectives.ScriptSrc, clonedCspReportOnlyDirective));

            var nonce = CspConfigurationOverrideHelper.GetCspScriptNonce(MockContext);

            Assert.AreEqual(nonce, clonedCspDirective.Nonce);
            Assert.AreEqual(nonce, clonedCspReportOnlyDirective.Nonce);
            _directiveConfigMapper.Verify(m => m.SetCspDirectiveConfig(overrideConfig, CspDirectives.ScriptSrc, clonedCspDirective), Times.Once);
            _directiveConfigMapper.Verify(m => m.SetCspDirectiveConfig(overrideConfigReportOnly, CspDirectives.ScriptSrc, clonedCspReportOnlyDirective), Times.Once);
        }

        [Test]
        public void GetCspScriptNonce_ScriptNonceRequestedAndOverrideWithoutNonce_SetsNonceOnOverride()
        {
            var overrideConfig = new CspOverrideConfiguration();
            var overrideConfigReportOnly = new CspOverrideConfiguration();
            var overrideCspDirective = new CspDirectiveConfiguration();
            var overrideCspReportOnlyDirective = new CspDirectiveConfiguration();
            _contextHelper.Setup(h => h.GetCspConfigurationOverride(It.IsAny<HttpContextBase>(), false, false)).Returns(overrideConfig);
            _contextHelper.Setup(h => h.GetCspConfigurationOverride(It.IsAny<HttpContextBase>(), true, false)).Returns(overrideConfigReportOnly);
            _directiveConfigMapper.Setup(m => m.GetCspDirectiveConfig(overrideConfig, CspDirectives.ScriptSrc)).Returns(overrideCspDirective);
            _directiveConfigMapper.Setup(m => m.GetCspDirectiveConfig(overrideConfigReportOnly, CspDirectives.ScriptSrc)).Returns(overrideCspReportOnlyDirective);

            var nonce = CspConfigurationOverrideHelper.GetCspScriptNonce(MockContext);

            Assert.AreEqual(nonce, overrideCspDirective.Nonce);
            Assert.AreEqual(nonce, overrideCspReportOnlyDirective.Nonce);
        }

        [Test]
        public void GetCspScriptNonce_NonceRequestedAndNonceExists_ReturnsSameNonce()
        {
            var overrideConfig = new CspOverrideConfiguration { ScriptSrcDirective = new CspDirectiveConfiguration { Nonce = "heyhey" } };
            _contextHelper.Setup(h => h.GetCspConfigurationOverride(It.IsAny<HttpContextBase>(), false, false)).Returns(overrideConfig);

            var nonce = CspConfigurationOverrideHelper.GetCspScriptNonce(MockContext);

            Assert.AreEqual("heyhey", nonce);
        }

        [Test]
        public void GetCspStyleNonce_StyleNonceRequestedNoOverrides_ClonesBaseConfigAndOverridesNonce()
        {
            var cspConfig = new CspConfiguration();
            var cspConfigReportOnly = new CspConfiguration();
            var overrideConfig = new CspOverrideConfiguration();
            var overrideConfigReportOnly = new CspOverrideConfiguration();
            var clonedCspDirective = new CspDirectiveConfiguration();
            var clonedCspReportOnlyDirective = new CspDirectiveConfiguration();
            _contextHelper.Setup(h => h.GetCspConfiguration(It.IsAny<HttpContextBase>(), false)).Returns(cspConfig);
            _contextHelper.Setup(h => h.GetCspConfiguration(It.IsAny<HttpContextBase>(), true)).Returns(cspConfigReportOnly);
            _contextHelper.Setup(h => h.GetCspConfigurationOverride(It.IsAny<HttpContextBase>(), false, false)).Returns(overrideConfig);
            _contextHelper.Setup(h => h.GetCspConfigurationOverride(It.IsAny<HttpContextBase>(), true, false)).Returns(overrideConfigReportOnly);
            //No overrides
            _directiveConfigMapper.Setup(m => m.GetCspDirectiveConfig(overrideConfig, CspDirectives.StyleSrc)).Returns((ICspDirectiveConfiguration)null);
            _directiveConfigMapper.Setup(m => m.GetCspDirectiveConfig(overrideConfigReportOnly, CspDirectives.StyleSrc)).Returns((ICspDirectiveConfiguration)null);
            _directiveConfigMapper.Setup(m => m.GetCspDirectiveConfigCloned(cspConfig, CspDirectives.StyleSrc)).Returns(clonedCspDirective);
            _directiveConfigMapper.Setup(m => m.GetCspDirectiveConfigCloned(cspConfigReportOnly, CspDirectives.StyleSrc)).Returns(clonedCspReportOnlyDirective);
            _directiveConfigMapper.Setup(m => m.SetCspDirectiveConfig(overrideConfig, CspDirectives.StyleSrc, clonedCspDirective));
            _directiveConfigMapper.Setup(m => m.SetCspDirectiveConfig(overrideConfigReportOnly, CspDirectives.StyleSrc, clonedCspReportOnlyDirective));

            var nonce = CspConfigurationOverrideHelper.GetCspStyleNonce(MockContext);

            Assert.AreEqual(nonce, clonedCspDirective.Nonce);
            Assert.AreEqual(nonce, clonedCspReportOnlyDirective.Nonce);
            _directiveConfigMapper.Verify(m => m.SetCspDirectiveConfig(overrideConfig, CspDirectives.StyleSrc, clonedCspDirective), Times.Once);
            _directiveConfigMapper.Verify(m => m.SetCspDirectiveConfig(overrideConfigReportOnly, CspDirectives.StyleSrc, clonedCspReportOnlyDirective), Times.Once);
        }

        [Test]
        public void GetCspStyleNonce_StyleNonceRequestedAndOverrideWithoutNonce_SetsNonceOnOverride()
        {
            var overrideConfig = new CspOverrideConfiguration();
            var overrideConfigReportOnly = new CspOverrideConfiguration();
            var overrideCspDirective = new CspDirectiveConfiguration();
            var overrideCspReportOnlyDirective = new CspDirectiveConfiguration();
            _contextHelper.Setup(h => h.GetCspConfigurationOverride(It.IsAny<HttpContextBase>(), false, false)).Returns(overrideConfig);
            _contextHelper.Setup(h => h.GetCspConfigurationOverride(It.IsAny<HttpContextBase>(), true, false)).Returns(overrideConfigReportOnly);
            _directiveConfigMapper.Setup(m => m.GetCspDirectiveConfig(overrideConfig, CspDirectives.StyleSrc)).Returns(overrideCspDirective);
            _directiveConfigMapper.Setup(m => m.GetCspDirectiveConfig(overrideConfigReportOnly, CspDirectives.StyleSrc)).Returns(overrideCspReportOnlyDirective);

            var nonce = CspConfigurationOverrideHelper.GetCspStyleNonce(MockContext);

            Assert.AreEqual(nonce, overrideCspDirective.Nonce);
            Assert.AreEqual(nonce, overrideCspReportOnlyDirective.Nonce);
        }

        [Test]
        public void GetCspStyleNonce_NonceRequestedAndNonceExists_ReturnsSameNonce()
        {
            var overrideConfig = new CspOverrideConfiguration { StyleSrcDirective = new CspDirectiveConfiguration { Nonce = "heyhey" } };
            _contextHelper.Setup(h => h.GetCspConfigurationOverride(It.IsAny<HttpContextBase>(), false, false)).Returns(overrideConfig);

            var nonce = CspConfigurationOverrideHelper.GetCspStyleNonce(MockContext);

            Assert.AreEqual("heyhey", nonce);
        }
    }
}