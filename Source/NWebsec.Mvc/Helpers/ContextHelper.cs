// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Web;
using NWebsec.Core.HttpHeaders.Configuration;
using NWebsec.ExtensionMethods;

namespace NWebsec.Mvc.Helpers
{
    internal class ContextHelper : IContextHelper
    {
        public IXRobotsTagConfiguration GetXRobotsTagConfiguration(HttpContextBase context)
        {
            var owinContext = context.GetNWebsecOwinContext();
            if (owinContext != null && owinContext.XRobotsTag != null)
            {
                return owinContext.XRobotsTag;
            }
            return context.GetNWebsecContext().XRobotsTag;
        }

        public IXFrameOptionsConfiguration GetXFrameOptionsConfiguration(HttpContextBase context)
        {
            var owinContext = context.GetNWebsecOwinContext();
            if (owinContext != null && owinContext.XRobotsTag != null)
            {
                return owinContext.XFrameOptions;
            }
            return context.GetNWebsecContext().XFrameOptions;
        }

        public ISimpleBooleanConfiguration GetXContentTypeOptionsConfiguration(HttpContextBase context)
        {
            var owinContext = context.GetNWebsecOwinContext();
            if (owinContext != null && owinContext.XContentTypeOptions != null)
            {
                return owinContext.XContentTypeOptions;
            }
            return context.GetNWebsecContext().XContentTypeOptions;
        }
        
        public ISimpleBooleanConfiguration GetXDownloadOptionsConfiguration(HttpContextBase context)
        {
            var owinContext = context.GetNWebsecOwinContext();
            if (owinContext != null && owinContext.XDownloadOptions != null)
            {
                return owinContext.XDownloadOptions;
            }
            return context.GetNWebsecContext().XDownloadOptions;
        }

        public IXXssProtectionConfiguration GetXXssProtectionConfiguration(HttpContextBase context)
        {
            var owinContext = context.GetNWebsecOwinContext();
            if (owinContext != null && owinContext.XXssProtection != null)
            {
                return owinContext.XXssProtection;
            }
            return context.GetNWebsecContext().XXssProtection;
        }

        public ICspConfiguration GetCspConfiguration(HttpContextBase context)
        {
            var owinContext = context.GetNWebsecOwinContext();
            if (owinContext != null && owinContext.Csp != null)
            {
                return owinContext.Csp;
            }
            return context.GetNWebsecContext().Csp;
        }

        public ICspConfiguration GetCspReportonlyConfiguration(HttpContextBase context)
        {
            var owinContext = context.GetNWebsecOwinContext();
            if (owinContext != null && owinContext.Csp != null)
            {
                return owinContext.Csp;
            }
            return context.GetNWebsecContext().Csp;
        }
    }
}