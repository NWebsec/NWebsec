// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Web;

namespace NWebsec.Mvc.Html
{
    public class ReferrerPolicyGenerator : IReferrerPolicy
    {
        private readonly HtmlString _none;
        private readonly HtmlString _origin;
        private readonly HtmlString _noneWhenDowngrade;
        private readonly HtmlString _unsafeUrl;
        private readonly HtmlString _originWhenCrossorigin;

        public ReferrerPolicyGenerator()
        {
            const string formatString = @"<meta name=""referrer"" content=""{0}"" />";
            _none = new HtmlString(string.Format(formatString, "none"));
            _origin = new HtmlString(string.Format(formatString, "origin"));
            _noneWhenDowngrade = new HtmlString(string.Format(formatString, "none-when-downgrade"));
            _originWhenCrossorigin = new HtmlString(string.Format(formatString, "origin-when-crossorigin"));
            _unsafeUrl = new HtmlString(string.Format(formatString, "unsafe-url"));
        }

        public HtmlString None()
        {
            return _none;
        }

        public HtmlString NoneWhenDownGrade()
        {
            return _noneWhenDowngrade;
        }

        public HtmlString Origin()
        {
            return _origin;
        }

        public HtmlString OriginWhenCrossOrigin()
        {
            return _originWhenCrossorigin;
        }

        public HtmlString UnsafeUrl()
        {
            return _unsafeUrl;
        }
    }
}