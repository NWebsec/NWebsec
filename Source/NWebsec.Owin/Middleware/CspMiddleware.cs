// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NWebsec.Owin.Middleware
{
    using AppFunc = Func<IDictionary<string, object>, Task>;

    public class CspMiddleware : MiddlewareBase
    {
        public CspMiddleware(AppFunc next) : base(next)
        {
        }
    }
}