// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Security.Cryptography.X509Certificates;

namespace NWebsec.Core.Common.HttpHeaders.Configuration
{
    public interface IHpkpCertConfiguration
    {
        string ThumbPrint { get; set; }
        StoreLocation StoreLocation { get; set; }
        StoreName Storename { get; set; }
        string SpkiPinValue { get; set; }
    }
}