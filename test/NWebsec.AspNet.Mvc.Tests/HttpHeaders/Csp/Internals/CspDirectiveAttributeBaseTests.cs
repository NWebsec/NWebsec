// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Moq;
using NWebsec.Mvc.HttpHeaders.Csp;
using NWebsec.Mvc.HttpHeaders.Csp.Internals;
using Xunit;

namespace NWebsec.AspNet.Mvc.Tests.HttpHeaders.Csp.Internals
{
    public class CspDirectiveAttributeBaseTests
    {

        [Fact]
        public void ValidateParams_EnabledAndNoDirectives_ThrowsException()
        {
            var cspDirectiveAttribute = new Mock<CspDirectiveAttributeBase>(MockBehavior.Strict).Object;
            Assert.Throws<ArgumentException>(() => cspDirectiveAttribute.ValidateParams());
        }

        [Fact]
        public void ValidateParams_DisabledAndNoDirectives_NoException()
        {
            var cspDirectiveAttribute = new Mock<CspDirectiveAttributeBase>(MockBehavior.Strict).Object;
            cspDirectiveAttribute.Enabled = false;

            cspDirectiveAttribute.ValidateParams();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ValidateParams_HashSourcesEnabled_NoException(bool enabled)
        {
            var cspDirectiveAttribute = new Mock<CspDirectiveAttributeBase>(MockBehavior.Strict).Object;
            Mock.Get(cspDirectiveAttribute).Setup(d => d.EnableHashSources).Returns(true);
            cspDirectiveAttribute.Enabled = enabled;

            cspDirectiveAttribute.CustomSources = "sha256-Kgv+CLuzs+N/onD9CCIVKvqXrFhN+F5GOItmgi/EYd0=";

            cspDirectiveAttribute.ValidateParams();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ValidateParams_HashSourcesDisabled_Throws(bool enabled)
        {
            var cspDirectiveAttribute = new Mock<CspDirectiveAttributeBase>(MockBehavior.Strict).Object;
            Mock.Get(cspDirectiveAttribute).Setup(d => d.EnableHashSources).Returns(false);
            cspDirectiveAttribute.Enabled = enabled;

            Assert.Throws<ArgumentException>(() => cspDirectiveAttribute.CustomSources = "sha256-Kgv+CLuzs+N/onD9CCIVKvqXrFhN+F5GOItmgi/EYd0=");
        }

        [Fact]
        public void ValidateParams_EnabledAndDirectives_NoException()
        {
            foreach (var cspSandboxAttributeBase in ConfiguredAttributes())
            {
                cspSandboxAttributeBase.ValidateParams();
            }
        }

        [Fact]
        public void ValidateParams_EnabledAndMalconfiguredDirectives_Throws()
        {
            foreach (var cspSandboxAttributeBase in MalconfiguredAttributes())
            {
                Assert.Throws<ArgumentException>(() => cspSandboxAttributeBase.ValidateParams());
            }
        }

        private IEnumerable<CspDirectiveAttributeBase> ConfiguredAttributes()
        {
            var attribute = new Mock<CspDirectiveAttributeBase>(MockBehavior.Strict).Object;
            Mock.Get(attribute).Setup(d => d.EnableHashSources).Returns(false);

            attribute.None = true;
            yield return attribute;

            attribute = new Mock<CspDirectiveAttributeBase>(MockBehavior.Strict).Object;
            Mock.Get(attribute).Setup(d => d.EnableHashSources).Returns(false);
            attribute.Self = true;
            yield return attribute;

            yield return new CspStyleSrcAttribute { UnsafeInline = true };
            yield return new CspScriptSrcAttribute { UnsafeEval = true };
            yield return new CspScriptSrcAttribute { StrictDynamic = true };
        }

        private IEnumerable<CspDirectiveAttributeBase> MalconfiguredAttributes()
        {
            var attribute = new Mock<CspDirectiveAttributeBase>(MockBehavior.Strict).Object;
            Mock.Get(attribute).Setup(d => d.EnableHashSources).Returns(false);
            attribute.Self = true;
            attribute.None = true;
            yield return attribute;

            attribute = new Mock<CspDirectiveAttributeBase>(MockBehavior.Strict).Object;
            Mock.Get(attribute).Setup(d => d.EnableHashSources).Returns(false);
            attribute.None = true;
            attribute.CustomSources = "www.nwebsec.com";
            yield return attribute;

            yield return new CspStyleSrcAttribute { None = true, UnsafeInline = true };
            yield return new CspScriptSrcAttribute { None = true, UnsafeEval = true };
            yield return new CspScriptSrcAttribute { None = true, StrictDynamic = true };
        }
    }
}