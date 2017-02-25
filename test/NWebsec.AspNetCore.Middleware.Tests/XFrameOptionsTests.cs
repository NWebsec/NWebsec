// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using Xunit;
using NWebsec.AspNetCore.Core.HttpHeaders;

namespace NWebsec.AspNetCore.Middleware.Tests
{
    public class XFrameOptionsTests
    {
        private readonly XFrameOptions _options;

        public XFrameOptionsTests()
        {
            _options = new XFrameOptions();
        }

        [Fact]
        public void Deny_SetsDeny()
        {
            _options.Deny();

            Assert.Equal(XfoPolicy.Deny, _options.Policy);
        }

        [Fact]
        public void SameOrigin_SetsSameOrigin()
        {
            _options.SameOrigin();

            Assert.Equal(XfoPolicy.SameOrigin, _options.Policy);
        }
    }
}