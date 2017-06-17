// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;

namespace NWebsec.AspNetCore.Core.Exceptions
{
#if NETSTANDARD1_3
#else
    [Serializable]
#endif
    public class RedirectValidationException : Exception
    {
        public RedirectValidationException(string message) : base(message)
        {
        }
    }
}