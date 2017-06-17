// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

namespace NWebsec.AspNetCore.Core.HttpHeaders.Configuration
{
    public interface IReferrerPolicyConfiguration
    {
        ReferrerPolicy Policy { get; set; }
    }
}