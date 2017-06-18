// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

namespace NWebsec.Core.Common.HttpHeaders.Configuration
{
    public class SameHostHttpsRedirectConfiguration : ISameHostHttpsRedirectConfiguration
    {
        public SameHostHttpsRedirectConfiguration()
        {
            Ports = new int[0];
        }

        public bool Enabled { get; set; }
        public int[] Ports { get; set; }
    }
}