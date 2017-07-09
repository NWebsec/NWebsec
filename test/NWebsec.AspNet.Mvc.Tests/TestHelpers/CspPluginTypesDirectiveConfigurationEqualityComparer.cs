// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using NWebsec.Core.Common.HttpHeaders.Configuration;

namespace NWebsec.AspNet.Mvc.Tests.TestHelpers
{
    public class CspPluginTypesDirectiveConfigurationEqualityComparer : IEqualityComparer<ICspPluginTypesDirectiveConfiguration>
    {
        public bool Equals(ICspPluginTypesDirectiveConfiguration x, ICspPluginTypesDirectiveConfiguration y)
        {
            return x.Enabled == y.Enabled && x.MediaTypes.SequenceEqual(y.MediaTypes);
        }

        public int GetHashCode(ICspPluginTypesDirectiveConfiguration obj)
        {
            throw new NotImplementedException();
        }
    }
}