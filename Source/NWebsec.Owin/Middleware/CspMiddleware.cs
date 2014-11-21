// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NWebsec.Core.HttpHeaders;
using NWebsec.Core.HttpHeaders.Configuration;
using NWebsec.Owin.Core;

namespace NWebsec.Owin.Middleware
{
    using AppFunc = Func<IDictionary<string, object>, Task>;

    public class CspMiddleware : MiddlewareBase
    {
        private readonly ICspConfiguration _config;
        private readonly HeaderResult _headerResult;
        private readonly bool _reportOnly;

        public CspMiddleware(AppFunc next, ICspConfiguration options, bool reportOnly)
            : base(next)
        {
            _config = options;
            _reportOnly = reportOnly;

            var headerGenerator = new HeaderGenerator();
            _headerResult = headerGenerator.CreateCspResult(_config, reportOnly);
        }

        internal override void PreInvokeNext(OwinEnvironment owinEnvironment)
        {
            if (_reportOnly)
            {
                owinEnvironment.NWebsecContext.CspReportOnly = _config;
            }
            else
            {
                owinEnvironment.NWebsecContext.Csp = _config;
            }

            if (_headerResult.Action == HeaderResult.ResponseAction.Set)
            {
                owinEnvironment.ResponseHeaders.SetHeader(_headerResult.Name, _headerResult.Value);
            }
        }
    }
}