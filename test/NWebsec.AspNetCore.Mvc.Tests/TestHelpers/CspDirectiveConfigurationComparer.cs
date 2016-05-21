// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using NWebsec.Core.HttpHeaders.Configuration;

namespace NWebsec.Mvc.Tests.TestHelpers
{
    public class CspDirectiveConfigurationComparer : IComparer<ICspDirectiveConfiguration>
    {
        public int Compare(ICspDirectiveConfiguration x, ICspDirectiveConfiguration y)
        {
            var result = x.Enabled.CompareTo(y.Enabled);
            if (result != 0) return result;

            result = x.NoneSrc.CompareTo(y.NoneSrc);
            if (result != 0) return result;

            result = x.SelfSrc.CompareTo(y.SelfSrc);
            if (result != 0) return result;

            result = x.UnsafeInlineSrc.CompareTo(y.UnsafeInlineSrc);
            if (result != 0) return result;

            result = x.UnsafeEvalSrc.CompareTo(y.UnsafeEvalSrc);
            if (result != 0) return result;

            if (x.Nonce == null)
            {
                if (y.Nonce != null) return -1;
            }

            result = String.Compare(x.Nonce, y.Nonce, StringComparison.Ordinal);

            if (result != 0) return result;

            if (x.CustomSources == null) return y.CustomSources == null ? 0 : -1;

            result = x.CustomSources.Count().CompareTo(y.CustomSources.Count());

            if (result != 0) return result;

            if (x.CustomSources.SequenceEqual(y.CustomSources))
            {
                return 0;
            }

            var xs = x.CustomSources.ToArray();
            var ys = y.CustomSources.ToArray();

            for (var i = 0; i < xs.Length; i++)
            {
                result = String.Compare(xs[i], ys[i], StringComparison.Ordinal);
                if (result != 0) return result;

            }

            return 0;
        }
    }
}