// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

namespace NWebsec.Core.HttpHeaders.Configuration
{
    public class CspConfiguration : ICspConfiguration
    {
        public CspConfiguration()
        {
            DefaultSrcDirective = new CspDirectiveConfiguration();
            ScriptSrcDirective = new CspDirectiveUnsafeEvalConfiguration();
            ObjectSrcDirective = new CspDirectiveConfiguration();
            StyleSrcDirective = new CspDirectiveUnsafeInlineConfiguration();
            ImgSrcDirective = new CspDirectiveConfiguration();
            MediaSrcDirective = new CspDirectiveConfiguration();
            FrameSrcDirective = new CspDirectiveConfiguration();
            FontSrcDirective = new CspDirectiveConfiguration();
            ConnectSrcDirective = new CspDirectiveConfiguration();
            ReportUriDirective = new CspReportUriDirectiveConfiguration();
        }
        public bool Enabled { get; set; }
        public bool XContentSecurityPolicyHeader { get; set; }
        public bool XWebKitCspHeader { get; set; }
        public ICspDirectiveConfiguration DefaultSrcDirective { get; set; }
        public ICspDirectiveUnsafeEvalConfiguration ScriptSrcDirective { get; set; }
        public ICspDirectiveConfiguration ObjectSrcDirective { get; set; }
        public ICspDirectiveUnsafeInlineConfiguration StyleSrcDirective { get; set; }
        public ICspDirectiveConfiguration ImgSrcDirective { get; set; }
        public ICspDirectiveConfiguration MediaSrcDirective { get; set; }
        public ICspDirectiveConfiguration FrameSrcDirective { get; set; }
        public ICspDirectiveConfiguration FontSrcDirective { get; set; }
        public ICspDirectiveConfiguration ConnectSrcDirective { get; set; }
        public ICspReportUriDirective ReportUriDirective { get; set; }
    }
}