// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Moq;
using Xunit;
using NWebsec.Core.Common.Web;
using NWebsec.Mvc.Common.Csp;
using NWebsec.Mvc.Common.Helpers;

namespace NWebsec.AspNetCore.Mvc.TagHelpers.Tests
{
    public class CspMediaTypeTagHelperTests
    {
        private readonly CspMediaTypeTagHelper _tagHelper;
        private readonly Mock<ICspConfigurationOverrideHelper> _cspOverrideHelper;
        private readonly Mock<IHeaderOverrideHelper> _headerOverrideHelper;

        public CspMediaTypeTagHelperTests()
        {
            _cspOverrideHelper = new Mock<ICspConfigurationOverrideHelper>(MockBehavior.Strict);
            _headerOverrideHelper = new Mock<IHeaderOverrideHelper>(MockBehavior.Strict);
            _headerOverrideHelper.Setup(ho => ho.SetCspHeaders(It.IsAny<IHttpContextWrapper>(), It.IsAny<bool>()));

            _tagHelper = new CspMediaTypeTagHelper(_cspOverrideHelper.Object, _headerOverrideHelper.Object);
            var httpContext = new DefaultHttpContext { Items = new Dictionary<object, object>() };
            _tagHelper.ViewContext = new Microsoft.AspNetCore.Mvc.Rendering.ViewContext { HttpContext = httpContext };
        }

        [Fact]
        public void Process_ScriptTag_AddsHeaderAndTypeAttribute()
        {
            _cspOverrideHelper.Setup(co => co.SetCspPluginTypesOverride(It.IsAny<IHttpContextWrapper>(), It.IsAny<CspPluginTypesOverride>(), It.IsAny<bool>()));

            var contextAttributes = new TagHelperAttributeList
                {
                    { "src", "https://plugin.nwebsec.com" }};

            var outputAttributes = new TagHelperAttributeList
                {
                    { "src", "https://script.nwebsec.com" }};

            var tagHelperContext = new TagHelperContext(
               contextAttributes,
                new Dictionary<object, object>(),
                "test");

            var tagHelperOutput = new TagHelperOutput("object", outputAttributes, (useCachedResult, encoder) =>
             {
                 var tagHelperContent = new DefaultTagHelperContent().SetContent("Content");
                 return Task.FromResult(tagHelperContent);
             });

            _tagHelper.NwsCspPluginType = "application/pdf";

            _tagHelper.Process(tagHelperContext, tagHelperOutput);

            //Assert gets script nonce, sets context, sets headers, switches attribute

            _cspOverrideHelper.Verify(co => co.SetCspPluginTypesOverride(It.IsAny<IHttpContextWrapper>(), It.IsAny<CspPluginTypesOverride>(), false), Times.Once);
            _cspOverrideHelper.Verify(co => co.SetCspPluginTypesOverride(It.IsAny<IHttpContextWrapper>(), It.IsAny<CspPluginTypesOverride>(), true), Times.Once);

            _headerOverrideHelper.Verify(ho => ho.SetCspHeaders(It.IsAny<IHttpContextWrapper>(), false), Times.Once);
            _headerOverrideHelper.Verify(ho => ho.SetCspHeaders(It.IsAny<IHttpContextWrapper>(), true), Times.Once);

            Assert.True(tagHelperOutput.Attributes.ContainsName("type"));
            Assert.Equal("application/pdf", tagHelperOutput.Attributes["type"].Value);
        }
    }
}