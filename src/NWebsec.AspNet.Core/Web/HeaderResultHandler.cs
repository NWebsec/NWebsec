// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using NWebsec.Core.Common.HttpHeaders;
using NWebsec.Core.Common.Web;

namespace NWebsec.Core.Web
{
    public class HeaderResultHandler : IHeaderResultHandler
    {
        public void HandleHeaderResult(IHttpContextWrapper httpContext, HeaderResult result)
        {
            if (result == null)
            {
                return;
            }

            switch (result.Action)
            {
                case HeaderResult.ResponseAction.Set:
                    httpContext.SetHttpHeader(result.Name, result.Value);
                    return;
                case HeaderResult.ResponseAction.Remove:
                    httpContext.RemoveHttpHeader(result.Name);
                    return;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}