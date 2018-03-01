// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NWebsec.Mvc.Common.Helpers;

namespace NWebsec.AspNetCore.Mvc.Tests.TestHelpers
{
    public class CspCommonDirectives : IEnumerable<CspDirectives>
    {
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<CspDirectives> GetEnumerator()
        {
            return Enum.GetValues(typeof(CspDirectives)).Cast<CspDirectives>().Except(new[] {CspDirectives.ReportUri}).GetEnumerator();
        }
    }
}