// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Moq;
using Xunit;
using NWebsec.Core.Common.Web;
using NWebsec.Mvc.Common.Helpers;

namespace NWebsec.AspNetCore.Mvc.TagHelpers.Tests
{
    public class CspNonceTagHelperTests
    {
        private readonly CspNonceTagHelper _tagHelper;
        private readonly Mock<ICspConfigurationOverrideHelper> _cspOverrideHelper;
        private readonly Mock<IHeaderOverrideHelper> _headerOverrideHelper;

        public CspNonceTagHelperTests()
        {
            _cspOverrideHelper = new Mock<ICspConfigurationOverrideHelper>(MockBehavior.Strict);
            _headerOverrideHelper = new Mock<IHeaderOverrideHelper>(MockBehavior.Strict);
            

            _tagHelper = new CspNonceTagHelper(_cspOverrideHelper.Object, _headerOverrideHelper.Object);
            var httpContext = new DefaultHttpContext { Items = new Dictionary<object, object>() };
            _tagHelper.ViewContext = new Microsoft.AspNetCore.Mvc.Rendering.ViewContext { HttpContext = httpContext };
        }

        [Fact]
        public void Process_EnabledOnScriptTag_AddsNonce()
        {
            _cspOverrideHelper.Setup(co => co.GetCspScriptNonce(It.IsAny<IHttpContextWrapper>())).Returns("scriptnonce");
            _headerOverrideHelper.Setup(ho => ho.SetCspHeaders(It.IsAny<IHttpContextWrapper>(), It.IsAny<bool>()));

            var contextAttributes = new TagHelperAttributeList
                {
                    { "src", "https://script.nwebsec.com" }};

            var outputAttributes = new TagHelperAttributeList
                {
                    { "src", "https://script.nwebsec.com" }};

            var tagHelperContext = new TagHelperContext(
               contextAttributes,
                new Dictionary<object, object>(),
                "test");

            var tagHelperOutput = new TagHelperOutput("script", outputAttributes, (useCachedResult, encoder) =>
            {
                var tagHelperContent = new DefaultTagHelperContent().SetContent("Content");
                return Task.FromResult(tagHelperContent);
            });

            _tagHelper.UseCspNonce = true;

            _tagHelper.Process(tagHelperContext, tagHelperOutput);

            //Assert gets script nonce, sets context, sets headers, sets attribute

            _cspOverrideHelper.Verify(co => co.GetCspScriptNonce(It.IsAny<IHttpContextWrapper>()), Times.Once);

            _headerOverrideHelper.Verify(ho => ho.SetCspHeaders(It.IsAny<IHttpContextWrapper>(), false), Times.Once);
            _headerOverrideHelper.Verify(ho => ho.SetCspHeaders(It.IsAny<IHttpContextWrapper>(), true), Times.Once);

            Assert.True(tagHelperOutput.Attributes.ContainsName("nonce"));
            Assert.Equal("scriptnonce", tagHelperOutput.Attributes["nonce"].Value);
        }

        [Fact]
        public void Process_DisabledOnScriptTag_DoesNothing()
        {
            //Don't setup mocks.

            var contextAttributes = new TagHelperAttributeList
                {
                    { "src", "https://script.nwebsec.com" }};

            var outputAttributes = new TagHelperAttributeList
                {
                    { "src", "https://script.nwebsec.com" }};

            var tagHelperContext = new TagHelperContext(
               contextAttributes,
                new Dictionary<object, object>(),
                "test");

            var tagHelperOutput = new TagHelperOutput("script", outputAttributes, (useCachedResult, encoder) =>
            {
                var tagHelperContent = new DefaultTagHelperContent().SetContent("Content");
                return Task.FromResult(tagHelperContent);
            });

            _tagHelper.UseCspNonce = false;

            _tagHelper.Process(tagHelperContext, tagHelperOutput);

            //Strict mocks will throw if touched.

            Assert.False(tagHelperOutput.Attributes.ContainsName("type"));
        }

        [Fact]
        public void Process_EnabledOnStyleTag_AddsNonce()
        {
            _cspOverrideHelper.Setup(co => co.GetCspStyleNonce(It.IsAny<IHttpContextWrapper>())).Returns("stylenonce");
            _headerOverrideHelper.Setup(ho => ho.SetCspHeaders(It.IsAny<IHttpContextWrapper>(), It.IsAny<bool>()));

            var contextAttributes = new TagHelperAttributeList
                {
                    { "src", "https://style.nwebsec.com" }};

            var outputAttributes = new TagHelperAttributeList
                {
                    { "src", "https://style.nwebsec.com" }};

            var tagHelperContext = new TagHelperContext(
               contextAttributes,
                new Dictionary<object, object>(),
                "test");

            var tagHelperOutput = new TagHelperOutput("style", outputAttributes, (useCachedResult, encoder) =>
            {
                var tagHelperContent = new DefaultTagHelperContent().SetContent("Content");
                return Task.FromResult(tagHelperContent);
            });

            _tagHelper.UseCspNonce = true;

            _tagHelper.Process(tagHelperContext, tagHelperOutput);

            //Assert gets style nonce, sets context, sets headers, sets attribute

            _cspOverrideHelper.Verify(co => co.GetCspStyleNonce(It.IsAny<IHttpContextWrapper>()), Times.Once);

            _headerOverrideHelper.Verify(ho => ho.SetCspHeaders(It.IsAny<IHttpContextWrapper>(), false), Times.Once);
            _headerOverrideHelper.Verify(ho => ho.SetCspHeaders(It.IsAny<IHttpContextWrapper>(), true), Times.Once);

            Assert.True(tagHelperOutput.Attributes.ContainsName("nonce"));
            Assert.Equal("stylenonce", tagHelperOutput.Attributes["nonce"].Value);
        }

        [Fact]
        public void Process_DisabledOnStyleTag_DoesNothing()
        {
            //Don't setup mocks.

            var contextAttributes = new TagHelperAttributeList
                {
                    { "src", "https://style.nwebsec.com" }};

            var outputAttributes = new TagHelperAttributeList
                {
                    { "src", "https://style.nwebsec.com" }};

            var tagHelperContext = new TagHelperContext(
               contextAttributes,
                new Dictionary<object, object>(),
                "test");

            var tagHelperOutput = new TagHelperOutput("style", outputAttributes, (useCachedResult, encoder) =>
            {
                var tagHelperContent = new DefaultTagHelperContent().SetContent("Content");
                return Task.FromResult(tagHelperContent);
            });

            _tagHelper.UseCspNonce = false;

            _tagHelper.Process(tagHelperContext, tagHelperOutput);

            //Strict mocks will throw if touched.

            Assert.False(tagHelperOutput.Attributes.ContainsName("nonce"));
        }
    }
}