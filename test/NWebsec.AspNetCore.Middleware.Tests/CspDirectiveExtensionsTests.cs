// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Xunit;

namespace NWebsec.AspNetCore.Middleware.Tests
{
    public class CspDirectiveExtensionsTests
    {
        [Fact]
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

            Assert.Throws<InvalidOperationException>(() => directiveSelf.None());
            Assert.Throws<InvalidOperationException>(() => directiveSources.None());
            Assert.Throws<InvalidOperationException>(() => directiveUnsafeInline.None());
            Assert.Throws<InvalidOperationException>(() => directiveUnsafeEval.None());
            Assert.Throws<InvalidOperationException>(() => directiveStrictDynamic.None());
        }

        [Fact]
        public void None_SetsNoneSrc()
        {
            var directive = new CspDirective();

            directive.None();

            Assert.True(directive.NoneSrc);
        }

        [Fact]
        public void Self_SetsSelfSrc()
        {
            var directive = new CspDirective();

            directive.Self();

            Assert.True(directive.SelfSrc);
        }

        [Fact]
        public void CustomSources_ValidSource_SetCustomSources()
        {
            var directive = new CspDirective();

            directive.CustomSources("source1", "source2");

            var expectedResult = new[] { "source1", "source2" };
            Assert.True(expectedResult.SequenceEqual(directive.CustomSources));
        }

        [Fact]
        public void CustomSources_NoParams_ThrowException()
        {
            var directive = new CspDirective();

            Assert.Throws<ArgumentException>(() => directive.CustomSources());
        }

        [Fact]
        public void CustomSources_InValidSource_ThrowsException()
        {
            var directive = new CspDirective();

            Assert.Throws<ArgumentException>(() => directive.CustomSources("https:", "nwebsec.*.com"));
        }

        [Fact]
        public void UnsafeInline_SetsUnsafeInline()
        {
            var directiveUnsafeInline = new CspDirective();

            directiveUnsafeInline.UnsafeInline();

            Assert.True(directiveUnsafeInline.UnsafeInlineSrc);
        }

        [Fact]
        public void UnsafeEval_SetsUnsafeEval()
        {
            var directiveUnsafeEval = new CspDirective();

            directiveUnsafeEval.UnsafeEval();

            Assert.True(directiveUnsafeEval.UnsafeEvalSrc);
        }

        [Fact]
        public void StrictDynamic_SetsStrictDynamic()
        {
            var directive = new CspDirective();

            directive.StrictDynamic();

            Assert.True(directive.StrictDynamicSrc);
        }
    }
}