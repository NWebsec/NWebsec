// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Text;
using System.Web;
using NWebsec.Core.HttpHeaders;
using NWebsec.Core.HttpHeaders.Configuration;
using NWebsec.Csp;
using NWebsec.ExtensionMethods;
using NWebsec.Helpers;
using NWebsec.Modules.Configuration;

namespace NWebsec.HttpHeaders
{
    class HttpHeaderSetter
    {
        private readonly CspReportHelper _reportHelper;
        private readonly IHandlerTypeHelper _handlerHelper;
        private readonly HeaderGenerator _headerGenerator;
        private readonly bool _overrideHeaders;

        internal HttpHeaderSetter(bool overrideHeaders)
        {
            _overrideHeaders = overrideHeaders;
            _reportHelper = new CspReportHelper();
            _handlerHelper = new HandlerTypeHelper();
            _headerGenerator = new HeaderGenerator();
        }

        internal HttpHeaderSetter(IHandlerTypeHelper handlerTypeHelper, CspReportHelper reportHelper, bool overrideHeaders=true)
        {
            _overrideHeaders = overrideHeaders;
            _reportHelper = reportHelper;
            _handlerHelper = handlerTypeHelper;
            _headerGenerator = new HeaderGenerator();
        }

        

        

        

       
    }
}
