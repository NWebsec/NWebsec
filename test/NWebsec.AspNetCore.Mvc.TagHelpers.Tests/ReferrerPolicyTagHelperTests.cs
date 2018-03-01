// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Xunit;

namespace NWebsec.AspNetCore.Mvc.TagHelpers.Tests
{
    public class ReferrerPolicyTagHelperTests
    {
        private const string ContentAttribute = "referrerpolicy";

        public static readonly IEnumerable<object[]> ReferrerActionsAndValues = new TheoryData<string, string>
        {
            { "meta", "content"},
            {"a", ContentAttribute},
            {"area", ContentAttribute},
            {"img", ContentAttribute},
            {"iframe", ContentAttribute},
            {"link", ContentAttribute},
        };
        
        [Theory]
        [MemberData(nameof(ReferrerActionsAndValues))]
        public void Process_EnabledOnHtmlTag_AddPolicyAsContentAttribute(string tag, string expectedAttributeName)
        {
            var contextAttributes = new TagHelperAttributeList();
            var outputAttributes = new TagHelperAttributeList();

            var tagHelperContext = new TagHelperContext(
               contextAttributes,
                new Dictionary<object, object>(),
                "test");

            var tagHelperOutput = new TagHelperOutput(tag, outputAttributes, (useCachedResult, encoder) =>
            {
                var tagHelperContent = new DefaultTagHelperContent().SetContent("Content");
                return Task.FromResult(tagHelperContent);
            });

            var taghelper = new ReferrerPolicyTagHelper { Policy = ReferrerPolicy.NoReferrer };

            taghelper.Process(tagHelperContext, tagHelperOutput);

            Assert.True(tagHelperOutput.Attributes.ContainsName(expectedAttributeName));
            Assert.Equal("no-referrer", tagHelperOutput.Attributes[expectedAttributeName].Value);
        }
    }
}