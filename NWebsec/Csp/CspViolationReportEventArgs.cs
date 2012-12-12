// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;

namespace NWebsec.Csp
{
    public class CspViolationReportEventArgs : EventArgs
    {
        public CspViolationReport ViolationReport { get; internal set; }
    }
}
