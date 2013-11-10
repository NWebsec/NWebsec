// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Web.Mvc;

namespace NWebsec.Mvc.HttpHeaders
{
    /// <summary>
    /// This attribute has been discontinued, and will be removed in the near future.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    [Obsolete("The StrictTransportSecurity attribute is no longer supported.", true)]
#pragma warning disable 1591
    public class StrictTransportSecurityAttribute : ActionFilterAttribute
    {
        public bool IncludeSubdomains { get; set; }

        public StrictTransportSecurityAttribute(string maxAge)
        {
            
        }
    }
}
