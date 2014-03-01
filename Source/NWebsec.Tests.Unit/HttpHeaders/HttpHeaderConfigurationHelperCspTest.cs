// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using NUnit.Framework;
using NWebsec.Core.HttpHeaders.Configuration;
using NWebsec.HttpHeaders;
using NWebsec.Modules.Configuration;
using NWebsec.Modules.Configuration.Csp;

namespace NWebsec.Tests.Unit.HttpHeaders
{
    [TestFixture]
    public class HttpHeaderConfigurationHelperCspTest : HttpHeaderConfigurationHelperCspTestBase
    {
        protected override bool ReportOnly
        {
            get { return false; }
        }

        protected override CspConfigurationElement CspConfig
        {
            get { return ConfigSection.SecurityHttpHeaders.Csp; }
        }

        [Test]
        public void GetCspHeaderWithOverride_CspEnabledReportOnlyDisabledInConfig_CspEnabledReportOnlyDisabled()
        {
            var configSection = new HttpHeaderSecurityConfigurationSection();
            configSection.SecurityHttpHeaders.Csp.Enabled = true;
            configSection.SecurityHttpHeaders.CspReportOnly.Enabled = false;
            HeaderConfigurationHelper = new HttpHeaderConfigurationHelper(configSection);

            var cspOverrideElement = HeaderConfigurationHelper.GetCspHeaderWithOverride(MockContext, false);
            var cspReportOnlyOverrideElement = HeaderConfigurationHelper.GetCspHeaderWithOverride(MockContext, true);
            Assert.IsTrue(cspOverrideElement.Enabled);
            Assert.IsFalse(cspReportOnlyOverrideElement.Enabled);
        }

        [Test]
        public void GetCspHeaderWithOverride_CspDisabledReportOnlyEnabledInConfig_CspDisabledReportOnlyEnabled()
        {
            var configSection = new HttpHeaderSecurityConfigurationSection();
            configSection.SecurityHttpHeaders.Csp.Enabled = false;
            configSection.SecurityHttpHeaders.CspReportOnly.Enabled = true;
            HeaderConfigurationHelper = new HttpHeaderConfigurationHelper(configSection);

            var cspOverrideElement = HeaderConfigurationHelper.GetCspHeaderWithOverride(MockContext, false);
            var cspReportOnlyOverrideElement = HeaderConfigurationHelper.GetCspHeaderWithOverride(MockContext, true);
            Assert.IsFalse(cspOverrideElement.Enabled);
            Assert.IsTrue(cspReportOnlyOverrideElement.Enabled);
        }

        [Test]
        public void CloneElement_UnsafeEval_ClonesElement()
        {
            var directive = new CspDirectiveUnsafeEvalConfiguration();
            directive.UnsafeEvalSrc = true;
            directive.UnsafeInlineSrc = false;
            directive.CustomSources = new string[] {};
                var clone = (ICspDirectiveUnsafeEvalConfiguration)HeaderConfigurationHelper.CloneElement(directive);

            Assert.IsTrue(clone.UnsafeEvalSrc);
        }

        [Test]
        public void CloneElement_UnsafeInline_ClonesElement()
        {
            var directive = new CspDirectiveUnsafeInlineConfiguration();
            directive.UnsafeInlineSrc = true;
            directive.CustomSources = new string[] { };
            var clone = (ICspDirectiveUnsafeInlineConfiguration)HeaderConfigurationHelper.CloneElement(directive);

            Assert.IsTrue(clone.UnsafeInlineSrc);
        }
    }
}