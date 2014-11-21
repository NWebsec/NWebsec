// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

namespace NWebsec.Core.HttpHeaders.Configuration
{
    public interface ICspConfiguration
    {
        bool Enabled { get; set; }
        ICspDirectiveConfiguration DefaultSrcDirective { get; set; }
        ICspDirectiveConfiguration ScriptSrcDirective { get; set; }
        ICspDirectiveConfiguration ObjectSrcDirective { get; set; }
        ICspDirectiveConfiguration StyleSrcDirective { get; set; }
        ICspDirectiveConfiguration ImgSrcDirective { get; set; }
        ICspDirectiveConfiguration MediaSrcDirective { get; set; }
        ICspDirectiveConfiguration FrameSrcDirective { get; set; }
        ICspDirectiveConfiguration FontSrcDirective { get; set; }
        ICspDirectiveConfiguration ConnectSrcDirective { get; set; }
        //CSP 2
        ICspDirectiveConfiguration FrameAncestorsDirective { get; set; }
        ICspReportUriDirectiveConfiguration ReportUriDirective { get; set; }
    }
}