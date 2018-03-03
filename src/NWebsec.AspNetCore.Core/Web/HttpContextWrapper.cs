// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Http;
using NWebsec.Core.Common;
using NWebsec.Core.Common.Web;

namespace NWebsec.AspNetCore.Core.Web
{
    public class HttpContextWrapper : IHttpContextWrapper
    //TODO tests
    {
        private readonly HttpContext _context;

        public HttpContextWrapper(HttpContext context)
        {
            _context = context;
        }

        public T GetOriginalHttpContext<T>() where T : class
        {
            return _context as T;
        }

        public NWebsecContext GetNWebsecContext()
        {
            return GetNwsContext(NWebsecContext.ContextKey);
        }

        //Will never be here in ASP.NET Core.
        public NWebsecContext GetNWebsecOwinContext() => null;

        public NWebsecContext GetNWebsecOverrideContext()
        {
            return GetNwsContext(NWebsecContext.ContextKeyOverrides);
        }

        public void SetItem<T>(string key, T value) where T : class
        {
            _context.Items[key] = value;
        }

        public T GetItem<T>(string key) where T : class
        {
            return _context.Items.ContainsKey(key) ? (T)_context.Items[key] : null;
        }

        public void SetHttpHeader(string name, string value)
        {
            _context.Response.Headers[name] = value;
        }

        public void RemoveHttpHeader(string name)
        {
            _context.Response.Headers.Remove(name);
        }

        public void SetNoCacheHeaders()
        {
            _context.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            _context.Response.Headers["Expires"] = "-1";
            _context.Response.Headers["Pragma"] = "no-cache";
        }

        private NWebsecContext GetNwsContext(string contextKey)
        {
            if (!_context.Items.ContainsKey(contextKey))
            {
                _context.Items[contextKey] = new NWebsecContext();
            }

            return (NWebsecContext)_context.Items[contextKey];
        }
    }
}