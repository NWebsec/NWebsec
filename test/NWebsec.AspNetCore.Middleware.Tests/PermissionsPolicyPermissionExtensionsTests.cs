// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Linq;
using NWebsec.Core.Common.Middleware.Options;
using Xunit;

namespace NWebsec.AspNetCore.Middleware.Tests
{
    public class PermissionsPolicyPermissionExtensionsTests
    {
        [Fact]
        public void None_DirectiveSourcesConfigured_ThrowsException()
        {
            var directiveSelf = new PermissionsPolicyPermission();
            var directiveAll = new PermissionsPolicyPermission();
            var directiveSources = new PermissionsPolicyPermission();

            directiveSelf.Self();
            directiveAll.All();
            directiveSources.CustomSources("https:");

            Assert.Throws<InvalidOperationException>(() => directiveSelf.None());
            Assert.Throws<InvalidOperationException>(() => directiveAll.None());
            Assert.Throws<InvalidOperationException>(() => directiveSources.None());
        }

        [Fact]
        public void None_SetsNoneSrc()
        {
            var directive = new PermissionsPolicyPermission();

            directive.None();

            Assert.True(directive.NoneSrc);
        }

        [Fact]
        public void All_SetsAllSrc()
        {
            var directive = new PermissionsPolicyPermission();

            directive.All();

            Assert.True(directive.AllSrc);
        }

        [Fact]
        public void Self_SetsSelfSrc()
        {
            var directive = new PermissionsPolicyPermission();

            directive.Self();

            Assert.True(directive.SelfSrc);
        }

        [Fact]
        public void CustomSources_ValidSource_SetCustomSources()
        {
            var directive = new PermissionsPolicyPermission();

            directive.CustomSources("source1", "source2");

            var expectedResult = new[] { "source1", "source2" };
            Assert.True(expectedResult.SequenceEqual(directive.CustomSources));
        }

        [Fact]
        public void CustomSources_NoParams_ThrowException()
        {
            var directive = new PermissionsPolicyPermission();

            Assert.Throws<ArgumentException>(() => directive.CustomSources());
        }

        [Fact]
        public void CustomSources_InvalidSource_ThrowsException()
        {
            var directive = new PermissionsPolicyPermission();

            Assert.Throws<ArgumentException>(() => directive.CustomSources("https:", "nwebsec.*.com"));
        }
    }
}
