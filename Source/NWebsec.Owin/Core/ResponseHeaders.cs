// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace NWebsec.Owin.Core
{
    internal class ResponseHeaders
    {
        private readonly IDictionary<string, string[]> _headers;

        internal ResponseHeaders(IDictionary<string, string[]> headers)
        {
            _headers = headers;
        }

        internal void SetHeader(string name, string value)
        {
            _headers[name] = new[] {value};
        }

        internal void RemoveHeader(string name)
        {
            _headers.Remove(name);
        }
    }
}