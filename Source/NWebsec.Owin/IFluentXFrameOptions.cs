// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using NWebsec.Fluent;

namespace NWebsec.Owin
{
    public interface IFluentXFrameOptions : IFluentInterface
    {
        void Deny();
        void SameOrigin();
    }
}