// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Web.Mvc;

namespace NWebsec.Mvc.HttpHeaders
{
    /// <summary>
    /// This attribute has been discontinued, and will be removed in the near future.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    [Obsolete("The SuppressVersionHttpHeaders attribute is no longer supported.", true)]
    public class SuppressVersionHttpHeadersAttribute : ActionFilterAttribute
    {
        public bool Enabled { get; set; }
        public string ServerHeader { get; set; }

        public SuppressVersionHttpHeadersAttribute()
        {
            Enabled = true;
            ServerHeader = String.Empty;
        }
    }
}
