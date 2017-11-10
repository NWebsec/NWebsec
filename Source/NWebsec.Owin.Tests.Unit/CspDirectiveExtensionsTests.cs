﻿// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Linq;
using NUnit.Framework;

namespace NWebsec.Owin.Tests.Unit
{
    [TestFixture]
    public class CspDirectiveExtensionsTests
    {
        [Test]
        public void None_DirectiveSourcesConfigured_ThrowsException()
        {
            var directiveSelf = new CspDirective();
            var directiveSources = new CspDirective();
            var directiveUnsafeInline = new CspDirective();
            var directiveUnsafeEval = new CspDirective();
            var directiveStrictDynamic = new CspDirective();

            directiveSelf.Self();
            directiveSources.CustomSources("https:");
            directiveUnsafeInline.UnsafeInline();
            directiveUnsafeEval.UnsafeEval();
            directiveStrictDynamic.StrictDynamic();

            Assert.Throws<InvalidOperationException>(directiveSelf.None);
            Assert.Throws<InvalidOperationException>(directiveSources.None);
            Assert.Throws<InvalidOperationException>(directiveUnsafeInline.None);
            Assert.Throws<InvalidOperationException>(directiveUnsafeEval.None);
            Assert.Throws<InvalidOperationException>(directiveStrictDynamic.None);
        }

        [Test]
        public void None_SetsNoneSrc()
        {
            var directive = new CspDirective();

            directive.None();

            Assert.IsTrue(directive.NoneSrc);
        }

        [Test]
        public void Self_SetsSelfSrc()
        {
            var directive = new CspDirective();

            directive.Self();

            Assert.IsTrue(directive.SelfSrc);
        }

        [Test]
        public void CustomSources_ValidSource_SetCustomSources()
        {
            var directive = new CspDirective();

            directive.CustomSources("source1", "source2");

            var expectedResult = new[] { "source1", "source2" };
            Assert.IsTrue(expectedResult.SequenceEqual(directive.CustomSources));
        }

        [Test]
        public void CustomSources_NoParams_ThrowException()
        {
            var directive = new CspDirective();

            Assert.Throws<ArgumentException>(() => directive.CustomSources());
        }

        [Test]
        public void CustomSources_InValidSource_ThrowsException()
        {
            var directive = new CspDirective();

            Assert.Throws<ArgumentException>(() => directive.CustomSources("https:", "nwebsec.*.com"));
        }

        [Test]
        public void UnsafeInline_SetsUnsafeInline()
        {
            var directiveUnsafeInline = new CspDirective();

            directiveUnsafeInline.UnsafeInline();

            Assert.IsTrue(directiveUnsafeInline.UnsafeInlineSrc);
        }

        [Test]
        public void UnsafeEval_SetsUnsafeEval()
        {
            var directiveUnsafeEval = new CspDirective();

            directiveUnsafeEval.UnsafeEval();

            Assert.IsTrue(directiveUnsafeEval.UnsafeEvalSrc);
        }

        [Test]
        public void StrictDynamic_SetsStrictDynamic()
        {
            var directive = new CspDirective();

            directive.StrictDynamic();

            Assert.True(directive.StrictDynamicSrc);
        }
    }
}