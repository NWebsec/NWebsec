// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Razor.TagHelpers;
using NWebsec.AspNetCore.Core.Extensions;
using NWebsec.AspNetCore.Mvc.TagHelpers.Extensions;

namespace NWebsec.AspNetCore.Mvc.TagHelpers
{
    //Leave tags other than <meta> alone for now, until browser support improves. Only Chrome supports those at the time of checkin.
    
    //[HtmlTargetElement(AnchorTag, Attributes = ReferrerPolicyAttributeName)]
    //[HtmlTargetElement(AreaTag, Attributes = ReferrerPolicyAttributeName)]
    //[HtmlTargetElement(ImgTag, Attributes = ReferrerPolicyAttributeName)]
    //[HtmlTargetElement(IframeTag, Attributes = ReferrerPolicyAttributeName)]
    //[HtmlTargetElement(LinkTag, Attributes = ReferrerPolicyAttributeName)]
    [HtmlTargetElement(MetaTag, Attributes = MetaTagAttributeSelector)]
    public class ReferrerPolicyTagHelper : TagHelper
    {
        //private const string AnchorTag = "a";
        //private const string AreaTag = "area";
        //private const string ImgTag = "img";
        //private const string IframeTag = "iframe";
        //private const string LinkTag = "link";
        private const string MetaTag = "meta";
        private const string ReferrerPolicyAttributeName = "nws-referrerpolicy";
        private const string MetaTagAttributeSelector = "[name=referrer]," + ReferrerPolicyAttributeName;

        /// <summary>
        /// Specifies the referrer policy.
        /// </summary>
        [HtmlAttributeName(ReferrerPolicyAttributeName)]
        public ReferrerPolicy Policy { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var corePolicy = Policy.MapToCoreType();
            var result = corePolicy.GetPolicyString();
            var attributeName = output.TagName == MetaTag ? "content" : "referrerpolicy";
            output.Attributes.Add(new TagHelperAttribute(attributeName, result));
        }
    }
}