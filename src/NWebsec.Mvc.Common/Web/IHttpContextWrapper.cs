// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using NWebsec.Annotations;
using System.Collections;

namespace NWebsec.Mvc.Common.Web
{
    public interface IHttpContextWrapper
    {
        [NotNull]
        IDictionary Items { get; }
    }
}