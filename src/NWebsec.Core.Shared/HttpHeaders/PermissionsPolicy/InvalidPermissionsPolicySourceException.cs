// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;

namespace NWebsec.Core.Common.HttpHeaders.PermissionsPolicy
{
    public class InvalidPermissionsPolicySourceException : Exception
    {
        public InvalidPermissionsPolicySourceException(string s) : base(s)
        {

        }
    }
}
