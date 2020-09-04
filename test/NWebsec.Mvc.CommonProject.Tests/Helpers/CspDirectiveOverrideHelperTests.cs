// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using NWebsec.Core.Common.HttpHeaders.Configuration;
using NWebsec.Mvc.Common.Csp;
using NWebsec.Mvc.Common.Helpers;
using NWebsec.Mvc.CommonProject.Tests.TestHelpers;
using Xunit;

namespace NWebsec.Mvc.CommonProject.Tests.Helpers
{
    public class CspDirectiveOverrideHelperTests
    {
        public static readonly IEnumerable<object[]> FalseThenTrue = new TheoryData<bool> { false, true };

        private readonly CspDirectiveOverrideHelper _overrideHelper;

        public CspDirectiveOverrideHelperTests()
        {
            _overrideHelper = new CspDirectiveOverrideHelper();
        }


        [Fact]
        public void GetOverridenCspDirectiveConfig_NullConfig_ReturnsNewDefaultConfig()
        {
            var directiveConfig = new CspDirectiveConfiguration();
            var directiveOverride = new CspDirectiveOverride { Enabled = directiveConfig.Enabled };

            var newConfig = _overrideHelper.GetOverridenCspDirectiveConfig(directiveOverride, null);

            Assert.NotSame(directiveConfig, newConfig);
            Assert.Equal(newConfig, directiveConfig, new CspDirectiveConfigurationEqualityComparer());
        }

        [Theory]
        [MemberData(nameof(FalseThenTrue))]
        public void GetOverridenCspDirectiveConfig_EnabledOverride_EnabledOverriden(bool expectedResult)
        {
            var directiveConfig = new CspDirectiveConfiguration { Enabled = !expectedResult };
            var directiveOverride = new CspDirectiveOverride { Enabled = expectedResult };

            var newConfig = _overrideHelper.GetOverridenCspDirectiveConfig(directiveOverride, directiveConfig);

            Assert.Equal(expectedResult, newConfig.Enabled);
        }


        [Fact]
        public void GetOverridenCspDirectiveConfig_NoneEnabledOverride_OverridesNoneAndDropsOtherSources()
        {
            //Overriding with 'none' should clear all other sources. 
            var directiveConfig = new CspDirectiveConfiguration
            {
                NoneSrc = false,
                SelfSrc = true,
                Nonce = "hei",
                UnsafeInlineSrc = true,
                UnsafeEvalSrc = true,
                StrictDynamicSrc = true,
                CustomSources = new[] { "nwebsec.com" }
            };
            var directiveOverride = new CspDirectiveOverride { None = true };
            var expectedConfig = new CspDirectiveConfiguration { NoneSrc = true };

            var newConfig = _overrideHelper.GetOverridenCspDirectiveConfig(directiveOverride, directiveConfig);

            Assert.Equal(newConfig, expectedConfig, new CspDirectiveConfigurationEqualityComparer());
        }

        [Fact]
        public void GetOverridenCspDirectiveConfig_NoneDisabledOverride_OverridesNoneAndKeepsOtherSources()
        {
            var directiveConfig = new CspDirectiveConfiguration
            {
                NoneSrc = false,
                SelfSrc = true,
                Nonce = "hei",
                UnsafeInlineSrc = true,
                UnsafeEvalSrc = true,
                StrictDynamicSrc = true,
                CustomSources = new[] { "nwebsec.com" }
            };
            var directiveOverride = new CspDirectiveOverride { None = false };

            var newConfig = _overrideHelper.GetOverridenCspDirectiveConfig(directiveOverride, directiveConfig);

            Assert.Equal(newConfig, directiveConfig, new CspDirectiveConfigurationEqualityComparer());
        }

        [Fact]
        public void GetOverridenCspDirectiveConfig_NoneInheritAndOtherSourcesOverride_OverridesNone()
        {
            //An inherited 'none' should be overriden when other sources are enabled.
            var overrides = new[]
            {
                new CspDirectiveOverride {Self = true},
                new CspDirectiveOverride {UnsafeInline = true},
                new CspDirectiveOverride {UnsafeEval = true},
                new CspDirectiveOverride {StrictDynamic = true},
                new CspDirectiveOverride {OtherSources = new []{"nwebsec.com"}},

            };

            foreach (var directiveOverride in overrides)
            {
                var directiveConfig = new CspDirectiveConfiguration { NoneSrc = true };
                var newConfig = _overrideHelper.GetOverridenCspDirectiveConfig(directiveOverride, directiveConfig);

                Assert.False(newConfig.NoneSrc);
            }
        }

        [Theory]
        [MemberData(nameof(FalseThenTrue))]
        public void GetOverridenCspDirectiveConfig_SelfOverride_OverridesSelf(bool expectedResult)
        {
            var directiveConfig = new CspDirectiveConfiguration { SelfSrc = !expectedResult };
            var directiveOverride = new CspDirectiveOverride { Self = expectedResult };

            var newConfig = _overrideHelper.GetOverridenCspDirectiveConfig(directiveOverride, directiveConfig);

            Assert.Equal(expectedResult, newConfig.SelfSrc);
        }

        [Theory]
        [MemberData(nameof(FalseThenTrue))]
        public void GetOverridenCspDirectiveConfig_SelfInherit_InheritsSelf(bool expectedResult)
        {
            var directiveConfig = new CspDirectiveConfiguration { SelfSrc = expectedResult };
            var directiveOverride = new CspDirectiveOverride();

            var newConfig = _overrideHelper.GetOverridenCspDirectiveConfig(directiveOverride, directiveConfig);

            Assert.Equal(expectedResult, newConfig.SelfSrc);
        }

        [Theory]
        [MemberData(nameof(FalseThenTrue))]
        public void GetOverridenCspDirectiveConfig_UnsafeInlineOverride_OverridesUnsafeInline(bool expectedResult)
        {
            var directiveConfig = new CspDirectiveConfiguration { UnsafeInlineSrc = !expectedResult };
            var directiveOverride = new CspDirectiveOverride { UnsafeInline = expectedResult };

            var newConfig = _overrideHelper.GetOverridenCspDirectiveConfig(directiveOverride, directiveConfig);

            Assert.Equal(expectedResult, newConfig.UnsafeInlineSrc);
        }

        [Theory]
        [MemberData(nameof(FalseThenTrue))]
        public void GetOverridenCspDirectiveConfig_UnsafeInlineInherit_InheritsUnsafeInline(bool expectedResult)
        {
            var directiveConfig = new CspDirectiveConfiguration { UnsafeInlineSrc = expectedResult };
            var directiveOverride = new CspDirectiveOverride();

            var newConfig = _overrideHelper.GetOverridenCspDirectiveConfig(directiveOverride, directiveConfig);

            Assert.Equal(expectedResult, newConfig.UnsafeInlineSrc);
        }

        [Theory]
        [MemberData(nameof(FalseThenTrue))]
        public void GetOverridenCspDirectiveConfig_UnsafeEvalOverride_OverridesUnsafeEval(bool expectedResult)
        {
            var directiveConfig = new CspDirectiveConfiguration { UnsafeEvalSrc = !expectedResult };
            var directiveOverride = new CspDirectiveOverride { UnsafeEval = expectedResult };

            var newConfig = _overrideHelper.GetOverridenCspDirectiveConfig(directiveOverride, directiveConfig);

            Assert.Equal(expectedResult, newConfig.UnsafeEvalSrc);
        }

        [Theory]
        [MemberData(nameof(FalseThenTrue))]
        public void GetOverridenCspDirectiveConfig_UnsafeEvalInherit_InheritsUnsafeEval(bool expectedResult)
        {
            var directiveConfig = new CspDirectiveConfiguration { UnsafeEvalSrc = expectedResult };
            var directiveOverride = new CspDirectiveOverride();

            var newConfig = _overrideHelper.GetOverridenCspDirectiveConfig(directiveOverride, directiveConfig);

            Assert.Equal(expectedResult, newConfig.UnsafeEvalSrc);
        }

        [Theory]
        [MemberData(nameof(FalseThenTrue))]
        public void GetOverridenCspDirectiveConfig_StrictDynamicOverride_OverridesStrictDynamic(bool expectedResult)
        {
            var directiveConfig = new CspDirectiveConfiguration { StrictDynamicSrc = !expectedResult };
            var directiveOverride = new CspDirectiveOverride { StrictDynamic = expectedResult };

            var newConfig = _overrideHelper.GetOverridenCspDirectiveConfig(directiveOverride, directiveConfig);

            Assert.Equal(expectedResult, newConfig.StrictDynamicSrc);
        }

        [Theory]
        [MemberData(nameof(FalseThenTrue))]
        public void GetOverridenCspDirectiveConfig_StrictDynamicInherit_InheritsStrictDynamic(bool expectedResult)
        {
            var directiveConfig = new CspDirectiveConfiguration { StrictDynamicSrc = expectedResult };
            var directiveOverride = new CspDirectiveOverride();

            var newConfig = _overrideHelper.GetOverridenCspDirectiveConfig(directiveOverride, directiveConfig);

            Assert.Equal(expectedResult, newConfig.StrictDynamicSrc);
        }

        [Fact]
        public void GetOverridenCspDirectiveConfig_NoCustomSourcesOverride_KeepsCustomSources()
        {
            var expectedSources = new[] { "www.nwebsec.com" };
            var directiveConfig = new CspDirectiveConfiguration { CustomSources = expectedSources };
            var directiveOverride = new CspDirectiveOverride { InheritOtherSources = true };

            var newConfig = _overrideHelper.GetOverridenCspDirectiveConfig(directiveOverride, directiveConfig);

            Assert.True(expectedSources.SequenceEqual(newConfig.CustomSources), "CustomSources differed.");
        }

        [Fact]
        public void GetOverridenCspDirectiveConfig_CustomSourcesOverride_OverriddesCustomSources()
        {
            var directiveConfig = new CspDirectiveConfiguration { CustomSources = new[] { "www.nwebsec.com" } };
            var directiveOverride = new CspDirectiveOverride { OtherSources = new[] { "*.nwebsec.com" }, InheritOtherSources = false };

            var newConfig = _overrideHelper.GetOverridenCspDirectiveConfig(directiveOverride, directiveConfig);

            Assert.False(newConfig.SelfSrc);
            Assert.True(newConfig.CustomSources.Count() == 1);
            Assert.Equal("*.nwebsec.com", newConfig.CustomSources.First());
        }

        [Fact]
        public void GetOverridenCspDirectiveConfig_CustomSourcesOverrideWithSourcesInherited_KeepsAllSources()
        {
            var directiveConfig = new CspDirectiveConfiguration { CustomSources = new[] { "transformtool.codeplex.com", "nwebsec.codeplex.com" } };
            var directiveOverride = new CspDirectiveOverride { OtherSources = new[] { "nwebsec.codeplex.com" }, InheritOtherSources = true };

            var newConfig = _overrideHelper.GetOverridenCspDirectiveConfig(directiveOverride, directiveConfig);

            Assert.Equal(2, newConfig.CustomSources.Count());
            Assert.Contains("transformtool.codeplex.com", newConfig.CustomSources.ToList());
            Assert.Contains("nwebsec.codeplex.com", newConfig.CustomSources.ToList());
        }

        [Fact]
        public void GetOverridenCspPluginTypesConfig_NullConfig_ReturnsNewDefaultConfig()
        {
            var directiveConfig = new CspPluginTypesDirectiveConfiguration();
            var directiveOverride = new CspPluginTypesOverride { Enabled = directiveConfig.Enabled };

            var newConfig = _overrideHelper.GetOverridenCspPluginTypesConfig(directiveOverride, null);

            Assert.NotSame(directiveConfig, newConfig);
            Assert.Equal(newConfig, directiveConfig, new CspPluginTypesDirectiveConfigurationEqualityComparer());
        }

        [Theory]
        [MemberData(nameof(FalseThenTrue))]
        public void GetOverridenCspPluginTypesConfig_EnabledOverride_EnabledOverriden(bool expectedResult)
        {
            var directiveConfig = new CspPluginTypesDirectiveConfiguration { Enabled = !expectedResult };
            var directiveOverride = new CspPluginTypesOverride { Enabled = expectedResult };

            var newConfig = _overrideHelper.GetOverridenCspPluginTypesConfig(directiveOverride, directiveConfig);

            Assert.Equal(expectedResult, newConfig.Enabled);
        }

        [Fact]
        public void GetOverridenCspPluginTypesConfig_NoMediaTypesOverride_KeepsMediaTypes()
        {
            var expectedMediaTypes = new[] { "application/pdf" };
            var directiveConfig = new CspPluginTypesDirectiveConfiguration { MediaTypes = expectedMediaTypes };
            var directiveOverride = new CspPluginTypesOverride { InheritMediaTypes = true };

            var newConfig = _overrideHelper.GetOverridenCspPluginTypesConfig(directiveOverride, directiveConfig);

            Assert.True(expectedMediaTypes.SequenceEqual(newConfig.MediaTypes), "MediaTypes differed.");
        }

        [Fact]
        public void GetOverridenCspPluginTypesConfig_MediaTypesOverride_OverriddesMediaTypes()
        {
            var directiveConfig = new CspPluginTypesDirectiveConfiguration { MediaTypes = new[] { "application/pdf" } };
            var directiveOverride = new CspPluginTypesOverride { MediaTypes = new[] { "image/png" }, InheritMediaTypes = false };

            var newConfig = _overrideHelper.GetOverridenCspPluginTypesConfig(directiveOverride, directiveConfig);

            Assert.True(newConfig.MediaTypes.Count() == 1);
            Assert.Equal("image/png", newConfig.MediaTypes.First());
        }

        [Fact]
        public void GetOverridenCspPluginTypesConfig_MediaTypesOverrideWithMediaTypesInherited_KeepsAllMediaTypes()
        {
            var directiveConfig = new CspPluginTypesDirectiveConfiguration { MediaTypes = new[] { "application/pdf", "image/png" } };
            var directiveOverride = new CspPluginTypesOverride { MediaTypes = new[] { "image/png" }, InheritMediaTypes = true };

            var newConfig = _overrideHelper.GetOverridenCspPluginTypesConfig(directiveOverride, directiveConfig);

            Assert.Equal(2, newConfig.MediaTypes.Count());
            Assert.Contains("application/pdf", newConfig.MediaTypes.ToList());
            Assert.Contains("image/png", newConfig.MediaTypes.ToList());
        }

        [Theory]
        [MemberData(nameof(FalseThenTrue))]
        public void GetOverridenCspSandboxConfig_EnableOverride_OverridesEnabled(bool expectedResult)
        {
            var directiveConfig = new CspSandboxDirectiveConfiguration { Enabled = !expectedResult };
            var directiveOverride = new CspSandboxOverride { Enabled = expectedResult };

            var newConfig = _overrideHelper.GetOverridenCspSandboxConfig(directiveOverride, directiveConfig);

            Assert.Equal(expectedResult, newConfig.Enabled);
        }

        [Theory]
        [MemberData(nameof(FalseThenTrue))]
        public void GetOverridenCspSandboxConfig_AllowDownloadsOverride_OverridesAllowDownloads(bool expectedResult)
        {
            var directiveConfig = new CspSandboxDirectiveConfiguration { AllowDownloads = !expectedResult };
            var directiveOverride = new CspSandboxOverride { AllowDownloads = expectedResult };

            var newConfig = _overrideHelper.GetOverridenCspSandboxConfig(directiveOverride, directiveConfig);

            Assert.Equal(expectedResult, newConfig.AllowDownloads);
        }

        [Theory]
        [MemberData(nameof(FalseThenTrue))]
        public void GetOverridenCspSandboxConfig_AllowDownloadsNotSet_InheritsAllowDownloads(bool expectedResult)
        {
            var directiveConfig = new CspSandboxDirectiveConfiguration { AllowDownloads = expectedResult };
            var directiveOverride = new CspSandboxOverride();

            var newConfig = _overrideHelper.GetOverridenCspSandboxConfig(directiveOverride, directiveConfig);

            Assert.Equal(expectedResult, newConfig.AllowDownloads);
        }

        [Theory]
        [MemberData(nameof(FalseThenTrue))]
        public void GetOverridenCspSandboxConfig_AllowFormsOverride_OverridesAllowForms(bool expectedResult)
        {
            var directiveConfig = new CspSandboxDirectiveConfiguration { AllowForms = !expectedResult };
            var directiveOverride = new CspSandboxOverride { AllowForms = expectedResult };

            var newConfig = _overrideHelper.GetOverridenCspSandboxConfig(directiveOverride, directiveConfig);

            Assert.Equal(expectedResult, newConfig.AllowForms);
        }

        [Theory]
        [MemberData(nameof(FalseThenTrue))]
        public void GetOverridenCspSandboxConfig_AllowFormsNotSet_InheritsAllowForms(bool expectedResult)
        {
            var directiveConfig = new CspSandboxDirectiveConfiguration { AllowForms = expectedResult };
            var directiveOverride = new CspSandboxOverride();

            var newConfig = _overrideHelper.GetOverridenCspSandboxConfig(directiveOverride, directiveConfig);

            Assert.Equal(expectedResult, newConfig.AllowForms);
        }

        [Theory]
        [MemberData(nameof(FalseThenTrue))]
        public void GetOverridenCspSandboxConfig_AllowModalsOverride_OverridesAllowModals(bool expectedResult)
        {
            var directiveConfig = new CspSandboxDirectiveConfiguration { AllowModals = !expectedResult };
            var directiveOverride = new CspSandboxOverride { AllowModals = expectedResult };

            var newConfig = _overrideHelper.GetOverridenCspSandboxConfig(directiveOverride, directiveConfig);

            Assert.Equal(expectedResult, newConfig.AllowModals);
        }

        [Theory]
        [MemberData(nameof(FalseThenTrue))]
        public void GetOverridenCspSandboxConfig_AllowModalsNotSet_InheritsAllowModals(bool expectedResult)
        {
            var directiveConfig = new CspSandboxDirectiveConfiguration { AllowModals = expectedResult };
            var directiveOverride = new CspSandboxOverride();

            var newConfig = _overrideHelper.GetOverridenCspSandboxConfig(directiveOverride, directiveConfig);

            Assert.Equal(expectedResult, newConfig.AllowModals);
        }

        [Theory]
        [MemberData(nameof(FalseThenTrue))]
        public void GetOverridenCspSandboxConfig_AllowOrientationLockOverride_OverridesAllowOrientationLock(bool expectedResult)
        {
            var directiveConfig = new CspSandboxDirectiveConfiguration { AllowOrientationLock = !expectedResult };
            var directiveOverride = new CspSandboxOverride { AllowOrientationLock = expectedResult };

            var newConfig = _overrideHelper.GetOverridenCspSandboxConfig(directiveOverride, directiveConfig);

            Assert.Equal(expectedResult, newConfig.AllowOrientationLock);
        }

        [Theory]
        [MemberData(nameof(FalseThenTrue))]
        public void GetOverridenCspSandboxConfig_AllowOrientationLockNotSet_InheritsAllowOrientationLock(bool expectedResult)
        {
            var directiveConfig = new CspSandboxDirectiveConfiguration { AllowOrientationLock = expectedResult };
            var directiveOverride = new CspSandboxOverride();

            var newConfig = _overrideHelper.GetOverridenCspSandboxConfig(directiveOverride, directiveConfig);

            Assert.Equal(expectedResult, newConfig.AllowOrientationLock);
        }

        [Theory]
        [MemberData(nameof(FalseThenTrue))]
        public void GetOverridenCspSandboxConfig_AllowPointerLockOverride_OverridesAllowPointerLock(bool expectedResult)
        {
            var directiveConfig = new CspSandboxDirectiveConfiguration { AllowPointerLock = !expectedResult };
            var directiveOverride = new CspSandboxOverride { AllowPointerLock = expectedResult };

            var newConfig = _overrideHelper.GetOverridenCspSandboxConfig(directiveOverride, directiveConfig);

            Assert.Equal(expectedResult, newConfig.AllowPointerLock);
        }

        [Theory]
        [MemberData(nameof(FalseThenTrue))]
        public void GetOverridenCspSandboxConfig_AllowPointerLockNotSet_InheritsAllowPointerLock(bool expectedResult)
        {
            var directiveConfig = new CspSandboxDirectiveConfiguration { AllowPointerLock = expectedResult };
            var directiveOverride = new CspSandboxOverride();

            var newConfig = _overrideHelper.GetOverridenCspSandboxConfig(directiveOverride, directiveConfig);

            Assert.Equal(expectedResult, newConfig.AllowPointerLock);
        }

        [Theory]
        [MemberData(nameof(FalseThenTrue))]
        public void GetOverridenCspSandboxConfig_AllowPopupsOverride_OverridesAllowPopups(bool expectedResult)
        {
            var directiveConfig = new CspSandboxDirectiveConfiguration { AllowPopups = !expectedResult };
            var directiveOverride = new CspSandboxOverride { AllowPopups = expectedResult };

            var newConfig = _overrideHelper.GetOverridenCspSandboxConfig(directiveOverride, directiveConfig);

            Assert.Equal(expectedResult, newConfig.AllowPopups);
        }

        [Theory]
        [MemberData(nameof(FalseThenTrue))]
        public void GetOverridenCspSandboxConfig_AllowPopupsNotSet_InheritsAllowPopups(bool expectedResult)
        {
            var directiveConfig = new CspSandboxDirectiveConfiguration { AllowPopups = expectedResult };
            var directiveOverride = new CspSandboxOverride();

            var newConfig = _overrideHelper.GetOverridenCspSandboxConfig(directiveOverride, directiveConfig);

            Assert.Equal(expectedResult, newConfig.AllowPopups);
        }

        [Theory]
        [MemberData(nameof(FalseThenTrue))]
        public void GetOverridenCspSandboxConfig_AllowPopupsToEscapeSandboxOverride_OverridesAllowPopupsToEscapeSandbox(bool expectedResult)
        {
            var directiveConfig = new CspSandboxDirectiveConfiguration { AllowPopupsToEscapeSandbox = !expectedResult };
            var directiveOverride = new CspSandboxOverride { AllowPopupsToEscapeSandbox = expectedResult };

            var newConfig = _overrideHelper.GetOverridenCspSandboxConfig(directiveOverride, directiveConfig);

            Assert.Equal(expectedResult, newConfig.AllowPopupsToEscapeSandbox);
        }

        [Theory]
        [MemberData(nameof(FalseThenTrue))]
        public void GetOverridenCspSandboxConfig_AllowPopupsToEscapeSandboxNotSet_InheritsAllowPopupsToEscapeSandbox(bool expectedResult)
        {
            var directiveConfig = new CspSandboxDirectiveConfiguration { AllowPopupsToEscapeSandbox = expectedResult };
            var directiveOverride = new CspSandboxOverride();

            var newConfig = _overrideHelper.GetOverridenCspSandboxConfig(directiveOverride, directiveConfig);

            Assert.Equal(expectedResult, newConfig.AllowPopupsToEscapeSandbox);
        }

        [Theory]
        [MemberData(nameof(FalseThenTrue))]
        public void GetOverridenCspSandboxConfig_AllowPresentationOverride_OverridesAllowPresentation(bool expectedResult)
        {
            var directiveConfig = new CspSandboxDirectiveConfiguration { AllowPresentation = !expectedResult };
            var directiveOverride = new CspSandboxOverride { AllowPresentation = expectedResult };

            var newConfig = _overrideHelper.GetOverridenCspSandboxConfig(directiveOverride, directiveConfig);

            Assert.Equal(expectedResult, newConfig.AllowPresentation);
        }

        [Theory]
        [MemberData(nameof(FalseThenTrue))]
        public void GetOverridenCspSandboxConfig_AllowPresentationNotSet_InheritsAllowPresentation(bool expectedResult)
        {
            var directiveConfig = new CspSandboxDirectiveConfiguration { AllowPresentation = expectedResult };
            var directiveOverride = new CspSandboxOverride();

            var newConfig = _overrideHelper.GetOverridenCspSandboxConfig(directiveOverride, directiveConfig);

            Assert.Equal(expectedResult, newConfig.AllowPresentation);
        }

        [Theory]
        [MemberData(nameof(FalseThenTrue))]
        public void GetOverridenCspSandboxConfig_AllowSameOriginOverride_OverridesAllowSameOrigin(bool expectedResult)
        {
            var directiveConfig = new CspSandboxDirectiveConfiguration { AllowSameOrigin = !expectedResult };
            var directiveOverride = new CspSandboxOverride { AllowSameOrigin = expectedResult };

            var newConfig = _overrideHelper.GetOverridenCspSandboxConfig(directiveOverride, directiveConfig);

            Assert.Equal(expectedResult, newConfig.AllowSameOrigin);
        }

        [Theory]
        [MemberData(nameof(FalseThenTrue))]
        public void GetOverridenCspSandboxConfig_AllowSameOriginNotSet_InheritsAllowSameOrigin(bool expectedResult)
        {
            var directiveConfig = new CspSandboxDirectiveConfiguration { AllowSameOrigin = expectedResult };
            var directiveOverride = new CspSandboxOverride();

            var newConfig = _overrideHelper.GetOverridenCspSandboxConfig(directiveOverride, directiveConfig);

            Assert.Equal(expectedResult, newConfig.AllowSameOrigin);
        }

        [Theory]
        [MemberData(nameof(FalseThenTrue))]
        public void GetOverridenCspSandboxConfig_AllowScriptsOverride_OverridesAllowScripts(bool expectedResult)
        {
            var directiveConfig = new CspSandboxDirectiveConfiguration { AllowScripts = !expectedResult };
            var directiveOverride = new CspSandboxOverride { AllowScripts = expectedResult };

            var newConfig = _overrideHelper.GetOverridenCspSandboxConfig(directiveOverride, directiveConfig);

            Assert.Equal(expectedResult, newConfig.AllowScripts);
        }

        [Theory]
        [MemberData(nameof(FalseThenTrue))]
        public void GetOverridenCspSandboxConfig_AllowScriptsNotSet_InheritsAllowScripts(bool expectedResult)
        {
            var directiveConfig = new CspSandboxDirectiveConfiguration { AllowScripts = expectedResult };
            var directiveOverride = new CspSandboxOverride();

            var newConfig = _overrideHelper.GetOverridenCspSandboxConfig(directiveOverride, directiveConfig);

            Assert.Equal(expectedResult, newConfig.AllowScripts);
        }

        [Theory]
        [MemberData(nameof(FalseThenTrue))]
        public void GetOverridenCspSandboxConfig_AllowTopNavigationOverride_OverridesAllowTopNavigation(bool expectedResult)
        {
            var directiveConfig = new CspSandboxDirectiveConfiguration { AllowTopNavigation = !expectedResult };
            var directiveOverride = new CspSandboxOverride { AllowTopNavigation = expectedResult };

            var newConfig = _overrideHelper.GetOverridenCspSandboxConfig(directiveOverride, directiveConfig);

            Assert.Equal(expectedResult, newConfig.AllowTopNavigation);
        }

        [Theory]
        [MemberData(nameof(FalseThenTrue))]
        public void GetOverridenCspSandboxConfig_AllowTopNavigationNotSet_InheritsAllowTopNavigation(bool expectedResult)
        {
            var directiveConfig = new CspSandboxDirectiveConfiguration { AllowTopNavigation = expectedResult };
            var directiveOverride = new CspSandboxOverride();

            var newConfig = _overrideHelper.GetOverridenCspSandboxConfig(directiveOverride, directiveConfig);

            Assert.Equal(expectedResult, newConfig.AllowTopNavigation);
        }

        [Theory]
        [MemberData(nameof(FalseThenTrue))]
        public void GetOverridenCspMixedContentConfig_EnableOverride_OverridesEnabled(bool expectedResult)
        {
            var directiveConfig = new CspMixedContentDirectiveConfiguration { Enabled = !expectedResult };
            var directiveOverride = new CspMixedContentOverride { Enabled = expectedResult };

            var newConfig = _overrideHelper.GetOverridenCspMixedContentConfig(directiveOverride, directiveConfig);

            Assert.Equal(expectedResult, newConfig.Enabled);
        }
    }
}