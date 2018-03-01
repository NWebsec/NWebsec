// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Web;
using NWebsec.Core.Common;

namespace NWebsec.ExtensionMethods
{
    public static class HttpContextBaseExtensions
    {
        public static NWebsecContext GetNWebsecContext(this HttpContextBase context)
        {
            if (context.Items[NWebsecContext.ContextKey] == null)
            {
                context.Items[NWebsecContext.ContextKey] = new NWebsecContext();
            }

            return context.Items[NWebsecContext.ContextKey] as NWebsecContext;
        }

        public static NWebsecContext GetNWebsecOwinContext(this HttpContextBase context)
        {
            var environment = context.Items["owin.Environment"] as IDictionary<string, object>;
            
            if (environment == null || !environment.ContainsKey(NWebsecContext.ContextKey))
            {
                return null;
            }
         
            return environment[NWebsecContext.ContextKey] as NWebsecContext;
        }
    }
}
