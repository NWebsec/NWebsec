// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using NWebsec.Fluent;

namespace NWebsec.Owin
{
    public interface IFluentHstsOptions : IFluentInterface
    {
        IFluentHstsOptions MaxAge(int days = 0, int hours = 0, int minutes = 0, int seconds = 0);
        IFluentHstsOptions IncludeSubdomains();
    }
}