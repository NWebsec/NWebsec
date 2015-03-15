// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace NWebsec.Core.HttpHeaders.Configuration
{
    public class CspPluginTypesDirectiveConfiguration : ICspPluginTypesDirectiveConfiguration
    {
        public bool Enabled { get; set; }

        public IEnumerable<string> MediaTypes { get; set; }
    }
}