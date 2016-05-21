// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using NWebsec.Core.HttpHeaders.Configuration;

namespace NWebsec.Mvc.Tests.TestHelpers
{
    public class CspPluginTypesDirectiveConfigurationComparer : IComparer<ICspPluginTypesDirectiveConfiguration>
    {
        public int Compare(ICspPluginTypesDirectiveConfiguration x, ICspPluginTypesDirectiveConfiguration y)
        {
            var result = x.Enabled.CompareTo(y.Enabled);
            if (result != 0) return result;

            result = x.MediaTypes.Count() - y.MediaTypes.Count();
            if (result != 0) return result;

            var elements = y.MediaTypes.Count();
            for (var i = 0; i < elements; i++)
            {
                result = string.Compare(x.MediaTypes.ElementAt(i), y.MediaTypes.ElementAt(i), StringComparison.Ordinal);
                if (result != 0) return result;
            }

            return 0;
        }
    }
}