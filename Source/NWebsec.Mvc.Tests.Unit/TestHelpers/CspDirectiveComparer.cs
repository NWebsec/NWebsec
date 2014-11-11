// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using NWebsec.Core.HttpHeaders.Configuration;

namespace NWebsec.Mvc.Tests.Unit.TestHelpers
{
    public class CspDirectiveComparer : IComparer<ICspDirectiveConfiguration>
    {
        public int Compare(ICspDirectiveConfiguration x, ICspDirectiveConfiguration y)
        {
            int result;
            var booleanResults =
                ((result = x.Enabled.CompareTo(y.Enabled)) != 0
                    ? result
                    : ((result = x.NoneSrc.CompareTo(y.NoneSrc)) != 0
                        ? result
                        : ((result = x.SelfSrc.CompareTo(y.SelfSrc)) != 0
                            ? result
                            : ((result = x.UnsafeInlineSrc.CompareTo(y.UnsafeInlineSrc)) != 0
                                ? result
                                : ((result = x.UnsafeEvalSrc.CompareTo(y.UnsafeEvalSrc)) != 0 ? result : 0)))));

            if (booleanResults != 0) return booleanResults;

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