// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Web;
using NWebsec.Core.HttpHeaders;

namespace NWebsec.Mvc
{
    internal class HeaderResultHandler
    {

        internal void HandleResult(HttpContextBase context, HeaderResult result)
        {
            var response = context.Response;
            
            switch (result.Action)
            {
                case HeaderResult.ResponseAction.Delete:
                    response.Headers.Remove(result.Name);
                    return;

                case HeaderResult.ResponseAction.Set:
                    response.Headers.Set(result.Name, result.Value);
                    return;

                case HeaderResult.ResponseAction.Noop:
                    return;
                
                default:
                    throw new NotImplementedException("Never heard of " + result.Action);
            }
        }
    }
}
