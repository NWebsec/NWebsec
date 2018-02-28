// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Web;
using Moq;
using NWebsec.AspNet.Mvc.Tests.TestHelpers;
using NWebsec.Core.Common.HttpHeaders.Configuration;
using NWebsec.Mvc.Common.Csp;
using NWebsec.Mvc.Common.Helpers;
using NWebsec.Mvc.Helpers;
using Xunit;

namespace NWebsec.AspNet.Mvc.Tests.Helpers
{
    public class CspConfigurationOverrideHelperTests
    {
        public static readonly IEnumerable<object> ReportOnly = new TheoryData<bool> { false, true };

        protected HttpContextBase MockContext;
        protected CspConfigurationOverrideHelper CspConfigurationOverrideHelper;
        private readonly Mock<IContextConfigurationHelper> _contextHelper;
        private readonly Mock<ICspConfigMapper> _directiveConfigMapper;
        private readonly Mock<ICspDirectiveOverrideHelper> _directiveOverrideHelper;

        public CspConfigurationOverrideHelperTests()
        {
            MockContext = new Mock<HttpContextBase>().Object;

            _contextHelper = new Mock<IContextConfigurationHelper>(MockBehavior.Strict);
            _directiveConfigMapper = new Mock<ICspConfigMapper>(MockBehavior.Strict);
            _directiveOverrideHelper = new Mock<ICspDirectiveOverrideHelper>(MockBehavior.Strict);

            CspConfigurationOverrideHelper = new CspConfigurationOverrideHelper(_contextHelper.Object, _directiveConfigMapper.Object, _directiveOverrideHelper.Object);
        }

        [Theory, MemberData(nameof(ReportOnly))]
        public void GetCspElementWithOverrides_NoOverride_ReturnsNull(bool reportOnly)
        {
            _contextHelper.Setup(h => h.GetCspConfigurationOverride(It.IsAny<HttpContextBase>(), reportOnly, true)).Returns((CspOverrideConfiguration)null);

            var overrideConfig = CspConfigurationOverrideHelper.GetCspConfigWithOverrides(MockContext, reportOnly);

            Assert.Null(overrideConfig);
        }

        [Theory, MemberData(nameof(ReportOnly))]
        public void GetCspElementWithOverrides_OverrideAndConfigFromContext_MergesConfigFromContextAndOverrides(bool reportOnly)
        {
            var cspConfig = new CspConfiguration();
            var overrideConfig = new CspOverrideConfiguration();
            _contextHelper.Setup(h => h.GetCspConfiguration(It.IsAny<HttpContextBase>(), reportOnly)).Returns(cspConfig);
            _contextHelper.Setup(h => h.GetCspConfigurationOverride(It.IsAny<HttpContextBase>(), reportOnly, true)).Returns(overrideConfig);
            _directiveConfigMapper.Setup(m => m.MergeConfiguration(cspConfig, It.IsAny<ICspConfiguration>()));
            _directiveConfigMapper.Setup(m => m.MergeOverrides(overrideConfig, It.IsAny<ICspConfiguration>()));

            var overrideElement = CspConfigurationOverrideHelper.GetCspConfigWithOverrides(MockContext, reportOnly);

            Assert.NotNull(overrideElement);
            _directiveConfigMapper.Verify(m => m.MergeConfiguration(cspConfig, It.IsAny<ICspConfiguration>()), Times.Once);
            _directiveConfigMapper.Verify(m => m.MergeOverrides(overrideConfig, It.IsAny<ICspConfiguration>()), Times.Once);
        }


        [Theory, MemberData(nameof(ReportOnly))]
        public void GetCspElementWithOverrides_OverrideAndNoConfigFromContext_MergesOverrides(bool reportOnly)
        {
            var overrideConfig = new CspOverrideConfiguration();
            _contextHelper.Setup(h => h.GetCspConfiguration(It.IsAny<HttpContextBase>(), reportOnly)).Returns((ICspConfiguration)null);
            _contextHelper.Setup(h => h.GetCspConfigurationOverride(It.IsAny<HttpContextBase>(), reportOnly, true)).Returns(overrideConfig);
            _directiveConfigMapper.Setup(m => m.MergeOverrides(overrideConfig, It.IsAny<ICspConfiguration>()));

            var overrideElement = CspConfigurationOverrideHelper.GetCspConfigWithOverrides(MockContext, reportOnly);

            Assert.NotNull(overrideElement);
            _directiveConfigMapper.Verify(m => m.MergeOverrides(overrideConfig, It.IsAny<ICspConfiguration>()), Times.Once);
        }

        [Theory, MemberData(nameof(ReportOnly))]
        public void SetCspHeaderOverride_OverridesWithHeaderEnabled_HeaderEnabled(bool reportOnly)
        {
            var overrideConfig = new CspOverrideConfiguration();
            _contextHelper.Setup(h => h.GetCspConfigurationOverride(It.IsAny<HttpContextBase>(), reportOnly, false)).Returns(overrideConfig);
            var cspOverride = new CspHeaderConfiguration { Enabled = true };

            CspConfigurationOverrideHelper.SetCspHeaderOverride(MockContext, cspOverride, reportOnly);

            Assert.True(overrideConfig.EnabledOverride);
            Assert.True(overrideConfig.Enabled);
        }


        [Theory, MemberData(nameof(ReportOnly))]
        public void SetCspReportUriOverride_ReportUriDisabledAndOverridden_ReturnsOverridenReportUri(bool reportOnly)
        {
            var overrideConfig = new CspOverrideConfiguration();
            _contextHelper.Setup(h => h.GetCspConfigurationOverride(It.IsAny<HttpContextBase>(), reportOnly, false)).Returns(overrideConfig);
            var reportUri = new CspReportUriDirectiveConfiguration { Enabled = true, EnableBuiltinHandler = true };

            CspConfigurationOverrideHelper.SetCspReportUriOverride(MockContext, reportUri, reportOnly);

            Assert.True(overrideConfig.ReportUriDirective.Enabled);
            Assert.True(overrideConfig.ReportUriDirective.EnableBuiltinHandler);
        }

        [Theory, MemberData(nameof(GetDirectivesAndReportonlyEnumeration))]
        public void SetCspDirectiveOverride_NoCurrentOverride_ClonesConfigFromContextAndOverrides(bool reportOnly, CspDirectives directive)
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

        [Theory, MemberData(nameof(GetDirectivesAndReportonlyEnumeration))]
        public void SetCspDirectiveOverride_HasOverride_OverridesExistingOverride(bool reportOnly, CspDirectives directive)
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

        [Theory, MemberData(nameof(ReportOnly))]
        public void SetCspPluginTypesOverride_NoCurrentOverride_ClonesConfigFromContextAndOverrides(bool reportOnly)
        {

            var contextConfig = new CspConfiguration();
            var overrideConfig = new CspOverrideConfiguration();
            //Returns CSP config from context
            _contextHelper.Setup(h => h.GetCspConfiguration(It.IsAny<HttpContextBase>(), reportOnly)).Returns(contextConfig);
            _contextHelper.Setup(h => h.GetCspConfigurationOverride(It.IsAny<HttpContextBase>(), reportOnly, false)).Returns(overrideConfig);
            //Returns cloned directive config from context config
            var clonedContextDirective = new CspPluginTypesDirectiveConfiguration();
            _directiveConfigMapper.Setup(m => m.GetCspPluginTypesConfigCloned(contextConfig)).Returns(clonedContextDirective);
            //We need an override and a result.
            var directiveOverride = new CspPluginTypesOverride();
            var directiveOverrideResult = new CspPluginTypesDirectiveConfiguration();
            _directiveOverrideHelper.Setup(h => h.GetOverridenCspPluginTypesConfig(directiveOverride, clonedContextDirective)).Returns(directiveOverrideResult);

            CspConfigurationOverrideHelper.SetCspPluginTypesOverride(MockContext, directiveOverride, reportOnly);

            //Verify that the override result was set on the override config.
            Assert.Same(directiveOverrideResult, overrideConfig.PluginTypesDirective);
        }

        [Theory, MemberData(nameof(ReportOnly))]
        public void SetCspPluginTypesOverride_HasOverride_OverridesExistingOverride(bool reportOnly)
        {

            //There's an override for directive
            var currentDirectiveOverride = new CspPluginTypesDirectiveConfiguration();
            var overrideConfig = new CspOverrideConfiguration { PluginTypesDirective = currentDirectiveOverride };
            _contextHelper.Setup(h => h.GetCspConfigurationOverride(It.IsAny<HttpContextBase>(), reportOnly, false)).Returns(overrideConfig);
            //We need an override and a result.
            var directiveOverride = new CspPluginTypesOverride();
            var directiveOverrideResult = new CspPluginTypesDirectiveConfiguration();
            _directiveOverrideHelper.Setup(h => h.GetOverridenCspPluginTypesConfig(directiveOverride, currentDirectiveOverride)).Returns(directiveOverrideResult);

            CspConfigurationOverrideHelper.SetCspPluginTypesOverride(MockContext, directiveOverride, reportOnly);

            //Verify that the override result was set on the override config.
            Assert.Same(directiveOverrideResult, overrideConfig.PluginTypesDirective);
        }

        [Theory, MemberData(nameof(ReportOnly))]
        public void SetCspSandboxOverride_NoCurrentOverride_ClonesConfigFromContextAndOverrides(bool reportOnly)
        {

            var contextConfig = new CspConfiguration();
            var overrideConfig = new CspOverrideConfiguration();
            //Returns CSP config from context
            _contextHelper.Setup(h => h.GetCspConfiguration(It.IsAny<HttpContextBase>(), reportOnly)).Returns(contextConfig);
            _contextHelper.Setup(h => h.GetCspConfigurationOverride(It.IsAny<HttpContextBase>(), reportOnly, false)).Returns(overrideConfig);
            //Returns cloned directive config from context config
            var clonedContextDirective = new CspSandboxDirectiveConfiguration();
            _directiveConfigMapper.Setup(m => m.GetCspSandboxConfigCloned(contextConfig)).Returns(clonedContextDirective);
            //We need an override and a result.
            var directiveOverride = new CspSandboxOverride();
            var directiveOverrideResult = new CspSandboxDirectiveConfiguration();
            _directiveOverrideHelper.Setup(h => h.GetOverridenCspSandboxConfig(directiveOverride, clonedContextDirective)).Returns(directiveOverrideResult);

            CspConfigurationOverrideHelper.SetCspSandboxOverride(MockContext, directiveOverride, reportOnly);

            //Verify that the override result was set on the override config.
            Assert.Same(directiveOverrideResult, overrideConfig.SandboxDirective);
        }

        [Theory, MemberData(nameof(ReportOnly))]
        public void SetCspSandboxOverride_HasOverride_OverridesExistingOverride(bool reportOnly)
        {
            //There's an override for directive
            var currentDirectiveOverride = new CspSandboxDirectiveConfiguration();
            var overrideConfig = new CspOverrideConfiguration { SandboxDirective = currentDirectiveOverride };
            _contextHelper.Setup(h => h.GetCspConfigurationOverride(It.IsAny<HttpContextBase>(), reportOnly, false)).Returns(overrideConfig);
            //We need an override and a result.
            var directiveOverride = new CspSandboxOverride();
            var directiveOverrideResult = new CspSandboxDirectiveConfiguration();
            _directiveOverrideHelper.Setup(h => h.GetOverridenCspSandboxConfig(directiveOverride, currentDirectiveOverride)).Returns(directiveOverrideResult);

            CspConfigurationOverrideHelper.SetCspSandboxOverride(MockContext, directiveOverride, reportOnly);

            //Verify that the override result was set on the override config.
            Assert.Same(directiveOverrideResult, overrideConfig.SandboxDirective);
        }

        [Theory, MemberData(nameof(ReportOnly))]
        public void SetCspMixedContentOverride_NoCurrentOverride_ClonesConfigFromContextAndOverrides(bool reportOnly)
        {

            var contextConfig = new CspConfiguration();
            var overrideConfig = new CspOverrideConfiguration();
            //Returns CSP config from context
            _contextHelper.Setup(h => h.GetCspConfiguration(It.IsAny<HttpContextBase>(), reportOnly)).Returns(contextConfig);
            _contextHelper.Setup(h => h.GetCspConfigurationOverride(It.IsAny<HttpContextBase>(), reportOnly, false)).Returns(overrideConfig);
            //Returns cloned directive config from context config
            var clonedContextDirective = new CspMixedContentDirectiveConfiguration();
            _directiveConfigMapper.Setup(m => m.GetCspMixedContentConfigCloned(contextConfig)).Returns(clonedContextDirective);
            //We need an override and a result.
            var directiveOverride = new CspMixedContentOverride();
            var directiveOverrideResult = new CspMixedContentDirectiveConfiguration();
            _directiveOverrideHelper.Setup(h => h.GetOverridenCspMixedContentConfig(directiveOverride, clonedContextDirective)).Returns(directiveOverrideResult);

            CspConfigurationOverrideHelper.SetCspMixedContentOverride(MockContext, directiveOverride, reportOnly);

            //Verify that the override result was set on the override config.
            Assert.Same(directiveOverrideResult, overrideConfig.MixedContentDirective);
        }

        [Theory, MemberData(nameof(ReportOnly))]
        public void SetCspMixedContentOverride_HasOverride_OverridesExistingOverride(bool reportOnly)
        {
            //There's an override for directive
            var currentDirectiveOverride = new CspMixedContentDirectiveConfiguration();
            var overrideConfig = new CspOverrideConfiguration { MixedContentDirective = currentDirectiveOverride };
            _contextHelper.Setup(h => h.GetCspConfigurationOverride(It.IsAny<HttpContextBase>(), reportOnly, false)).Returns(overrideConfig);
            //We need an override and a result.
            var directiveOverride = new CspMixedContentOverride();
            var directiveOverrideResult = new CspMixedContentDirectiveConfiguration();
            _directiveOverrideHelper.Setup(h => h.GetOverridenCspMixedContentConfig(directiveOverride, currentDirectiveOverride)).Returns(directiveOverrideResult);

            CspConfigurationOverrideHelper.SetCspMixedContentOverride(MockContext, directiveOverride, reportOnly);

            //Verify that the override result was set on the override config.
            Assert.Same(directiveOverrideResult, overrideConfig.MixedContentDirective);
        }

        [Fact]
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

            Assert.Equal(nonce, clonedCspDirective.Nonce);
            Assert.Equal(nonce, clonedCspReportOnlyDirective.Nonce);
            _directiveConfigMapper.Verify(m => m.SetCspDirectiveConfig(overrideConfig, CspDirectives.ScriptSrc, clonedCspDirective), Times.Once);
            _directiveConfigMapper.Verify(m => m.SetCspDirectiveConfig(overrideConfigReportOnly, CspDirectives.ScriptSrc, clonedCspReportOnlyDirective), Times.Once);
        }

        [Fact]
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

            Assert.Equal(nonce, overrideCspDirective.Nonce);
            Assert.Equal(nonce, overrideCspReportOnlyDirective.Nonce);
        }

        [Fact]
        public void GetCspScriptNonce_NonceRequestedAndNonceExists_ReturnsSameNonce()
        {
            var overrideConfig = new CspOverrideConfiguration { ScriptSrcDirective = new CspDirectiveConfiguration { Nonce = "heyhey" } };
            _contextHelper.Setup(h => h.GetCspConfigurationOverride(It.IsAny<HttpContextBase>(), false, false)).Returns(overrideConfig);

            var nonce = CspConfigurationOverrideHelper.GetCspScriptNonce(MockContext);

            Assert.Equal("heyhey", nonce);
        }

        [Fact]
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

            Assert.Equal(nonce, clonedCspDirective.Nonce);
            Assert.Equal(nonce, clonedCspReportOnlyDirective.Nonce);
            _directiveConfigMapper.Verify(m => m.SetCspDirectiveConfig(overrideConfig, CspDirectives.StyleSrc, clonedCspDirective), Times.Once);
            _directiveConfigMapper.Verify(m => m.SetCspDirectiveConfig(overrideConfigReportOnly, CspDirectives.StyleSrc, clonedCspReportOnlyDirective), Times.Once);
        }

        [Fact]
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

            Assert.Equal(nonce, overrideCspDirective.Nonce);
            Assert.Equal(nonce, overrideCspReportOnlyDirective.Nonce);
        }

        [Fact]
        public void GetCspStyleNonce_NonceRequestedAndNonceExists_ReturnsSameNonce()
        {
            var overrideConfig = new CspOverrideConfiguration { StyleSrcDirective = new CspDirectiveConfiguration { Nonce = "heyhey" } };
            _contextHelper.Setup(h => h.GetCspConfigurationOverride(It.IsAny<HttpContextBase>(), false, false)).Returns(overrideConfig);

            var nonce = CspConfigurationOverrideHelper.GetCspStyleNonce(MockContext);

            Assert.Equal("heyhey", nonce);
        }

        public static IEnumerable<object[]> GetDirectivesAndReportonlyEnumeration()
        {
            foreach (var directive in new CspCommonDirectivesData())
            {
                yield return new[] { false, directive[0] };
                yield return new[] { true, directive[0] };
            }
        }
    }
}