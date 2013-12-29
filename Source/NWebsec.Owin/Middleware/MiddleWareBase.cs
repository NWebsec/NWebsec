// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NWebsec.Owin.Core;

namespace NWebsec.Owin.Middleware
{
    using AppFunc = Func<IDictionary<string, object>, Task>;

    public class MiddlewareBase
    {
        private readonly AppFunc _next;
        
        public MiddlewareBase(AppFunc next)
        {
            _next = next;
        }

        public async Task Invoke(IDictionary<string, object> environment)
        {
            var env = new OwinEnvironment(environment);
            
            PreInvokeNext(env);
            
            if (_next != null)
            {
                await _next(environment);
            }

            PostInvokeNext(env);
        }

        internal virtual void PreInvokeNext(OwinEnvironment environment)
        {
        }

        internal virtual void PostInvokeNext(OwinEnvironment environment)
        {
        }
    }
}
