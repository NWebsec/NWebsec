// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;

namespace NWebsec.Tests.Functional
{
    public class TestHelper
    {
        internal Uri GetUri(string baseUri, string path, string query="")
        {
            var absoluteUri = new UriBuilder(baseUri);
            absoluteUri.Path += path;
            absoluteUri.Query = query;
            return absoluteUri.Uri;
        }

        internal Uri GetHttpsUri(string baseUri, string path, string query = "")
        {
            var absoluteUri = new UriBuilder(baseUri);
            absoluteUri.Scheme = "https";
            absoluteUri.Port = 4443;
            absoluteUri.Path += path;
            absoluteUri.Query = query;
            return absoluteUri.Uri;
        }
    }
}
