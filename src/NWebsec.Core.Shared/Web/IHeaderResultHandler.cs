// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using NWebsec.Core.Common.HttpHeaders;

namespace NWebsec.Core.Common.Web
{
    public interface IHeaderResultHandler
    {
        void HandleHeaderResult(IHttpContextWrapper httpContext, HeaderResult result);
    }
}