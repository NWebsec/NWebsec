// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using NWebsec.Core.HttpHeaders.Configuration;
using NWebsec.HttpHeaders;

namespace NWebsec.Core.HttpHeaders
{
    public class HeaderGenerator
    {
        public HeaderResult GenerateXfoHeader(IXFrameOptionsConfiguration xfoConfig, IXFrameOptionsConfiguration oldXfoConfig = null)
        {

            if (oldXfoConfig != null && oldXfoConfig.Policy != XFrameOptionsPolicy.Disabled &&
                xfoConfig.Policy == XFrameOptionsPolicy.Disabled)
            {
                return new HeaderResult(HeaderResult.ResponseAction.Delete, HeaderConstants.XFrameOptionsHeader);
            }

            switch (xfoConfig.Policy)
            {
                case XFrameOptionsPolicy.Disabled:
                    return new HeaderResult(HeaderResult.ResponseAction.Noop, HeaderConstants.XFrameOptionsHeader);

                case XFrameOptionsPolicy.Deny:
                    return new HeaderResult(HeaderResult.ResponseAction.Set, HeaderConstants.XFrameOptionsHeader, "Deny");

                case XFrameOptionsPolicy.SameOrigin:
                    return new HeaderResult(HeaderResult.ResponseAction.Set, HeaderConstants.XFrameOptionsHeader, "SameOrigin");

                //case HttpHeadersConstants.XFrameOptions.AllowFrom:
                //    frameOptions = "ALLOW-FROM " + headerConfig.SecurityHttpHeaders.XFrameOptions.Origin.GetLeftPart(UriPartial.Authority);
                //    break;

                default:
                    throw new NotImplementedException("Apparently someone forgot to implement support for: " +
                                                      xfoConfig.Policy);
            }
        } 
    }
}