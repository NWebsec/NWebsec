// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using NWebsec.Core.Common.HttpHeaders.Csp;
using Xunit;

namespace NWebsec.Core.SharedProject.Tests.HttpHeaders.Csp
{
    public class CspHashSourceTests
    {

        [Theory]
        [InlineData("sha256-jzgBGA4UWFFmpOBq0JpdsySukE1FrEN5bUpoK8Z29fY=")]
        [InlineData("sha384-0nInIiD/IDiSR40cvI5mC4kWUYcZ97rzjFn7sSlXk9K0fmk4h3pR8kBd/kxj5clZ")]
        [InlineData("sha512-QXvBnRCFCpMpN/RLmlrqoQR0VdRbpJQxJ4EMgh7CUTulqjGK1N4PvI6BtooxhVfHm3fF3lxnGI+w/uQ4r/ssiw==")]
        public void Parse_ValidHashSource_ReturnsSource(string validHashSource)
        {
            var expectedSource = $"'{validHashSource}'";

            var result = CspHashSource.Parse(validHashSource);

            Assert.Equal(expectedSource, result);
        }

        [Theory]
        [InlineData("sha256")]
        [InlineData("sha384")]
        [InlineData("sha512")]
        [InlineData("sha256-")]
        [InlineData("sha384-")]
        [InlineData("sha512-")]
        [InlineData("SHA256-")]
        [InlineData("SHA384-")]
        [InlineData("SHA512-")]
        public void Parse_InValidHashSourcePrefix_ReturnsNull(string validHashSource)
        {
            var result = CspHashSource.Parse(validHashSource);

            Assert.Null(result);
        }

        [Theory]
        [InlineData("sha512-jzgBGA4UWFFmpOBq0JpdsySukE1FrEN5bUpoK8Z29fY=")]
        [InlineData("sha256-0nInIiD/IDiSR40cvI5mC4kWUYcZ97rzjFn7sSlXk9K0fmk4h3pR8kBd/kxj5clZ")]
        [InlineData("sha384-QXvBnRCFCpMpN/RLmlrqoQR0VdRbpJQxJ4EMgh7CUTulqjGK1N4PvI6BtooxhVfHm3fF3lxnGI+w/uQ4r/ssiw==")]
        public void Parse_InvalidHashLength_ReturnsNull(string validHashSource)
        {
            var result = CspHashSource.Parse(validHashSource);

            Assert.Null(result);
        }

        [Theory]
        [InlineData("sha256-===BGA4UWFFmpOBq0JpdsySukE1FrEN5bUpoK8Z29fY=")]
        [InlineData("sha384-===nIiD/IDiSR40cvI5mC4kWUYcZ97rzjFn7sSlXk9K0fmk4h3pR8kBd/kxj5clZ")]
        [InlineData("sha512-===nRCFCpMpN/RLmlrqoQR0VdRbpJQxJ4EMgh7CUTulqjGK1N4PvI6BtooxhVfHm3fF3lxnGI+w/uQ4r/ssiw==")]
        public void Parse_InValidBase64_ReturnsNull(string validHashSource)
        {
            var result = CspHashSource.Parse(validHashSource);

            Assert.Null(result);
        }
    }
}
