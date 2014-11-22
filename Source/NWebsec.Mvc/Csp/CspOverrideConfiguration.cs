// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using NWebsec.Core.HttpHeaders.Configuration;

namespace NWebsec.Mvc.Csp
{
    public class CspOverrideConfiguration: ICspConfiguration
    {
       public bool EnabledOverride { get; set; }

        //Interface members
        public bool Enabled { get; set; }
        public ICspDirectiveConfiguration DefaultSrcDirective { get; set; }
        public ICspDirectiveConfiguration ScriptSrcDirective { get; set; }
        public ICspDirectiveConfiguration ObjectSrcDirective { get; set; }
        public ICspDirectiveConfiguration StyleSrcDirective { get; set; }
        public ICspDirectiveConfiguration ImgSrcDirective { get; set; }
        public ICspDirectiveConfiguration MediaSrcDirective { get; set; }
        public ICspDirectiveConfiguration FrameSrcDirective { get; set; }
        public ICspDirectiveConfiguration FontSrcDirective { get; set; }
        public ICspDirectiveConfiguration ConnectSrcDirective { get; set; }
        public ICspDirectiveConfiguration BaseUriDirective { get; set; }
        public ICspDirectiveConfiguration ChildSrcDirective { get; set; }
        public ICspDirectiveConfiguration FormActionDirective { get; set; }
        public ICspDirectiveConfiguration FrameAncestorsDirective { get; set; }
        public ICspReportUriDirectiveConfiguration ReportUriDirective { get; set; }
    }
}