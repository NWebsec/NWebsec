// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using NWebsec.Mvc.Helpers;

namespace NWebsec.Mvc.Tests.TestHelpers
{
    public static class CspCommonDirectives
    {
        public static IEnumerable<CspDirectives> Directives()
        {
            return Enum.GetValues(typeof(CspDirectives)).Cast<CspDirectives>().Except(new[] { CspDirectives.ReportUri });
        }
    }
}