// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using NWebsec.Core.Common;

namespace NWebsec.Mvc.Common.Web
{
    public interface IHttpContextWrapper
    {
        NWebsecContext GetNWebsecContext();
        // TODO Add header overrides
        void AddItem(string key, string value);
        string GetItem(string key);

    }
}