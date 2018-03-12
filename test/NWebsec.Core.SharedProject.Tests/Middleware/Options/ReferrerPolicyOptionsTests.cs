// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using NWebsec.Core.Common.HttpHeaders;
using NWebsec.Core.Common.Middleware.Options;
using Xunit;

namespace NWebsec.Core.SharedProject.Tests.Middleware.Options
{
    public class ReferrerPolicyOptionsTests
    {
        private readonly ReferrerPolicyOptions _options;

        public ReferrerPolicyOptionsTests()
        {
            _options = new ReferrerPolicyOptions();
        }

        [Fact]
        public void NoReferrer_SetsNoReferrer()
        {
            _options.NoReferrer();

            Assert.Equal(ReferrerPolicy.NoReferrer, _options.Policy);
        }

        [Fact]
        public void NoReferrerWhenDowngrade_SetsNoReferrerWhenDowngrade()
        {
            _options.NoReferrerWhenDowngrade();

            Assert.Equal(ReferrerPolicy.NoReferrerWhenDowngrade, _options.Policy);
        }

        [Fact]
        public void SameOrigin()
        {
            _options.SameOrigin();

            Assert.Equal(ReferrerPolicy.SameOrigin, _options.Policy);
        }

        [Fact]
        public void Origin()
        {
            _options.Origin();

            Assert.Equal(ReferrerPolicy.Origin, _options.Policy);
        }

        [Fact]
        public void StrictOrigin()
        {
            _options.StrictOrigin();

            Assert.Equal(ReferrerPolicy.StrictOrigin, _options.Policy);
        }

        [Fact]
        public void OriginWhenCrossOrigin()
        {
            _options.OriginWhenCrossOrigin();

            Assert.Equal(ReferrerPolicy.OriginWhenCrossOrigin, _options.Policy);
        }

        [Fact]
        public void StrictOriginWhenCrossOrigin()
        {
            _options.StrictOriginWhenCrossOrigin();

            Assert.Equal(ReferrerPolicy.StrictOriginWhenCrossOrigin, _options.Policy);
        }

        [Fact]
        public void UnsafeUrl()
        {
            _options.UnsafeUrl();

            Assert.Equal(ReferrerPolicy.UnsafeUrl, _options.Policy);
        }
    }
}