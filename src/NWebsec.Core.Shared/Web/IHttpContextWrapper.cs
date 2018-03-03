// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

namespace NWebsec.Core.Common.Web
{
    public interface IHttpContextWrapper
    {
        T GetOriginalHttpContext<T>() where T : class;

        NWebsecContext GetNWebsecContext();

        //Here to cater for old Katana stuff.
        NWebsecContext GetNWebsecOwinContext();

        NWebsecContext GetNWebsecOverrideContext();

        void SetItem<T>(string key, T value) where T : class;
        T GetItem<T>(string key) where T : class;

        void SetHttpHeader(string name, string value);

        void RemoveHttpHeader(string name);

        void SetNoCacheHeaders();
    }
}