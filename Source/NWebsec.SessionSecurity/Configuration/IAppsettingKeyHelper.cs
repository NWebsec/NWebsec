// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

namespace NWebsec.SessionSecurity.Configuration
{
    public interface IAppsettingKeyHelper
    {
        byte[] GetKeyFromAppsetting(string name);
    }
}
