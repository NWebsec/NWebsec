// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Web;
using NWebsec.Core.Common.Web;

namespace NWebsec.Helpers
{
    public interface IHandlerTypeHelper
    {
        bool IsUnmanagedHandler(IHttpContextWrapper context);
        bool IsStaticContentHandler(IHttpContextWrapper context);
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

            switch (context.Handler.GetType().FullName)
            {
                case "System.Web.Handlers.AssemblyResourceLoader":
                case "System.Web.Handlers.ScriptResourceHandler":
                case "System.Web.Optimization.BundleHandler":
                    context.Items.Add(StaticContentHandlerKey, String.Empty);
                    return;
            }
        }

        public bool IsUnmanagedHandler(IHttpContextWrapper context)
        {
            return context.GetItem<string>(UnManagedHandlerKey) != null;
        }

        public bool IsStaticContentHandler(IHttpContextWrapper context)
        {
            return context.GetItem<string>(StaticContentHandlerKey) != null;
        }
    }
}
