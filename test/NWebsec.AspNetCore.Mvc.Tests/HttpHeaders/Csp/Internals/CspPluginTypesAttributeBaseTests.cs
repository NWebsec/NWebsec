// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using Moq;
using NUnit.Framework;
using NWebsec.AspNetCore.Mvc.HttpHeaders.Csp.Internals;

namespace NWebsec.AspNetCore.Mvc.Tests.HttpHeaders.Csp.Internals
{
    [TestFixture]
    public class CspPluginTypesAttributeBaseTests
    {

        [Test]
        public void ValidateParams_EnabledAndDirectives_NoException()
        {
            var cspPluginTypesAttributeBaseMock = new Mock<CspPluginTypesAttributeBase>(MockBehavior.Strict).Object;
            cspPluginTypesAttributeBaseMock.MediaTypes = "application/pdf image/gif";

            Assert.DoesNotThrow(() => cspPluginTypesAttributeBaseMock.ValidateParams());
        }

        [Test]
        public void ValidateParams_EnabledAndNoDirectives_ThrowsException()
        {
            var cspPluginTypesAttributeBaseMock = new Mock<CspPluginTypesAttributeBase>(MockBehavior.Strict).Object;
            Assert.Throws<ArgumentException>(() => cspPluginTypesAttributeBaseMock.ValidateParams());
        }

        [Test]
        public void ValidateParams_DisabledAndNoDirectives_NoException()
        {
            var cspPluginTypesAttributeBaseMock = new Mock<CspPluginTypesAttributeBase>(MockBehavior.Strict).Object;
            cspPluginTypesAttributeBaseMock.Enabled = false;

            Assert.DoesNotThrow(() => cspPluginTypesAttributeBaseMock.ValidateParams());
        }

    }
}