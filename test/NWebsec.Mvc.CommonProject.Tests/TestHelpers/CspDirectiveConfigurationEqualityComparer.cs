// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using NWebsec.Core.Common.HttpHeaders.Configuration;

namespace NWebsec.Mvc.CommonProject.Tests.TestHelpers
{
    public class CspDirectiveConfigurationEqualityComparer : IEqualityComparer<ICspDirectiveConfiguration>
    {
        public bool Equals(ICspDirectiveConfiguration x, ICspDirectiveConfiguration y)

        {
            var result = x.Enabled == y.Enabled &&
                         x.NoneSrc == y.NoneSrc &&
                         x.SelfSrc == y.SelfSrc &&
                         x.UnsafeInlineSrc == y.UnsafeInlineSrc &&
                         x.UnsafeEvalSrc == y.UnsafeEvalSrc &&
                         x.StrictDynamicSrc == y.StrictDynamicSrc;

            if (result == false) return false;

            if (x.Nonce == null)
            {
                if (y.Nonce != null) return false;
            }

            result = String.Equals(x.Nonce, y.Nonce, StringComparison.Ordinal);

            if (result == false) return false;

            if (x.CustomSources == null) return y.CustomSources == null;

            result = x.CustomSources.Count() == (y.CustomSources.Count());

            if (result == false) return false;

            return x.CustomSources.SequenceEqual(y.CustomSources);
        }

        public int GetHashCode(ICspDirectiveConfiguration obj)
        {
            throw new NotImplementedException();
        }
    }
}