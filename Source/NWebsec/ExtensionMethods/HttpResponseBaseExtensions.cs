// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Web;

namespace NWebsec.ExtensionMethods
{
    internal static class HttpResponseBaseExtensions
    {
        internal static bool HasStatusCodeThatRedirects(this HttpResponseBase response)
        {
            return response.StatusCode >= 300 && response.StatusCode < 400 && response.StatusCode != 304;
        }
    }
}
