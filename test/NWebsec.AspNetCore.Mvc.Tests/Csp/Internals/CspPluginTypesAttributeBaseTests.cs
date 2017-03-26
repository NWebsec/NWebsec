// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using Moq;
using NWebsec.AspNetCore.Mvc.Csp.Internals;
using Xunit;

namespace NWebsec.AspNetCore.Mvc.Tests.Csp.Internals
{
    public class CspPluginTypesAttributeBaseTests
    {

        [Fact]
        public void ValidateParams_EnabledAndDirectives_NoException()
        {
            var cspPluginTypesAttributeBaseMock = new Mock<CspPluginTypesAttributeBase>(MockBehavior.Strict).Object;
            cspPluginTypesAttributeBaseMock.MediaTypes = "application/pdf image/gif";

            cspPluginTypesAttributeBaseMock.ValidateParams();
        }

        [Fact]
        public void ValidateParams_EnabledAndNoDirectives_ThrowsException()
        {
            var cspPluginTypesAttributeBaseMock = new Mock<CspPluginTypesAttributeBase>(MockBehavior.Strict).Object;
            Assert.Throws<ArgumentException>(() => cspPluginTypesAttributeBaseMock.ValidateParams());
        }

        [Fact]
        public void ValidateParams_DisabledAndNoDirectives_NoException()
        {
            var cspPluginTypesAttributeBaseMock = new Mock<CspPluginTypesAttributeBase>(MockBehavior.Strict).Object;
            cspPluginTypesAttributeBaseMock.Enabled = false;

            cspPluginTypesAttributeBaseMock.ValidateParams();
        }

    }
}