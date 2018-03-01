// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Http;
using NWebsec.Core.Common;
using NWebsec.Mvc.Common.Web;

namespace NWebsec.AspNetCore.Mvc.Web
{
    public class HttpContextWrapper : IHttpContextWrapper //TODO tests
    {
        private readonly HttpContext _context;

        public HttpContextWrapper(HttpContext context)
        {
            _context = context;
        }

        public NWebsecContext GetNWebsecContext()
        {
            if (!_context.Items.ContainsKey(NWebsecContext.ContextKey))
            {
                _context.Items[NWebsecContext.ContextKey] = new NWebsecContext();
            }

            return (NWebsecContext)_context.Items[NWebsecContext.ContextKey];
        }

        public void AddItem(string key, string value)
        {
            _context.Items[key] = value;
        }

        public string GetItem(string key)
        {
            return _context.Items.ContainsKey(key) ? (string)_context.Items[key] : null;
        }
    }
}