// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Moq;
using NUnit.Framework;
using NWebsec.AspNetCore.Mvc.Helpers;

namespace NWebsec.AspNetCore.Mvc.TagHelpers.Tests
{
    [TestFixture]
    public class CspNonceTagHelperTests
    {
        private CspNonceTagHelper _tagHelper;
        private Mock<ICspConfigurationOverrideHelper> _cspOverrideHelper;
        private Mock<IHeaderOverrideHelper> _headerOverrideHelper;

        [SetUp]
        public void Setup()
        {
            _cspOverrideHelper = new Mock<ICspConfigurationOverrideHelper>(MockBehavior.Strict);
            _headerOverrideHelper = new Mock<IHeaderOverrideHelper>(MockBehavior.Strict);
            _headerOverrideHelper.Setup(ho => ho.SetCspHeaders(It.IsAny<HttpContext>(), It.IsAny<bool>()));

            _tagHelper = new CspNonceTagHelper(_cspOverrideHelper.Object, _headerOverrideHelper.Object);
            var httpContext = new DefaultHttpContext { Items = new Dictionary<object, object>() };
            _tagHelper.ViewContext = new Microsoft.AspNetCore.Mvc.Rendering.ViewContext() { HttpContext = httpContext };
        }

        [Test]
        public void Process_ScriptTag_AddsNonce()
        {
            _cspOverrideHelper.Setup(co => co.GetCspScriptNonce(It.IsAny<HttpContext>())).Returns("scriptnonce");
            var contextAttributes = new TagHelperAttributeList
                {
                    { "src", "https://script.nwebsec.com" },
                    { "nws-csp-nonce", "" }};

            var outputAttributes = new TagHelperAttributeList
                {
                    { "src", "https://script.nwebsec.com" },
                    { "nws-csp-nonce", "" }};

            var tagHelperContext = new TagHelperContext(
               contextAttributes,
                new Dictionary<object, object>(),
                "test");

            var tagHelperOutput = new TagHelperOutput("script", outputAttributes, (useCachedResult, encoder) =>
             {
                 var tagHelperContent = new DefaultTagHelperContent();
                 tagHelperContent.SetContent("Content");
                 return Task.FromResult<TagHelperContent>(tagHelperContent);
             });

            _tagHelper.Process(tagHelperContext, tagHelperOutput);

            //Assert gets script nonce, sets context, sets headers, switches attribute

            Assert.AreEqual("scriptnonce", tagHelperOutput.Attributes["nonce"].Value);
            TagHelperAttribute attr;
            Assert.IsFalse(tagHelperOutput.Attributes.TryGetAttribute("nws-csp-nonce", out attr));
            _cspOverrideHelper.Verify(co => co.GetCspScriptNonce(It.IsAny<HttpContext>()), Times.Once);
        }

        [Test]
        public void Process_StyleTag_AddsNonce()
        {
            _cspOverrideHelper.Setup(co => co.GetCspStyleNonce(It.IsAny<HttpContext>())).Returns("stylenonce");
            var contextAttributes = new TagHelperAttributeList
                {
                    { "src", "https://style.nwebsec.com" },
                    { "nws-csp-nonce", "" }};

            var outputAttributes = new TagHelperAttributeList
                {
                    { "src", "https://style.nwebsec.com" },
                    { "nws-csp-nonce", "" }};

            var tagHelperContext = new TagHelperContext(
               contextAttributes,
                new Dictionary<object, object>(),
                "test");
            var tagHelperOutput = new TagHelperOutput("style", outputAttributes, (useCachedResult, encoder) =>
            {
                var tagHelperContent = new DefaultTagHelperContent();
                tagHelperContent.SetContent("Content");
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

            _tagHelper.Process(tagHelperContext, tagHelperOutput);

            //Assert gets script nonce, sets context, sets headers, switches attribute

            Assert.AreEqual("stylenonce", tagHelperOutput.Attributes["nonce"].Value);
            TagHelperAttribute attr;
            Assert.IsFalse(tagHelperOutput.Attributes.TryGetAttribute("nws-csp-nonce", out attr));
            _cspOverrideHelper.Verify(co => co.GetCspStyleNonce(It.IsAny<HttpContext>()), Times.Once);
        }
    }
}