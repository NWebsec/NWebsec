// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Web;
using System.Web.Mvc;
using NWebsec.Mvc;

[assembly: PreApplicationStartMethod(typeof(MvcStart), "DisableMvcVersionHeader")]

namespace NWebsec.Mvc
{
    public class MvcStart
    {
        public static void DisableMvcVersionHeader()
        {
            MvcHandler.DisableMvcResponseHeader = true;
        }
    }
}
