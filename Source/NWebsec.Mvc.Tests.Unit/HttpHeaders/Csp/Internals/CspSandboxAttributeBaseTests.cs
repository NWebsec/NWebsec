// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using NWebsec.Mvc.HttpHeaders.Csp.Internals;

namespace NWebsec.Mvc.Tests.Unit.HttpHeaders.Csp.Internals
{
    [TestFixture]
    public class CspSandboxAttributeBaseTests
    {
        
        [Test]
        public void ValidateParams_EnabledAndNoDirectives_ThrowsException()
        {
            var cspSandboxAttributeBaseMock = new Mock<CspSandboxAttributeBase>(MockBehavior.Strict).Object;
            Assert.Throws<ApplicationException>(() => cspSandboxAttributeBaseMock.ValidateParams());
        }

        [Test]
        public void ValidateParams_DisabledAndNoDirectives_NoException()
        {
            var cspSandboxAttributeBaseMock = new Mock<CspSandboxAttributeBase>(MockBehavior.Strict).Object;
            cspSandboxAttributeBaseMock.Enabled = false;

            Assert.DoesNotThrow(() => cspSandboxAttributeBaseMock.ValidateParams());
        }

        [Test]
        public void ValidateParams_EnabledAndDirectives_NoException()
        {
            foreach (var cspSandboxAttributeBase in ConfiguredAttributes())
            {
                Assert.DoesNotThrow(() => cspSandboxAttributeBase.ValidateParams());
            }
        }

        private IEnumerable<CspSandboxAttributeBase> ConfiguredAttributes()
        {
            var attribute = new Mock<CspSandboxAttributeBase>(MockBehavior.Strict).Object;
            attribute.AllowForms = true;
            yield return attribute;

            attribute = new Mock<CspSandboxAttributeBase>(MockBehavior.Strict).Object;
            attribute.AllowPointerLock = true;
            yield return attribute;

            attribute = new Mock<CspSandboxAttributeBase>(MockBehavior.Strict).Object;
            attribute.AllowPopups = true;
            yield return attribute;

            attribute = new Mock<CspSandboxAttributeBase>(MockBehavior.Strict).Object;
            attribute.AllowSameOrigin = true;
            yield return attribute;

            attribute = new Mock<CspSandboxAttributeBase>(MockBehavior.Strict).Object;
            attribute.AllowScripts = true;
            yield return attribute;

            attribute = new Mock<CspSandboxAttributeBase>(MockBehavior.Strict).Object;
            attribute.AllowTopNavigation = true;
            yield return attribute;
        }
    }
}