// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Web;
using NWebsec.Core.Common.HttpHeaders;

namespace NWebsec.Helpers
{
    public class HeaderResultHandler : IHeaderResultHandler
    {
        public void HandleHeaderResult(HttpResponseBase response, HeaderResult result)
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
                case HeaderResult.ResponseAction.Remove:
                    response.Headers.Remove(result.Name);
                    return;

            }
        }  
    }
}