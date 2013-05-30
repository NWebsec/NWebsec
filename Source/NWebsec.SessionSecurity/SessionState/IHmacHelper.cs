// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

namespace NWebsec.SessionSecurity.SessionState
{
    internal interface IHmacHelper
    {
        byte[] CalculateMac(byte[] input);
    }
}