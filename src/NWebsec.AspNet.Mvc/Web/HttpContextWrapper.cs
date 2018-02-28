// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using NWebsec.Mvc.Common.Web;
using System.Collections;
using System.Web;

namespace NWebsec.Mvc.Web
{
    public class HttpContextWrapper : IHttpContextWrapper
    {
        private readonly HttpContextBase _context;

        public HttpContextWrapper(HttpContextBase context)
        {
            _context = context;
        }

        public IDictionary Items => _context.Items;
    }
}