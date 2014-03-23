// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Linq;
using System.Web;
using Moq;
using NUnit.Framework;
using NWebsec.ExtensionMethods;

namespace NWebsec.Tests.Unit.ExtensionMethods
{
    [TestFixture]
    public class HttpResponseBaseExtensionsTests
    {
        [Test]
        public void HasStatusCodeThatRedirects_ReturnsTrue()
        {
            var redirectStatusCodes =
                Enumerable.Range(300, 304 - 300).Concat(Enumerable.Range(305, 400-305));

            foreach (var redirectStatusCode in redirectStatusCodes)
            {
                var mockContext = new Mock<HttpResponseBase>();
                mockContext.Setup(r => r.StatusCode).Returns(redirectStatusCode);

                Assert.IsTrue(mockContext.Object.HasStatusCodeThatRedirects(), "Failed for statuscode: " + redirectStatusCode);
            }
        }

        [Test]
        public void HasStatusCodeThatDoesNotRedirect_ReturnsFalse()
        {
            var nonRedirectStatusCodes =
                Enumerable.Range(100, 300 - 100).Concat(Enumerable.Range(400, 600 - 400)).Concat(new[] { 304 });

            foreach (var redirectStatusCode in nonRedirectStatusCodes)
            {
                var mockContext = new Mock<HttpResponseBase>();
                mockContext.Setup(r => r.StatusCode).Returns(redirectStatusCode);

                Assert.IsFalse(mockContext.Object.HasStatusCodeThatRedirects(), "Failed for statuscode: " + redirectStatusCode);
            }
        }
    }
}