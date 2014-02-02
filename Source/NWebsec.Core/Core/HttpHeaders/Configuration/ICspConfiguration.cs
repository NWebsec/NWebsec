// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

namespace NWebsec.Core.HttpHeaders.Configuration
{
    public interface ICspConfiguration
    {
        bool Enabled { get; set; }
        bool XContentSecurityPolicyHeader { get; set; }
        bool XWebKitCspHeader { get; set; }
        ICspDirective DefaultSrcDirective { get; set; }
        ICspDirectiveUnsafeEval ScriptSrcDirective { get; set; }
        ICspDirective ObjectSrcDirective { get; set; }
        ICspDirectiveUnsafeInline StyleSrcDirective { get; set; }
        ICspDirective ImgSrcDirective { get; set; }
        ICspDirective MediaSrcDirective { get; set; }
        ICspDirective FrameSrcDirective { get; set; }
        ICspDirective FontSrcDirective { get; set; }
        ICspDirective ConnectSrcDirective { get; set; }
    }
}