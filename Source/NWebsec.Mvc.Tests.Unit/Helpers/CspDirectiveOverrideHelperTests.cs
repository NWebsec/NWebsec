// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Linq;
using NUnit.Framework;
using NWebsec.Core.HttpHeaders.Configuration;
using NWebsec.Mvc.Csp;
using NWebsec.Mvc.Helpers;
using NWebsec.Mvc.Tests.Unit.TestHelpers;

namespace NWebsec.Mvc.Tests.Unit.Helpers
{
    [TestFixture]
    public class CspDirectiveOverrideHelperTests
    {
        private CspDirectiveOverrideHelper _overrideHelper;

        [SetUp]
        public void Setup()
        {
            _overrideHelper = new CspDirectiveOverrideHelper();
        }


        [Test]
        public void GetOverridenCspDirectiveConfig_NullConfig_ReturnsNewDefaultConfig()
        {
            var directiveConfig = new CspDirectiveConfiguration();
            var directiveOverride = new CspDirectiveOverride { Enabled = directiveConfig.Enabled };

            var newConfig = _overrideHelper.GetOverridenCspDirectiveConfig(directiveOverride, null);

            Assert.AreNotSame(directiveConfig, newConfig);
            Assert.That(newConfig, Is.EqualTo(directiveConfig).Using(new CspDirectiveConfigurationComparer()));
        }

        [Test]
        public void GetOverridenCspDirectiveConfig_EnabledOverride_EnabledOverriden([Values(true, false)] bool expectedResult)
        {
            var directiveConfig = new CspDirectiveConfiguration { Enabled = !expectedResult };
            var directiveOverride = new CspDirectiveOverride { Enabled = expectedResult };

            var newConfig = _overrideHelper.GetOverridenCspDirectiveConfig(directiveOverride, directiveConfig);

            Assert.AreEqual(expectedResult, newConfig.Enabled);
        }


        [Test]
        public void GetOverridenCspDirectiveConfig_NoneEnabledOverride_OverridesNoneAndDropsOtherSources()
        {
            //Overriding with 'none' should clear all other sources. 
            var directiveConfig = new CspDirectiveConfiguration
            {
                NoneSrc = false,
                SelfSrc = true,
                Nonce = "hei",
                UnsafeEvalSrc = true,
                UnsafeInlineSrc = true,
                CustomSources = new[] { "nwebsec.com" }
            };
            var directiveOverride = new CspDirectiveOverride { None = true };
            var expectedConfig = new CspDirectiveConfiguration { NoneSrc = true };

            var newConfig = _overrideHelper.GetOverridenCspDirectiveConfig(directiveOverride, directiveConfig);

            Assert.That(newConfig, Is.EqualTo(expectedConfig).Using(new CspDirectiveConfigurationComparer()));
        }

        [Test]
        public void GetOverridenCspDirectiveConfig_NoneDisabledOverride_OverridesNoneAndKeepsOtherSources()
        {
            var directiveConfig = new CspDirectiveConfiguration
            {
                NoneSrc = false,
                SelfSrc = true,
                Nonce = "hei",
                UnsafeEvalSrc = true,
                UnsafeInlineSrc = true,
                CustomSources = new[] { "nwebsec.com" }
            };
            var directiveOverride = new CspDirectiveOverride { None = false };


            var newConfig = _overrideHelper.GetOverridenCspDirectiveConfig(directiveOverride, directiveConfig);

            Assert.That(newConfig, Is.EqualTo(directiveConfig).Using(new CspDirectiveConfigurationComparer()));
        }

        [Test]
        public void GetOverridenCspDirectiveConfig_NoneInheritAndOtherSourcesOverride_OverridesNone()
        {
            //An inherited 'none' should be overriden when other sources are enabled.
            var overrides = new[]
            {
                new CspDirectiveOverride {Self = true},
                new CspDirectiveOverride {UnsafeEval = true},
                new CspDirectiveOverride {UnsafeInline = true},
                new CspDirectiveOverride {OtherSources = new []{"nwebsec.com"}},
            
            };

            foreach (var directiveOverride in overrides)
            {
                var directiveConfig = new CspDirectiveConfiguration { NoneSrc = true };
                var newConfig = _overrideHelper.GetOverridenCspDirectiveConfig(directiveOverride, directiveConfig);

                Assert.IsFalse(newConfig.NoneSrc);
            }
        }

        [Test]
        public void GetOverridenCspDirectiveConfig_SelfOverride_OverridesSelf([Values(true, false)] bool expectedResult)
        {
            var directiveConfig = new CspDirectiveConfiguration { SelfSrc = !expectedResult };
            var directiveOverride = new CspDirectiveOverride { Self = expectedResult };

            var newConfig = _overrideHelper.GetOverridenCspDirectiveConfig(directiveOverride, directiveConfig);

            Assert.AreEqual(expectedResult, newConfig.SelfSrc);
        }

        [Test]
        public void GetOverridenCspDirectiveConfig_SelfInherit_InheritsSelf([Values(true, false)] bool expectedResult)
        {
            var directiveConfig = new CspDirectiveConfiguration { SelfSrc = expectedResult };
            var directiveOverride = new CspDirectiveOverride();

            var newConfig = _overrideHelper.GetOverridenCspDirectiveConfig(directiveOverride, directiveConfig);

            Assert.AreEqual(expectedResult, newConfig.SelfSrc);
        }

        [Test]
        public void GetOverridenCspDirectiveConfig_UnsafeEvalOverride_OverridesUnsafeEval([Values(true, false)] bool expectedResult)
        {
            var directiveConfig = new CspDirectiveConfiguration { UnsafeEvalSrc = !expectedResult };
            var directiveOverride = new CspDirectiveOverride { UnsafeEval = expectedResult };

            var newConfig = _overrideHelper.GetOverridenCspDirectiveConfig(directiveOverride, directiveConfig);

            Assert.AreEqual(expectedResult, newConfig.UnsafeEvalSrc);
        }

        [Test]
        public void GetOverridenCspDirectiveConfig_UnsafeEvalInherit_InheritsUnsafeEval([Values(true, false)] bool expectedResult)
        {
            var directiveConfig = new CspDirectiveConfiguration { UnsafeEvalSrc = expectedResult };
            var directiveOverride = new CspDirectiveOverride();

            var newConfig = _overrideHelper.GetOverridenCspDirectiveConfig(directiveOverride, directiveConfig);

            Assert.AreEqual(expectedResult, newConfig.UnsafeEvalSrc);
        }

        [Test]
        public void GetOverridenCspDirectiveConfig_UnsafeInlineOverride_OverridesUnsafeInline([Values(true, false)] bool expectedResult)
        {
            var directiveConfig = new CspDirectiveConfiguration { UnsafeInlineSrc = !expectedResult };
            var directiveOverride = new CspDirectiveOverride { UnsafeInline = expectedResult };

            var newConfig = _overrideHelper.GetOverridenCspDirectiveConfig(directiveOverride, directiveConfig);

            Assert.AreEqual(expectedResult, newConfig.UnsafeInlineSrc);
        }

        [Test]
        public void GetOverridenCspDirectiveConfig_UnsafeInlineInherit_InheritsUnsafeInline([Values(true, false)] bool expectedResult)
        {
            var directiveConfig = new CspDirectiveConfiguration { UnsafeInlineSrc = expectedResult };
            var directiveOverride = new CspDirectiveOverride();

            var newConfig = _overrideHelper.GetOverridenCspDirectiveConfig(directiveOverride, directiveConfig);

            Assert.AreEqual(expectedResult, newConfig.UnsafeInlineSrc);
        }

        [Test]
        public void GetOverridenCspDirectiveConfig_NoCustomSourcesOverride_KeepsCustomSources()
        {
            var expectedSources = new[] { "www.nwebsec.com" };
            var directiveConfig = new CspDirectiveConfiguration { CustomSources = expectedSources };
            var directiveOverride = new CspDirectiveOverride { InheritOtherSources = true };

            var newConfig = _overrideHelper.GetOverridenCspDirectiveConfig(directiveOverride, directiveConfig);

            Assert.IsTrue(expectedSources.SequenceEqual(newConfig.CustomSources), "CustomSources differed.");
        }

        [Test]
        public void GetOverridenCspDirectiveConfig_CustomSourcesOverride_OverriddesCustomSources()
        {
            var directiveConfig = new CspDirectiveConfiguration { CustomSources = new[] { "www.nwebsec.com" } };
            var directiveOverride = new CspDirectiveOverride { OtherSources = new[] { "*.nwebsec.com" }, InheritOtherSources = false };

            var newConfig = _overrideHelper.GetOverridenCspDirectiveConfig(directiveOverride, directiveConfig);

            Assert.IsFalse(newConfig.SelfSrc);
            Assert.IsTrue(newConfig.CustomSources.Count() == 1);
            Assert.IsTrue(newConfig.CustomSources.First().Equals("*.nwebsec.com"));
        }

        [Test]
        public void GetOverridenCspDirectiveConfig_CustomSourcesOverrideWithSourcesInherited_KeepsAllSources()
        {
            var directiveConfig = new CspDirectiveConfiguration { CustomSources = new[] { "transformtool.codeplex.com" } };
            var directiveOverride = new CspDirectiveOverride { OtherSources = new[] { "nwebsec.codeplex.com" }, InheritOtherSources = true };

            var newConfig = _overrideHelper.GetOverridenCspDirectiveConfig(directiveOverride, directiveConfig);

            Assert.AreEqual(2, newConfig.CustomSources.Count());
            Assert.Contains("transformtool.codeplex.com", newConfig.CustomSources.ToList());
            Assert.Contains("nwebsec.codeplex.com", newConfig.CustomSources.ToList());
        }

        [Test]
        public void GetOverridenCspPluginTypesConfig_NullConfig_ReturnsNewDefaultConfig()
        {
            var directiveConfig = new CspPluginTypesDirectiveConfiguration();
            var directiveOverride = new CspPluginTypesOverride { Enabled = directiveConfig.Enabled };

            var newConfig = _overrideHelper.GetOverridenCspPluginTypesConfig(directiveOverride, null);

            Assert.AreNotSame(directiveConfig, newConfig);
            Assert.That(newConfig, Is.EqualTo(directiveConfig).Using(new CspPluginTypesDirectiveConfigurationComparer()));
        }

        [Test]
        public void GetOverridenCspPluginTypesConfig_EnabledOverride_EnabledOverriden([Values(true, false)] bool expectedResult)
        {
            var directiveConfig = new CspPluginTypesDirectiveConfiguration { Enabled = !expectedResult };
            var directiveOverride = new CspPluginTypesOverride { Enabled = expectedResult };

            var newConfig = _overrideHelper.GetOverridenCspPluginTypesConfig(directiveOverride, directiveConfig);

            Assert.AreEqual(expectedResult, newConfig.Enabled);
        }

        [Test]
        public void GetOverridenCspPluginTypesConfig_NoMediaTypesOverride_KeepsMediaTypes()
        {
            var expectedMediaTypes = new[] { "application/pdf" };
            var directiveConfig = new CspPluginTypesDirectiveConfiguration { MediaTypes = expectedMediaTypes };
            var directiveOverride = new CspPluginTypesOverride { InheritMediaTypes = true };

            var newConfig = _overrideHelper.GetOverridenCspPluginTypesConfig(directiveOverride, directiveConfig);

            Assert.IsTrue(expectedMediaTypes.SequenceEqual(newConfig.MediaTypes), "MediaTypes differed.");
        }

        [Test]
        public void GetOverridenCspPluginTypesConfig_MediaTypesOverride_OverriddesMediaTypes()
        {
            var directiveConfig = new CspPluginTypesDirectiveConfiguration { MediaTypes = new[] { "application/pdf" } };
            var directiveOverride = new CspPluginTypesOverride { MediaTypes = new[] { "image/png" }, InheritMediaTypes = false };

            var newConfig = _overrideHelper.GetOverridenCspPluginTypesConfig(directiveOverride, directiveConfig);

            Assert.IsTrue(newConfig.MediaTypes.Count() == 1);
            Assert.IsTrue(newConfig.MediaTypes.First().Equals("image/png"));
        }

        [Test]
        public void GetOverridenCspPluginTypesConfig_MediaTypesOverrideWithMediaTypesInherited_KeepsAllMediaTypes()
        {
            var directiveConfig = new CspPluginTypesDirectiveConfiguration { MediaTypes = new[] { "application/pdf" } };
            var directiveOverride = new CspPluginTypesOverride { MediaTypes = new[] { "image/png" }, InheritMediaTypes = true };

            var newConfig = _overrideHelper.GetOverridenCspPluginTypesConfig(directiveOverride, directiveConfig);

            Assert.AreEqual(2, newConfig.MediaTypes.Count());
            Assert.Contains("application/pdf", newConfig.MediaTypes.ToList());
            Assert.Contains("image/png", newConfig.MediaTypes.ToList());
        }

        [Test]
        public void GetOverridenCspSandboxConfig_EnableOverride_OverridesEnabled([Values(true, false)] bool expectedResult)
        {
            var directiveConfig = new CspSandboxDirectiveConfiguration { Enabled = !expectedResult };
            var directiveOverride = new CspSandboxOverride { Enabled = expectedResult };

            var newConfig = _overrideHelper.GetOverridenCspSandboxConfig(directiveOverride, directiveConfig);

            Assert.AreEqual(expectedResult, newConfig.Enabled);
        }

        [Test]
        public void GetOverridenCspSandboxConfig_AllowFormsOverride_OverridesAllowForms([Values(true, false)] bool expectedResult)
        {
            var directiveConfig = new CspSandboxDirectiveConfiguration { AllowForms = !expectedResult };
            var directiveOverride = new CspSandboxOverride { AllowForms = expectedResult };

            var newConfig = _overrideHelper.GetOverridenCspSandboxConfig(directiveOverride, directiveConfig);

            Assert.AreEqual(expectedResult, newConfig.AllowForms);
        }

        [Test]
        public void GetOverridenCspSandboxConfig_AllowFormsNotSet_InheritsAllowForms([Values(true, false)] bool expectedResult)
        {
            var directiveConfig = new CspSandboxDirectiveConfiguration { AllowForms = expectedResult };
            var directiveOverride = new CspSandboxOverride();

            var newConfig = _overrideHelper.GetOverridenCspSandboxConfig(directiveOverride, directiveConfig);

            Assert.AreEqual(expectedResult, newConfig.AllowForms);
        }

        [Test]
        public void GetOverridenCspSandboxConfig_AllowPointerLockOverride_OverridesAllowPointerLock([Values(true, false)] bool expectedResult)
        {
            var directiveConfig = new CspSandboxDirectiveConfiguration { AllowPointerLock = !expectedResult };
            var directiveOverride = new CspSandboxOverride { AllowPointerLock = expectedResult };

            var newConfig = _overrideHelper.GetOverridenCspSandboxConfig(directiveOverride, directiveConfig);

            Assert.AreEqual(expectedResult, newConfig.AllowPointerLock);
        }

        [Test]
        public void GetOverridenCspSandboxConfig_AllowPointerLockNotSet_InheritsAllowPointerLock([Values(true, false)] bool expectedResult)
        {
            var directiveConfig = new CspSandboxDirectiveConfiguration { AllowPointerLock = expectedResult };
            var directiveOverride = new CspSandboxOverride();

            var newConfig = _overrideHelper.GetOverridenCspSandboxConfig(directiveOverride, directiveConfig);

            Assert.AreEqual(expectedResult, newConfig.AllowPointerLock);
        }

        [Test]
        public void GetOverridenCspSandboxConfig_AllowPopupsOverride_OverridesAllowPopups([Values(true, false)] bool expectedResult)
        {
            var directiveConfig = new CspSandboxDirectiveConfiguration { AllowPopups = !expectedResult };
            var directiveOverride = new CspSandboxOverride { AllowPopups = expectedResult };

            var newConfig = _overrideHelper.GetOverridenCspSandboxConfig(directiveOverride, directiveConfig);

            Assert.AreEqual(expectedResult, newConfig.AllowPopups);
        }

        [Test]
        public void GetOverridenCspSandboxConfig_AllowPopupsNotSet_InheritsAllowPopups([Values(true, false)] bool expectedResult)
        {
            var directiveConfig = new CspSandboxDirectiveConfiguration { AllowPopups = expectedResult };
            var directiveOverride = new CspSandboxOverride();

            var newConfig = _overrideHelper.GetOverridenCspSandboxConfig(directiveOverride, directiveConfig);

            Assert.AreEqual(expectedResult, newConfig.AllowPopups);
        }

        [Test]
        public void GetOverridenCspSandboxConfig_AllowSameOriginOverride_OverridesAllowSameOrigin([Values(true, false)] bool expectedResult)
        {
            var directiveConfig = new CspSandboxDirectiveConfiguration { AllowSameOrigin = !expectedResult };
            var directiveOverride = new CspSandboxOverride { AllowSameOrigin = expectedResult };

            var newConfig = _overrideHelper.GetOverridenCspSandboxConfig(directiveOverride, directiveConfig);

            Assert.AreEqual(expectedResult, newConfig.AllowSameOrigin);
        }

        [Test]
        public void GetOverridenCspSandboxConfig_AllowSameOriginNotSet_InheritsAllowSameOrigin([Values(true, false)] bool expectedResult)
        {
            var directiveConfig = new CspSandboxDirectiveConfiguration { AllowSameOrigin = expectedResult };
            var directiveOverride = new CspSandboxOverride();

            var newConfig = _overrideHelper.GetOverridenCspSandboxConfig(directiveOverride, directiveConfig);

            Assert.AreEqual(expectedResult, newConfig.AllowSameOrigin);
        }

        [Test]
        public void GetOverridenCspSandboxConfig_AllowScriptsOverride_OverridesAllowScripts([Values(true, false)] bool expectedResult)
        {
            var directiveConfig = new CspSandboxDirectiveConfiguration { AllowScripts = !expectedResult };
            var directiveOverride = new CspSandboxOverride { AllowScripts = expectedResult };

            var newConfig = _overrideHelper.GetOverridenCspSandboxConfig(directiveOverride, directiveConfig);

            Assert.AreEqual(expectedResult, newConfig.AllowScripts);
        }

        [Test]
        public void GetOverridenCspSandboxConfig_AllowScriptsNotSet_InheritsAllowScripts([Values(true, false)] bool expectedResult)
        {
            var directiveConfig = new CspSandboxDirectiveConfiguration { AllowScripts = expectedResult };
            var directiveOverride = new CspSandboxOverride();

            var newConfig = _overrideHelper.GetOverridenCspSandboxConfig(directiveOverride, directiveConfig);

            Assert.AreEqual(expectedResult, newConfig.AllowScripts);
        }

        [Test]
        public void GetOverridenCspSandboxConfig_AllowTopNavigationOverride_OverridesAllowTopNavigation([Values(true, false)] bool expectedResult)
        {
            var directiveConfig = new CspSandboxDirectiveConfiguration { AllowTopNavigation = !expectedResult };
            var directiveOverride = new CspSandboxOverride { AllowTopNavigation = expectedResult };

            var newConfig = _overrideHelper.GetOverridenCspSandboxConfig(directiveOverride, directiveConfig);

            Assert.AreEqual(expectedResult, newConfig.AllowTopNavigation);
        }

        [Test]
        public void GetOverridenCspSandboxConfig_AllowTopNavigationNotSet_InheritsAllowTopNavigation([Values(true, false)] bool expectedResult)
        {
            var directiveConfig = new CspSandboxDirectiveConfiguration { AllowTopNavigation = expectedResult };
            var directiveOverride = new CspSandboxOverride();

            var newConfig = _overrideHelper.GetOverridenCspSandboxConfig(directiveOverride, directiveConfig);

            Assert.AreEqual(expectedResult, newConfig.AllowTopNavigation);
        }
    }
}