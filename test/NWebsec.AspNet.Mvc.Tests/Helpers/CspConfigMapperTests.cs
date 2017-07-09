// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Linq;
using NWebsec.AspNet.Mvc.Tests.TestHelpers;
using NWebsec.Core.Common.HttpHeaders.Configuration;
using NWebsec.Mvc.Csp;
using NWebsec.Mvc.Helpers;
using Xunit;

namespace NWebsec.AspNet.Mvc.Tests.Helpers
{
    public class CspConfigMapperTests
    {
        private readonly CspConfigMapper _mapper;

        public CspConfigMapperTests()
        {
            _mapper = new CspConfigMapper();
        }

        [Theory, ClassData(typeof(CspCommonDirectivesData))]
        public void GetCspDirectiveConfig_CommonCspDirectives_NoException(CspDirectives directive)
        {
            var config = new CspConfiguration();

            _mapper.GetCspDirectiveConfig(config, directive);
        }

        [Theory, ClassData(typeof(CspCommonDirectivesData))]
        public void SetCspDirectiveConfig_CommonCspDirectives_NoException(CspDirectives directive)
        {
            var config = new CspConfiguration();
            var directiveConfig = new CspDirectiveConfiguration();

            _mapper.SetCspDirectiveConfig(config, directive, directiveConfig);
        }

        [Fact]
        public void GetCspDirectiveConfig_DirectiveSet_ReturnsDirective()
        {
            var directives = new CspCommonDirectives().ToArray();
            var config = new CspConfiguration(false);

            foreach (var directive in directives)
            {
                _mapper.SetCspDirectiveConfig(config, directive, new CspDirectiveConfiguration { Nonce = directive.ToString() });
            }

            foreach (var directive in directives)
            {
                var directiveConfig = _mapper.GetCspDirectiveConfig(config, directive);
                Assert.NotNull(directiveConfig);
                Assert.Equal(directive.ToString(), directiveConfig.Nonce);
            }
        }

        [Fact]
        public void GetCspDirectiveConfigCloned_NoDirective_ReturnsNull()
        {
            var config = new CspConfiguration(false);
            var mapper = new CspConfigMapper();

            var clone = mapper.GetCspDirectiveConfigCloned(config, CspDirectives.ScriptSrc);

            Assert.Null(clone);
        }

        [Fact]
        public void GetCspDirectiveConfigCloned_DefaultDirective_ClonesDirective()
        {
            var directive = new CspDirectiveConfiguration();

            var config = new CspConfiguration(false) { ScriptSrcDirective = directive };
            var mapper = new CspConfigMapper();

            var clone = mapper.GetCspDirectiveConfigCloned(config, CspDirectives.ScriptSrc);


            Assert.NotSame(directive, clone);
            Assert.Equal(directive, clone, new CspDirectiveConfigurationEqualityComparer());
        }

        [Fact]
        public void GetCspDirectiveConfigCloned_Configured_ClonesDirective()
        {
            var directive = new CspDirectiveConfiguration
            {
                Enabled = false,
                NoneSrc = true,
                SelfSrc = true,
                UnsafeEvalSrc = true,
                UnsafeInlineSrc = false,
                CustomSources = new[] { "https://www.nwebsec.com", "www.klings.org" }
            };

            var config = new CspConfiguration(false) { ScriptSrcDirective = directive };
            var mapper = new CspConfigMapper();

            var clone = mapper.GetCspDirectiveConfigCloned(config, CspDirectives.ScriptSrc);

            Assert.NotSame(directive, clone);
            Assert.Equal(directive, clone, new CspDirectiveConfigurationEqualityComparer());
        }

        [Fact]
        public void GetCspPluginTypesConfigCloned_NoDirective_ReturnsNull()
        {
            var config = new CspConfiguration(false);
            var mapper = new CspConfigMapper();

            var clone = mapper.GetCspPluginTypesConfigCloned(config);

            Assert.Null(clone);
        }

        [Fact]
        public void GetCspPluginTypesConfigCloned_Configured_ClonesDirective()
        {
            var firstDirective = new CspPluginTypesDirectiveConfiguration()
            {
                Enabled = false,
                MediaTypes = new[] { "application/pdf" }
            };
            var firstConfig = new CspConfiguration(false) { PluginTypesDirective = firstDirective };

            var secondDirective = new CspPluginTypesDirectiveConfiguration()
            {
                Enabled = true,
                MediaTypes = new[] { "image/png", "application/pdf" }
            };
            var secondConfig = new CspConfiguration(false) { PluginTypesDirective = secondDirective };
            var mapper = new CspConfigMapper();

            var firstResult = mapper.GetCspPluginTypesConfigCloned(firstConfig);
            var secondResult = mapper.GetCspPluginTypesConfigCloned(secondConfig);

            Assert.Equal(firstDirective, firstResult, new CspPluginTypesDirectiveConfigurationEqualityComparer());
            Assert.Equal(secondDirective, secondResult, new CspPluginTypesDirectiveConfigurationEqualityComparer());
        }

        [Fact]
        public void GetCspSandboxConfigCloned_NoDirective_ReturnsNull()
        {
            var config = new CspConfiguration(false);
            var mapper = new CspConfigMapper();

            var clone = mapper.GetCspSandboxConfigCloned(config);

            Assert.Null(clone);
        }

        [Fact]
        public void GetCspSandboxConfigCloned_Configured_ClonesDirective()
        {
            var firstDirective = new CspSandboxDirectiveConfiguration
            {
                AllowForms = true,
                AllowModals = true,
                AllowOrientationLock = true,
                AllowPointerLock = true,
                AllowPopups = true
            };
            var firstConfig = new CspConfiguration(false) { SandboxDirective = firstDirective };
            var secondDirective = new CspSandboxDirectiveConfiguration()
            {
                AllowPopupsToEscapeSandbox = true,
                AllowPresentation = true,
                AllowSameOrigin = true,
                AllowScripts = true,
                AllowTopNavigation = true,
                Enabled = true
            };
            var secondConfig = new CspConfiguration(false) { SandboxDirective = secondDirective };
            var mapper = new CspConfigMapper();

            var firstResult = mapper.GetCspSandboxConfigCloned(firstConfig);
            var secondResult = mapper.GetCspSandboxConfigCloned(secondConfig);

            Assert.Equal(firstDirective, firstResult, new CspSandboxDirectiveConfigurationEqualityComparer());
            Assert.Equal(secondDirective, secondResult, new CspSandboxDirectiveConfigurationEqualityComparer());
        }

        [Fact]
        public void GetCspMixedContentConfigCloned_NoDirective_ReturnsNull()
        {
            var config = new CspConfiguration(false);
            var mapper = new CspConfigMapper();

            var clone = mapper.GetCspMixedContentConfigCloned(config);

            Assert.Null(clone);
        }

        [Theory]
        [InlineData(true), InlineData(false)]
        public void GetCspMixedContentConfigCloned_Configured_ClonesDirective(bool enabled)
        {
            var directive = new CspMixedContentDirectiveConfiguration { Enabled = enabled };
            var cspConfig = new CspConfiguration(false) { MixedContentDirective = directive };

            var mapper = new CspConfigMapper();

            var result = mapper.GetCspMixedContentConfigCloned(cspConfig);

            Assert.NotNull(result);
            Assert.NotSame(directive, result);
            Assert.Equal(directive.Enabled, result.Enabled);
        }

        [Fact]
        public void MergeConfiguration_SourceAndTargetDirectivesNotConfigured_MergesHeaderAttributesAndInitializesDirectives()
        {
            var sourceConfig = new CspConfiguration(false) { Enabled = false };
            var destinationConfig = new CspConfiguration(false) { Enabled = true };

            _mapper.MergeConfiguration(sourceConfig, destinationConfig);

            var directives = new CspCommonDirectives().ToArray();

            Assert.False(destinationConfig.Enabled);

            foreach (var directive in directives)
            {
                Assert.NotNull(_mapper.GetCspDirectiveConfig(destinationConfig, directive));
            }

            Assert.NotNull(destinationConfig.PluginTypesDirective);
            Assert.NotNull(destinationConfig.SandboxDirective);
            Assert.NotNull(destinationConfig.UpgradeInsecureRequestsDirective);
            Assert.NotNull(destinationConfig.MixedContentDirective);
            Assert.NotNull(destinationConfig.ReportUriDirective);
        }

        [Fact]
        public void MergeConfiguration_SourceDirectivesConfiguredAndTargetUninitialized_MergesHeaderAttributesAndSourceDirectives()
        {
            var sourceConfig = new CspConfiguration { Enabled = false };
            var destinationConfig = new CspConfiguration(false) { Enabled = true };

            _mapper.MergeConfiguration(sourceConfig, destinationConfig);

            var directives = new CspCommonDirectives().ToArray();

            Assert.False(destinationConfig.Enabled);

            foreach (var directive in directives)
            {
                Assert.Same(_mapper.GetCspDirectiveConfig(sourceConfig, directive), _mapper.GetCspDirectiveConfig(destinationConfig, directive));
            }

            Assert.Same(sourceConfig.PluginTypesDirective, destinationConfig.PluginTypesDirective);
            Assert.Same(sourceConfig.SandboxDirective, destinationConfig.SandboxDirective);
            Assert.Same(sourceConfig.UpgradeInsecureRequestsDirective, destinationConfig.UpgradeInsecureRequestsDirective);
            Assert.Same(sourceConfig.MixedContentDirective, destinationConfig.MixedContentDirective);
            Assert.Same(sourceConfig.ReportUriDirective, destinationConfig.ReportUriDirective);
        }

        [Fact]
        public void MergeConfiguration_SourceDirectivesMissingAndTargetDirectivesConfigured_MergesHeaderAttributesAndKeepsTargetDirectives()
        {
            var directives = new CspCommonDirectives().ToArray();

            var sourceConfig = new CspConfiguration(false) { Enabled = false };
            var destinationConfig = new CspConfiguration { Enabled = true };
            var expectedConfig = new CspConfiguration
            {
                Enabled = destinationConfig.Enabled,
                PluginTypesDirective = destinationConfig.PluginTypesDirective,
                SandboxDirective = destinationConfig.SandboxDirective,
                UpgradeInsecureRequestsDirective = destinationConfig.UpgradeInsecureRequestsDirective,
                MixedContentDirective = destinationConfig.MixedContentDirective,
                ReportUriDirective = destinationConfig.ReportUriDirective
            };
            //Poor man's clone, to get directives from destinationconfig to expected config.
            foreach (var directive in directives)
            {
                _mapper.SetCspDirectiveConfig(expectedConfig, directive, _mapper.GetCspDirectiveConfig(destinationConfig, directive));
            }

            _mapper.MergeConfiguration(sourceConfig, destinationConfig);

            Assert.False(destinationConfig.Enabled);

            foreach (var directive in directives)
            {
                Assert.Same(_mapper.GetCspDirectiveConfig(expectedConfig, directive), _mapper.GetCspDirectiveConfig(destinationConfig, directive));
            }

            Assert.Same(expectedConfig.PluginTypesDirective, destinationConfig.PluginTypesDirective);
            Assert.Same(expectedConfig.SandboxDirective, destinationConfig.SandboxDirective);
            Assert.Same(expectedConfig.UpgradeInsecureRequestsDirective, destinationConfig.UpgradeInsecureRequestsDirective);
            Assert.Same(expectedConfig.MixedContentDirective, destinationConfig.MixedContentDirective);
            Assert.Same(expectedConfig.ReportUriDirective, destinationConfig.ReportUriDirective);
        }

        [Fact]
        public void MergeConfiguration_SourceAndTargetDirectivesConfigured_MergesHeaderAttributesAndSourceDirectives()
        {
            var sourceConfig = new CspConfiguration { Enabled = false };
            var destinationConfig = new CspConfiguration { Enabled = true };

            _mapper.MergeConfiguration(sourceConfig, destinationConfig);

            var directives = new CspCommonDirectives().ToArray();

            Assert.False(destinationConfig.Enabled);

            foreach (var directive in directives)
            {
                Assert.Same(_mapper.GetCspDirectiveConfig(sourceConfig, directive), _mapper.GetCspDirectiveConfig(destinationConfig, directive));
            }

            Assert.Same(sourceConfig.PluginTypesDirective, destinationConfig.PluginTypesDirective);
            Assert.Same(sourceConfig.SandboxDirective, destinationConfig.SandboxDirective);
            Assert.Same(sourceConfig.UpgradeInsecureRequestsDirective, destinationConfig.UpgradeInsecureRequestsDirective);
            Assert.Same(sourceConfig.MixedContentDirective, destinationConfig.MixedContentDirective);
            Assert.Same(sourceConfig.ReportUriDirective, destinationConfig.ReportUriDirective);
        }

        [Fact]
        public void MergeOverrides_HeaderAndDirectivesNotConfigured_InitializesDirectives()
        {
            var sourceConfig = new CspOverrideConfiguration { Enabled = false, EnabledOverride = false };
            var destinationConfig = new CspConfiguration(false) { Enabled = true };

            _mapper.MergeOverrides(sourceConfig, destinationConfig);

            var directives = new CspCommonDirectives().ToArray();

            Assert.True(destinationConfig.Enabled);

            foreach (var directive in directives)
            {
                Assert.NotNull(_mapper.GetCspDirectiveConfig(destinationConfig, directive));
            }

            Assert.NotNull(destinationConfig.PluginTypesDirective);
            Assert.NotNull(destinationConfig.SandboxDirective);
            Assert.NotNull(destinationConfig.UpgradeInsecureRequestsDirective);
            Assert.NotNull(destinationConfig.MixedContentDirective);
            Assert.NotNull(destinationConfig.ReportUriDirective);
        }

        [Fact]
        public void MergeOverrides_HeaderConfiguredAndDirectivesNotConfigured_MergesHeaderConfigAndInitializesDirectives()
        {
            var sourceConfig = new CspOverrideConfiguration { Enabled = false, EnabledOverride = true };
            var destinationConfig = new CspConfiguration(false) { Enabled = true };

            _mapper.MergeOverrides(sourceConfig, destinationConfig);

            var directives = new CspCommonDirectives().ToArray();

            Assert.False(destinationConfig.Enabled);

            foreach (var directive in directives)
            {
                Assert.NotNull(_mapper.GetCspDirectiveConfig(destinationConfig, directive));
            }

            Assert.NotNull(destinationConfig.PluginTypesDirective);
            Assert.NotNull(destinationConfig.SandboxDirective);
            Assert.NotNull(destinationConfig.UpgradeInsecureRequestsDirective);
            Assert.NotNull(destinationConfig.MixedContentDirective);
            Assert.NotNull(destinationConfig.ReportUriDirective);
        }

        [Fact]
        public void MergeOverrides_HeaderNotConfiguredAndDirectivesConfigured_MergesDirectives()
        {
            var directives = new CspCommonDirectives().ToArray();
            var sourceConfig = new CspOverrideConfiguration { Enabled = false, EnabledOverride = false };
            foreach (var directive in directives)
            {
                _mapper.SetCspDirectiveConfig(sourceConfig, directive, new CspDirectiveConfiguration { Nonce = directive.ToString() });
            }

            sourceConfig.PluginTypesDirective = new CspPluginTypesDirectiveConfiguration();
            sourceConfig.SandboxDirective = new CspSandboxDirectiveConfiguration();
            sourceConfig.UpgradeInsecureRequestsDirective = new CspUpgradeDirectiveConfiguration();
            sourceConfig.MixedContentDirective = new CspMixedContentDirectiveConfiguration();
            sourceConfig.ReportUriDirective = new CspReportUriDirectiveConfiguration();
            var destinationConfig = new CspConfiguration(false) { Enabled = true };

            _mapper.MergeOverrides(sourceConfig, destinationConfig);

            Assert.True(destinationConfig.Enabled);

            foreach (var directive in directives)
            {
                var directiveConfig = _mapper.GetCspDirectiveConfig(destinationConfig, directive);
                Assert.NotNull(directiveConfig);
                Assert.Equal(directive.ToString(), directiveConfig.Nonce);
            }

            Assert.Same(sourceConfig.PluginTypesDirective, destinationConfig.PluginTypesDirective);
            Assert.Same(sourceConfig.SandboxDirective, destinationConfig.SandboxDirective);
            Assert.Same(sourceConfig.UpgradeInsecureRequestsDirective, destinationConfig.UpgradeInsecureRequestsDirective);
            Assert.Same(sourceConfig.MixedContentDirective, destinationConfig.MixedContentDirective);
            Assert.Same(sourceConfig.ReportUriDirective, destinationConfig.ReportUriDirective);
        }

        [Fact]
        public void MergeOverrides_HeaderConfiguredAndDirectivesConfigured_MergesHeaderAndDirectives()
        {
            var directives = new CspCommonDirectives().ToArray();
            var sourceConfig = new CspOverrideConfiguration { Enabled = false, EnabledOverride = true };
            foreach (var directive in directives)
            {
                _mapper.SetCspDirectiveConfig(sourceConfig, directive, new CspDirectiveConfiguration { Nonce = directive.ToString() });
            }
            sourceConfig.PluginTypesDirective = new CspPluginTypesDirectiveConfiguration();
            sourceConfig.SandboxDirective = new CspSandboxDirectiveConfiguration();
            sourceConfig.UpgradeInsecureRequestsDirective = new CspUpgradeDirectiveConfiguration();
            sourceConfig.MixedContentDirective = new CspMixedContentDirectiveConfiguration();
            sourceConfig.ReportUriDirective = new CspReportUriDirectiveConfiguration();

            var destinationConfig = new CspConfiguration(false) { Enabled = true };

            _mapper.MergeOverrides(sourceConfig, destinationConfig);

            Assert.False(destinationConfig.Enabled);

            foreach (var directive in directives)
            {
                var directiveConfig = _mapper.GetCspDirectiveConfig(destinationConfig, directive);
                Assert.NotNull(directiveConfig);
                Assert.Equal(directive.ToString(), directiveConfig.Nonce);
            }

            Assert.Same(sourceConfig.PluginTypesDirective, destinationConfig.PluginTypesDirective);
            Assert.Same(sourceConfig.SandboxDirective, destinationConfig.SandboxDirective);
            Assert.Same(sourceConfig.UpgradeInsecureRequestsDirective, destinationConfig.UpgradeInsecureRequestsDirective);
            Assert.Same(sourceConfig.MixedContentDirective, destinationConfig.MixedContentDirective);
            Assert.Same(sourceConfig.ReportUriDirective, destinationConfig.ReportUriDirective);
        }
    }
}