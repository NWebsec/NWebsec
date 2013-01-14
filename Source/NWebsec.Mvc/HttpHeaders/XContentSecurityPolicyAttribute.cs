// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Web.Mvc;

namespace NWebsec.Mvc.HttpHeaders
{
    /// <summary>
    /// This attribute is no longer supported.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    [Obsolete("This attribute has been discontinued. Please use the new CSP attributes in the NWebsec.Mvc.HttpHeaders.Csp namespace", true)]
#pragma warning disable 1591
    public class XContentSecurityPolicyAttribute : ActionFilterAttribute
    {
        public XContentSecurityPolicyAttribute(String directive, string sources)
        {
        }
    }
}
