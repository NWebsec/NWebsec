// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace NWebsec.Owin.Core
{
    internal class RequestHeaders
    {
        private readonly IDictionary<string, string[]> _headers;

        internal RequestHeaders(IDictionary<string, string[]> headers)
        {
            _headers = headers;
        }

        public string Host
        {
            get
            {
                try
                {
                    return _headers.ContainsKey("Host") ? _headers["Host"].Single() : null;
                }
                catch (Exception)
                {
                    throw new Exception("Multiple Host headers detected: " + String.Join(" ", _headers["Host"]));
                }
            }
        }
    }
}