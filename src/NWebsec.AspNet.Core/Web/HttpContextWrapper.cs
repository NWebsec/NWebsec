// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Web;
using NWebsec.Core.Common;
using NWebsec.Core.Common.Web;

namespace NWebsec.Core.Web
{
    public class HttpContextWrapper : IHttpContextWrapper
    //TODO tests
    {
        private readonly HttpContextBase _context;

        public HttpContextWrapper(HttpContextBase context)
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

        public NWebsecContext GetNWebsecOwinContext()
        {
            var environment = _context.Items["owin.Environment"] as IDictionary<string, object>;

            if (environment == null || !environment.ContainsKey(NWebsecContext.ContextKey))
            {
                return null;
            }

            return environment[NWebsecContext.ContextKey] as NWebsecContext;
        }

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
            return _context.Items.Contains(key) ? (T)_context.Items[key] : null;
        }

        public void SetHttpHeader(string name, string value)
        {
            _context.Response.Headers.Set(name, value);
        }

        public void RemoveHttpHeader(string name)
        {
            _context.Response.Headers.Remove(name);
        }

        public void SetNoCacheHeaders()
        {
            _context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            _context.Response.Cache.SetNoStore();
            _context.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            _context.Response.Headers["Expires"] = "-1";
            _context.Response.Headers["Pragma"] = "no-cache";
        }

        private NWebsecContext GetNwsContext(string contextKey)
        {
            if (!_context.Items.Contains(contextKey))
            {
                _context.Items[contextKey] = new NWebsecContext();
            }

            return (NWebsecContext)_context.Items[contextKey];
        }
    }
}