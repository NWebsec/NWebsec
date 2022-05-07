﻿// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

namespace NWebsec.Core.Common.HttpHeaders.PermissionsPolicy
{
    internal class PermissionsPolicySourceParseResult
    {
        public string Scheme { get; set; }
        public string Host { get; set; }
        public string Port { get; set; }
        public string PathAndQuery { get; set; }
    }
}
