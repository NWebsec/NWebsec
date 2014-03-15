// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Web;
using NWebsec.Core.HttpHeaders.Configuration;
using NWebsec.ExtensionMethods;

namespace NWebsec.Mvc.Helpers
{
    internal class ContextHelper
    {
        internal IXRobotsTagConfiguration GetXRobotsTagConfiguration(HttpContextBase context)
        {
            var owinContext = context.GetNWebsecOwinContext();
            if (owinContext != null && owinContext.XRobotsTag != null)
            {
                return owinContext.XRobotsTag;
            }
            return context.GetNWebsecContext().XRobotsTag;
        }

        internal IXFrameOptionsConfiguration GetXFrameOptionsConfiguration(HttpContextBase context)
        {
            var owinContext = context.GetNWebsecOwinContext();
            if (owinContext != null && owinContext.XRobotsTag != null)
            {
                return owinContext.XFrameOptions;
            }
            return context.GetNWebsecContext().XFrameOptions;
        }

        internal ISimpleBooleanConfiguration GetXContentTypeOptionsConfiguration(HttpContextBase context)
        {
            var owinContext = context.GetNWebsecOwinContext();
            if (owinContext != null && owinContext.XContentTypeOptions != null)
            {
                return owinContext.XContentTypeOptions;
            }
            return context.GetNWebsecContext().XContentTypeOptions;
        }
        
        internal ISimpleBooleanConfiguration GetXDownloadOptionsConfiguration(HttpContextBase context)
        {
            var owinContext = context.GetNWebsecOwinContext();
            if (owinContext != null && owinContext.XDownloadOptions != null)
            {
                return owinContext.XDownloadOptions;
            }
            return context.GetNWebsecContext().XDownloadOptions;
        }

        internal IXXssProtectionConfiguration GetXXssProtectionConfiguration(HttpContextBase context)
        {
            var owinContext = context.GetNWebsecOwinContext();
            if (owinContext != null && owinContext.XXssProtection != null)
            {
                return owinContext.XXssProtection;
            }
            return context.GetNWebsecContext().XXssProtection;
        }

        internal ICspConfiguration GetCspConfiguration(HttpContextBase context)
        {
            var owinContext = context.GetNWebsecOwinContext();
            if (owinContext != null && owinContext.Csp != null)
            {
                return owinContext.Csp;
            }
            return context.GetNWebsecContext().Csp;
        }

        internal ICspConfiguration GetCspReportonlyConfiguration(HttpContextBase context)
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