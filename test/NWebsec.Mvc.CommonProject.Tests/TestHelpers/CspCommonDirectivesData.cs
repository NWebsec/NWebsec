// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace NWebsec.Mvc.CommonProject.Tests.TestHelpers
{
    public class CspCommonDirectivesData : IEnumerable<object[]>
    {
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<object[]> GetEnumerator()
        {
            return new CspCommonDirectives().Select(directive => new object[] { directive }).GetEnumerator();
        }
    }
}