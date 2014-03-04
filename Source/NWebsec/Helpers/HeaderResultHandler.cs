// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Web;
using NWebsec.Core.HttpHeaders;

namespace NWebsec.Helpers
{
    public class HeaderResultHandler
    {
        internal void HandleHeaderResult(HttpResponseBase response, HeaderResult result)
        {
            if (result == null)
            {
                return;
            }

            switch (result.Action)
            {
                case HeaderResult.ResponseAction.Set:
                    response.Headers.Set(result.Name, result.Value);
                    return;
                case HeaderResult.ResponseAction.Delete:
                    response.Headers.Remove(result.Name);
                    return;

            }
        }  
    }
}