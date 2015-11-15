// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Linq;
using NUnit.Framework;
using NWebsec.Core.HttpHeaders.Configuration;
using NWebsec.Mvc.Csp;
using NWebsec.Mvc.Helpers;
using NWebsec.Mvc.Tests.TestHelpers;

namespace NWebsec.Mvc.Tests.Helpers
{
    [TestFixture]
    public class CspConfigMapperTests
    {
        private CspConfigMapper _mapper;

        [SetUp]
        public void Setup()
        {
            _mapper = new CspConfigMapper();
        }

        [Test]
        public void GetCspDirectiveConfig_CommonCspDirectives_NoException([ValueSource(typeof(CspCommonDirectives), "Directives")] CspDirectives directive)
        {
            var config = new CspConfiguration();

            Assert.DoesNotThrow(() => _mapper.GetCspDirectiveConfig(config, directive));
        }

        [Test]
        public void SetCspDirectiveConfig_CommonCspDirectives_NoException([ValueSource(typeof(CspCommonDirectives), "Directives")] CspDirectives directive)
        {
            var config = new CspConfiguration();
            var directiveConfig = new CspDirectiveConfiguration();

            Assert.DoesNotThrow(() => _mapper.SetCspDirectiveConfig(config, directive, directiveConfig));
        }

        [Test]
        public void GetCspDirectiveConfig_DirectiveSet_ReturnsDirective()
        {
            var directives = CspCommonDirectives.Directives().ToArray();
            var config = new CspConfiguration(false);

            foreach (var directive in directives)
            {
                _mapper.SetCspDirectiveConfig(config, directive, new CspDirectiveConfiguration { Nonce = directive.ToString() });
            }

            foreach (var directive in directives)
            {
                var directiveConfig = _mapper.GetCspDirectiveConfig(config, directive);
                Assert.IsNotNull(directiveConfig);
                Assert.AreEqual(directive.ToString(), directiveConfig.Nonce);
            }
        }

        [Test]
        public void GetCspDirectiveConfigCloned_NoDirective_ReturnsNull()
        {
            var config = new CspConfiguration(false);
            var mapper = new CspConfigMapper();

            var clone = mapper.GetCspDirectiveConfigCloned(config, CspDirectives.ScriptSrc);

            Assert.IsNull(clone);
        }

        [Test]
        public void GetCspDirectiveConfigCloned_DefaultDirective_ClonesDirective()
        {
            var directive = new CspDirectiveConfiguration();

            var config = new CspConfiguration(false) { ScriptSrcDirective = directive };
            var mapper = new CspConfigMapper();

            var clone = mapper.GetCspDirectiveConfigCloned(config, CspDirectives.ScriptSrc);


            Assert.AreNotSame(directive, clone);
            Assert.That(clone, Is.EqualTo(directive).Using(new CspDirectiveConfigurationComparer()));
        }

        [Test]
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

            Assert.AreNotSame(directive, clone);
            Assert.That(clone, Is.EqualTo(directive).Using(new CspDirectiveConfigurationComparer()));
        }

        [Test]
        public void GetCspPluginTypesConfigCloned_NoDirective_ReturnsNull()
        {
            var config = new CspConfiguration(false);
            var mapper = new CspConfigMapper();

            var clone = mapper.GetCspPluginTypesConfigCloned(config);

            Assert.IsNull(clone);
        }

        [Test]
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

            Assert.That(firstResult, Is.EqualTo(firstDirective).Using(new CspPluginTypesDirectiveConfigurationComparer()));
            Assert.That(secondResult, Is.EqualTo(secondDirective).Using(new CspPluginTypesDirectiveConfigurationComparer()));
        }

        [Test]
        public void GetCspSandboxConfigCloned_NoDirective_ReturnsNull()
        {
            var config = new CspConfiguration(false);
            var mapper = new CspConfigMapper();

            var clone = mapper.GetCspSandboxConfigCloned(config);

            Assert.IsNull(clone);
        }

        [Test]
        public void GetCspSandboxConfigCloned_Configured_ClonesDirective()
        {
            var firstDirective = new CspSandboxDirectiveConfiguration
            {
                AllowForms = true,
                AllowPointerLock = true,
                AllowPopups = true
            };
            var firstConfig = new CspConfiguration(false) { SandboxDirective = firstDirective };
            var secondDirective = new CspSandboxDirectiveConfiguration()
            {
                AllowSameOrigin = true,
                AllowScripts = true,
                AllowTopNavigation = true,
                Enabled = true
            };
            var secondConfig = new CspConfiguration(false) { SandboxDirective = secondDirective };
            var mapper = new CspConfigMapper();

            var firstResult = mapper.GetCspSandboxConfigCloned(firstConfig);
            var secondResult = mapper.GetCspSandboxConfigCloned(secondConfig);

            Assert.That(firstResult, Is.EqualTo(firstDirective).Using(new CspSandboxDirectiveConfigurationComparer()));
            Assert.That(secondResult, Is.EqualTo(secondDirective).Using(new CspSandboxDirectiveConfigurationComparer()));
        }

        [Test]
        public void MergeConfiguration_SourceAndTargetDirectivesNotConfigured_MergesHeaderAttributesAndInitializesDirectives()
        {
            var sourceConfig = new CspConfiguration(false) { Enabled = false };
            var destinationConfig = new CspConfiguration(false) { Enabled = true };

            _mapper.MergeConfiguration(sourceConfig, destinationConfig);

            var directives = CspCommonDirectives.Directives().ToArray();

            Assert.IsFalse(destinationConfig.Enabled);

            foreach (var directive in directives)
            {
                Assert.IsNotNull(_mapper.GetCspDirectiveConfig(destinationConfig, directive));
            }

            Assert.IsNotNull(destinationConfig.PluginTypesDirective);
            Assert.IsNotNull(destinationConfig.SandboxDirective);
            Assert.IsNotNull(destinationConfig.UpgradeInsecureRequestsDirective);
            Assert.IsNotNull(destinationConfig.ReportUriDirective);
        }

        [Test]
        public void MergeConfiguration_SourceDirectivesConfiguredAndTargetUninitialized_MergesHeaderAttributesAndSourceDirectives()
        {
            var sourceConfig = new CspConfiguration { Enabled = false };
            var destinationConfig = new CspConfiguration(false) { Enabled = true };

            _mapper.MergeConfiguration(sourceConfig, destinationConfig);

            var directives = CspCommonDirectives.Directives().ToArray();

            Assert.IsFalse(destinationConfig.Enabled);

            foreach (var directive in directives)
            {
                Assert.AreSame(_mapper.GetCspDirectiveConfig(sourceConfig, directive), _mapper.GetCspDirectiveConfig(destinationConfig, directive));
            }

            Assert.AreSame(sourceConfig.PluginTypesDirective, destinationConfig.PluginTypesDirective);
            Assert.AreSame(sourceConfig.SandboxDirective, destinationConfig.SandboxDirective);
            Assert.AreSame(sourceConfig.UpgradeInsecureRequestsDirective, destinationConfig.UpgradeInsecureRequestsDirective);
            Assert.AreSame(sourceConfig.ReportUriDirective, destinationConfig.ReportUriDirective);
        }

        [Test]
        public void MergeConfiguration_SourceDirectivesMissingAndTargetDirectivesConfigured_MergesHeaderAttributesAndKeepsTargetDirectives()
        {
            var directives = CspCommonDirectives.Directives().ToArray();

            var sourceConfig = new CspConfiguration(false) { Enabled = false };
            var destinationConfig = new CspConfiguration { Enabled = true };
            var expectedConfig = new CspConfiguration
            {
                Enabled = destinationConfig.Enabled,
                PluginTypesDirective = destinationConfig.PluginTypesDirective,
                SandboxDirective = destinationConfig.SandboxDirective,
                UpgradeInsecureRequestsDirective = destinationConfig.UpgradeInsecureRequestsDirective,
                ReportUriDirective = destinationConfig.ReportUriDirective
            };
            //Poor man's clone, to get directives from destinationconfig to expected config.
            foreach (var directive in directives)
            {
                _mapper.SetCspDirectiveConfig(expectedConfig, directive, _mapper.GetCspDirectiveConfig(destinationConfig, directive));
            }

            _mapper.MergeConfiguration(sourceConfig, destinationConfig);


            Assert.IsFalse(destinationConfig.Enabled);

            foreach (var directive in directives)
            {
                Assert.AreSame(_mapper.GetCspDirectiveConfig(expectedConfig, directive), _mapper.GetCspDirectiveConfig(destinationConfig, directive));
            }

            Assert.AreSame(expectedConfig.PluginTypesDirective, destinationConfig.PluginTypesDirective);
            Assert.AreSame(expectedConfig.SandboxDirective, destinationConfig.SandboxDirective);
            Assert.AreSame(expectedConfig.UpgradeInsecureRequestsDirective, destinationConfig.UpgradeInsecureRequestsDirective);
            Assert.AreSame(expectedConfig.ReportUriDirective, destinationConfig.ReportUriDirective);
        }

        [Test]
        public void MergeConfiguration_SourceAndTargetDirectivesConfigured_MergesHeaderAttributesAndSourceDirectives()
        {
            var sourceConfig = new CspConfiguration { Enabled = false };
            var destinationConfig = new CspConfiguration { Enabled = true };

            _mapper.MergeConfiguration(sourceConfig, destinationConfig);

            var directives = CspCommonDirectives.Directives().ToArray();

            Assert.IsFalse(destinationConfig.Enabled);

            foreach (var directive in directives)
            {
                Assert.AreSame(_mapper.GetCspDirectiveConfig(sourceConfig, directive), _mapper.GetCspDirectiveConfig(destinationConfig, directive));
            }

            Assert.AreSame(sourceConfig.PluginTypesDirective, destinationConfig.PluginTypesDirective);
            Assert.AreSame(sourceConfig.SandboxDirective, destinationConfig.SandboxDirective);
            Assert.AreSame(sourceConfig.UpgradeInsecureRequestsDirective, destinationConfig.UpgradeInsecureRequestsDirective);
            Assert.AreSame(sourceConfig.ReportUriDirective, destinationConfig.ReportUriDirective);
        }

        [Test]
        public void MergeOverrides_HeaderAndDirectivesNotConfigured_InitializesDirectives()
        {
            var sourceConfig = new CspOverrideConfiguration { Enabled = false, EnabledOverride = false };
            var destinationConfig = new CspConfiguration(false) { Enabled = true };

            _mapper.MergeOverrides(sourceConfig, destinationConfig);

            var directives = CspCommonDirectives.Directives().ToArray();

            Assert.IsTrue(destinationConfig.Enabled);

            foreach (var directive in directives)
            {
                Assert.IsNotNull(_mapper.GetCspDirectiveConfig(destinationConfig, directive));
            }

            Assert.IsNotNull(destinationConfig.PluginTypesDirective);
            Assert.IsNotNull(destinationConfig.SandboxDirective);
            Assert.IsNotNull(destinationConfig.UpgradeInsecureRequestsDirective);
            Assert.IsNotNull(destinationConfig.ReportUriDirective);
        }

        [Test]
        public void MergeOverrides_HeaderConfiguredAndDirectivesNotConfigured_MergesHeaderConfigAndInitializesDirectives()
        {
            var sourceConfig = new CspOverrideConfiguration { Enabled = false, EnabledOverride = true };
            var destinationConfig = new CspConfiguration(false) { Enabled = true };

            _mapper.MergeOverrides(sourceConfig, destinationConfig);

            var directives = CspCommonDirectives.Directives().ToArray();

            Assert.IsFalse(destinationConfig.Enabled);

            foreach (var directive in directives)
            {
                Assert.IsNotNull(_mapper.GetCspDirectiveConfig(destinationConfig, directive));
            }

            Assert.IsNotNull(destinationConfig.PluginTypesDirective);
            Assert.IsNotNull(destinationConfig.SandboxDirective);
            Assert.IsNotNull(destinationConfig.UpgradeInsecureRequestsDirective);
            Assert.IsNotNull(destinationConfig.ReportUriDirective);
        }

        [Test]
        public void MergeOverrides_HeaderNotConfiguredAndDirectivesConfigured_MergesDirectives()
        {
            var directives = CspCommonDirectives.Directives().ToArray();
            var sourceConfig = new CspOverrideConfiguration { Enabled = false, EnabledOverride = false };
            foreach (var directive in directives)
            {
                _mapper.SetCspDirectiveConfig(sourceConfig, directive, new CspDirectiveConfiguration { Nonce = directive.ToString() });
            }

            sourceConfig.PluginTypesDirective = new CspPluginTypesDirectiveConfiguration();
            sourceConfig.SandboxDirective = new CspSandboxDirectiveConfiguration();
            sourceConfig.UpgradeInsecureRequestsDirective = new CspUpgradeDirectiveConfiguration();
            sourceConfig.ReportUriDirective = new CspReportUriDirectiveConfiguration();
            var destinationConfig = new CspConfiguration(false) { Enabled = true };

            _mapper.MergeOverrides(sourceConfig, destinationConfig);

            Assert.IsTrue(destinationConfig.Enabled);

            foreach (var directive in directives)
            {
                var directiveConfig = _mapper.GetCspDirectiveConfig(destinationConfig, directive);
                Assert.IsNotNull(directiveConfig);
                Assert.AreEqual(directive.ToString(), directiveConfig.Nonce);
            }

            Assert.AreSame(sourceConfig.PluginTypesDirective, destinationConfig.PluginTypesDirective);
            Assert.AreSame(sourceConfig.SandboxDirective, destinationConfig.SandboxDirective);
            Assert.AreSame(sourceConfig.UpgradeInsecureRequestsDirective, destinationConfig.UpgradeInsecureRequestsDirective);
            Assert.AreSame(sourceConfig.ReportUriDirective, destinationConfig.ReportUriDirective);
        }

        [Test]
        public void MergeOverrides_HeaderConfiguredAndDirectivesConfigured_MergesHeaderAndDirectives()
        {
            var directives = CspCommonDirectives.Directives().ToArray();
            var sourceConfig = new CspOverrideConfiguration { Enabled = false, EnabledOverride = true };
            foreach (var directive in directives)
            {
                _mapper.SetCspDirectiveConfig(sourceConfig, directive, new CspDirectiveConfiguration { Nonce = directive.ToString() });
            }
            sourceConfig.PluginTypesDirective = new CspPluginTypesDirectiveConfiguration();
            sourceConfig.SandboxDirective = new CspSandboxDirectiveConfiguration();
            sourceConfig.UpgradeInsecureRequestsDirective = new CspUpgradeDirectiveConfiguration();
            sourceConfig.ReportUriDirective = new CspReportUriDirectiveConfiguration();

            var destinationConfig = new CspConfiguration(false) { Enabled = true };

            _mapper.MergeOverrides(sourceConfig, destinationConfig);

            Assert.IsFalse(destinationConfig.Enabled);

            foreach (var directive in directives)
            {
                var directiveConfig = _mapper.GetCspDirectiveConfig(destinationConfig, directive);
                Assert.IsNotNull(directiveConfig);
                Assert.AreEqual(directive.ToString(), directiveConfig.Nonce);
            }

            Assert.AreSame(sourceConfig.PluginTypesDirective, destinationConfig.PluginTypesDirective);
            Assert.AreSame(sourceConfig.SandboxDirective, destinationConfig.SandboxDirective);
            Assert.AreSame(sourceConfig.UpgradeInsecureRequestsDirective, destinationConfig.UpgradeInsecureRequestsDirective);
            Assert.AreSame(sourceConfig.ReportUriDirective, destinationConfig.ReportUriDirective);
        }
    }
}