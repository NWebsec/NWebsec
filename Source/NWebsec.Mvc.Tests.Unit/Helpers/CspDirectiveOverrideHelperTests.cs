// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using NWebsec.Core.HttpHeaders.Configuration;
using NWebsec.Csp;
using NWebsec.Mvc.Csp;
using NWebsec.Mvc.Helpers;
using NWebsec.Mvc.Tests.Unit.TestHelpers;

namespace NWebsec.Mvc.Tests.Unit.Helpers
{
    [TestFixture]
    public class CspDirectiveOverrideHelperTests
    {
        private CspDirectiveOverrideHelper _overrideHelper;

        public static IEnumerable<Tuple<bool, Source>> SourceCombos = new[] { new Tuple<bool, Source>(true, Source.Enable), new Tuple<bool, Source>(false, Source.Disable) };

        [SetUp]
        public void Setup()
        {
            _overrideHelper = new CspDirectiveOverrideHelper();
        }


        [Test]
        public void GetCspElementWithOverrides_NullConfig_ReturnsNewDefaultConfig()
        {
            var directiveConfig = new CspDirectiveConfiguration();
            var directiveOverride = new CspDirectiveOverride { Enabled = directiveConfig.Enabled };

            var newConfig = _overrideHelper.GetOverridenCspDirectiveConfig(directiveOverride, null);

            Assert.AreNotSame(directiveConfig, newConfig);
            Assert.That(newConfig, Is.EqualTo(directiveConfig).Using(new CspDirectiveComparer()));
        }

        [Test]
        public void GetCspElementWithOverrides_EnabledOverride_EnabledOverriden([Values(true, false)] bool expectedResult)
        {
            var directiveConfig = new CspDirectiveConfiguration { Enabled = !expectedResult };
            var directiveOverride = new CspDirectiveOverride { Enabled = expectedResult };

            var newConfig = _overrideHelper.GetOverridenCspDirectiveConfig(directiveOverride, directiveConfig);

            Assert.AreEqual(expectedResult, newConfig.Enabled);
        }


        [Test]
        public void GetCspElementWithOverrides_NoneOverride_OverridesNone([ValueSource("SourceCombos")] Tuple<bool, Source> combination)
        {
            var expectedResult = combination.Item1;
            var src = combination.Item2;
            var directiveConfig = new CspDirectiveConfiguration { NoneSrc = !expectedResult };
            var directiveOverride = new CspDirectiveOverride { None = src };

            var newConfig = _overrideHelper.GetOverridenCspDirectiveConfig(directiveOverride, directiveConfig);

            Assert.AreEqual(expectedResult, newConfig.NoneSrc);
        }

        [Test]
        public void GetCspElementWithOverrides_NoneInherit_InheritsNone([Values(true, false)] bool expectedResult)
        {
            var directiveConfig = new CspDirectiveConfiguration { NoneSrc = expectedResult };
            var directiveOverride = new CspDirectiveOverride { None = Source.Inherit };

            var newConfig = _overrideHelper.GetOverridenCspDirectiveConfig(directiveOverride, directiveConfig);

            Assert.AreEqual(expectedResult, newConfig.NoneSrc);
        }

        [Test]
        public void GetCspElementWithOverrides_SelfOverride_OverridesSelf([ValueSource("SourceCombos")] Tuple<bool, Source> combination)
        {
            var expectedResult = combination.Item1;
            var src = combination.Item2;
            var directiveConfig = new CspDirectiveConfiguration { SelfSrc = !expectedResult };
            var directiveOverride = new CspDirectiveOverride { Self = src };

            var newConfig = _overrideHelper.GetOverridenCspDirectiveConfig(directiveOverride, directiveConfig);

            Assert.AreEqual(expectedResult, newConfig.SelfSrc);
        }

        [Test]
        public void GetCspElementWithOverrides_SelfInherit_InheritsSelf([Values(true, false)] bool expectedResult)
        {
            var directiveConfig = new CspDirectiveConfiguration { SelfSrc = expectedResult };
            var directiveOverride = new CspDirectiveOverride { Self = Source.Inherit };

            var newConfig = _overrideHelper.GetOverridenCspDirectiveConfig(directiveOverride, directiveConfig);

            Assert.AreEqual(expectedResult, newConfig.SelfSrc);
        }

        [Test]
        public void GetCspElementWithOverrides_UnsafeEvalOverride_OverridesSelf([ValueSource("SourceCombos")] Tuple<bool, Source> combination)
        {
            var expectedResult = combination.Item1;
            var src = combination.Item2;
            var directiveConfig = new CspDirectiveConfiguration { UnsafeEvalSrc = !expectedResult };
            var directiveOverride = new CspDirectiveOverride { UnsafeEval = src };

            var newConfig = _overrideHelper.GetOverridenCspDirectiveConfig(directiveOverride, directiveConfig);

            Assert.AreEqual(expectedResult, newConfig.UnsafeEvalSrc);
        }

        [Test]
        public void GetCspElementWithOverrides_UnsafeEvalInherit_InheritsSelf([Values(true, false)] bool expectedResult)
        {
            var directiveConfig = new CspDirectiveConfiguration { UnsafeEvalSrc = expectedResult };
            var directiveOverride = new CspDirectiveOverride { UnsafeEval = Source.Inherit };

            var newConfig = _overrideHelper.GetOverridenCspDirectiveConfig(directiveOverride, directiveConfig);

            Assert.AreEqual(expectedResult, newConfig.UnsafeEvalSrc);
        }

        [Test]
        public void GetCspElementWithOverrides_UnsafeInlineOverride_OverridesSelf([ValueSource("SourceCombos")] Tuple<bool, Source> combination)
        {
            var expectedResult = combination.Item1;
            var src = combination.Item2;
            var directiveConfig = new CspDirectiveConfiguration { UnsafeInlineSrc = !expectedResult };
            var directiveOverride = new CspDirectiveOverride { UnsafeInline = src };

            var newConfig = _overrideHelper.GetOverridenCspDirectiveConfig(directiveOverride, directiveConfig);

            Assert.AreEqual(expectedResult, newConfig.UnsafeInlineSrc);
        }

        [Test]
        public void GetCspElementWithOverrides_UnsafeInlineInherit_InheritsSelf([Values(true, false)] bool expectedResult)
        {
            var directiveConfig = new CspDirectiveConfiguration { UnsafeInlineSrc = expectedResult };
            var directiveOverride = new CspDirectiveOverride { UnsafeInline = Source.Inherit };

            var newConfig = _overrideHelper.GetOverridenCspDirectiveConfig(directiveOverride, directiveConfig);

            Assert.AreEqual(expectedResult, newConfig.UnsafeInlineSrc);
        }

        [Test]
        public void GetCspElementWithOverrides_NoCustomSourcesOverride_KeepsCustomSources()
        {
            var expectedSources = new[] { "www.nwebsec.com" };
            var directiveConfig = new CspDirectiveConfiguration { CustomSources = expectedSources };
            var directiveOverride = new CspDirectiveOverride { InheritOtherSources = true };

            var newConfig = _overrideHelper.GetOverridenCspDirectiveConfig(directiveOverride, directiveConfig);

            Assert.IsTrue(expectedSources.SequenceEqual(newConfig.CustomSources), "CustomSources differed.");
        }

        [Test]
        public void GetCspElementWithOverrides_CustomSourcesOverride_OverriddesCustomSources()
        {
            var directiveConfig = new CspDirectiveConfiguration { CustomSources = new[] { "www.nwebsec.com" } };
            var directiveOverride = new CspDirectiveOverride { OtherSources = "*.nwebsec.com", InheritOtherSources = false };

            var newConfig = _overrideHelper.GetOverridenCspDirectiveConfig(directiveOverride, directiveConfig);

            Assert.IsFalse(newConfig.SelfSrc);
            Assert.IsTrue(newConfig.CustomSources.Count() == 1);
            Assert.IsTrue(newConfig.CustomSources.First().Equals("*.nwebsec.com"));
        }

        [Test]
        public void GetCspElementWithOverrides_CustomSourcesOverrideWithSourcesInherited_KeepsAllSources()
        {
            var directiveConfig = new CspDirectiveConfiguration { CustomSources = new[] { "transformtool.codeplex.com" } };
            var directiveOverride = new CspDirectiveOverride { OtherSources = "nwebsec.codeplex.com", InheritOtherSources = true };

            var newConfig = _overrideHelper.GetOverridenCspDirectiveConfig(directiveOverride, directiveConfig);

            Assert.AreEqual(2, newConfig.CustomSources.Count());
            Assert.Contains("transformtool.codeplex.com", newConfig.CustomSources.ToList());
            Assert.Contains("nwebsec.codeplex.com", newConfig.CustomSources.ToList());
        }
    }
}