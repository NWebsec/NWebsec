// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;

namespace NWebsec.Tests.Functional
{
    internal class TestHelper
    {
        internal Uri GetUri(string baseUri, string path)
        {
            var absoluteUri = new UriBuilder(baseUri);
            absoluteUri.Path += path;
            return absoluteUri.Uri;
        }
    }
}
