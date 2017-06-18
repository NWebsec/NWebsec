// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Web;

namespace NWebsec.Helpers
{
    public interface IHandlerTypeHelper
    {
        bool IsUnmanagedHandler(HttpContextBase context);
        bool IsStaticContentHandler(HttpContextBase context);
    }

    public class HandlerTypeHelper : IHandlerTypeHelper
    {
        private const string UnManagedHandlerKey = "NWebsecUnmanagedHandler";
        private const string StaticContentHandlerKey = "NWebsecStaticContentHandler";

        public void RequestHandlerMapped(HttpContextBase context)
        {
            if (context.Handler == null)
            {
                context.Items.Add(UnManagedHandlerKey, String.Empty);
                return;
            }

            var handlerName = context.Handler.GetType().FullName;
            if (handlerName.Equals("System.Web.Handlers.AssemblyResourceLoader") ||
                handlerName.Equals("System.Web.Handlers.ScriptResourceHandler") ||
                handlerName.Equals("System.Web.Optimization.BundleHandler"))
            {
                context.Items.Add(StaticContentHandlerKey, String.Empty);
            }
        }

        public bool IsUnmanagedHandler(HttpContextBase context)
        {
            return context.Items[UnManagedHandlerKey] != null;
        }

        public bool IsStaticContentHandler(HttpContextBase context)
        {
            return context.Items[StaticContentHandlerKey] != null;
        }
    }
}
