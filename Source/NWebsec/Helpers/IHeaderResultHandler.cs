// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Web;
using NWebsec.Core.HttpHeaders;

namespace NWebsec.Helpers
{
    public interface IHeaderResultHandler
    {
        void HandleHeaderResult(HttpResponseBase response, HeaderResult result);
    }
}