// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace NWebsec.Core.Configuration
{
    public interface IRedirectValidationConfiguration
    {
        IEnumerable<string> RedirectUris { get; set; }
    }
}