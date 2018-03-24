// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Configuration;
using NWebsec.Core.Common.HttpHeaders.Configuration;
using NWebsec.Modules.Configuration.Csp.Validation;

namespace NWebsec.Modules.Configuration.Csp
{
    public class CspConfigurationElement : CspHeaderConfigurationElement, ICspConfiguration
    {

        [ConfigurationProperty("default-src", IsRequired = false)]
        [CspDirectiveBaseConfigurationElementValidator]
        public CspDirectiveBaseConfigurationElement<CspSourceConfigurationElement> DefaultSrc
        {
            get => (CspDirectiveBaseConfigurationElement<CspSourceConfigurationElement>)this["default-src"];
            set => this["default-src"] = value;
        }

        [ConfigurationProperty("script-src", IsRequired = false)]
        [CspDirectiveUnsafeInlineUnsafeEvalConfigurationElementValidator]
        public CspDirectiveUnsafeInlineUnsafeEvalConfigurationElement ScriptSrc
        {
            get => (CspDirectiveUnsafeInlineUnsafeEvalConfigurationElement)this["script-src"];
            set => this["script-src"] = value;
        }

        [ConfigurationProperty("object-src", IsRequired = false)]
        [CspDirectiveBaseConfigurationElementValidator]
        public CspDirectiveBaseConfigurationElement<CspSourceConfigurationElement> ObjectSrc
        {
            get => (CspDirectiveBaseConfigurationElement<CspSourceConfigurationElement>)this["object-src"];
            set => this["object-src"] = value;
        }

        [ConfigurationProperty("style-src", IsRequired = false)]
        [CspDirectiveUnsafeInlineConfigurationElementValidator]
        public CspDirectiveUnsafeInlineConfigurationElement StyleSrc
        {
            get => (CspDirectiveUnsafeInlineConfigurationElement)this["style-src"];
            set => this["style-src"] = value;
        }

        [ConfigurationProperty("img-src", IsRequired = false)]
        [CspDirectiveBaseConfigurationElementValidator]
        public CspDirectiveBaseConfigurationElement<CspSourceConfigurationElement> ImgSrc
        {
            get => (CspDirectiveBaseConfigurationElement<CspSourceConfigurationElement>)this["img-src"];
            set => this["img-src"] = value;
        }

        [ConfigurationProperty("media-src", IsRequired = false)]
        [CspDirectiveBaseConfigurationElementValidator]
        public CspDirectiveBaseConfigurationElement<CspSourceConfigurationElement> MediaSrc
        {
            get => (CspDirectiveBaseConfigurationElement<CspSourceConfigurationElement>)this["media-src"];
            set => this["media-src"] = value;
        }

        [ConfigurationProperty("frame-src", IsRequired = false)]
        [CspDirectiveBaseConfigurationElementValidator]
        public CspDirectiveBaseConfigurationElement<CspSourceConfigurationElement> FrameSrc
        {
            get => (CspDirectiveBaseConfigurationElement<CspSourceConfigurationElement>)this["frame-src"];
            set => this["frame-src"] = value;
        }

        [ConfigurationProperty("font-src", IsRequired = false)]
        [CspDirectiveBaseConfigurationElementValidator]
        public CspDirectiveBaseConfigurationElement<CspSourceConfigurationElement> FontSrc
        {
            get => (CspDirectiveBaseConfigurationElement<CspSourceConfigurationElement>)this["font-src"];
            set => this["font-src"] = value;
        }

        [ConfigurationProperty("connect-src", IsRequired = false)]
        [CspDirectiveBaseConfigurationElementValidator]
        public CspDirectiveBaseConfigurationElement<CspSourceConfigurationElement> ConnectSrc
        {
            get => (CspDirectiveBaseConfigurationElement<CspSourceConfigurationElement>)this["connect-src"];
            set => this["connect-src"] = value;
        }

        [ConfigurationProperty("base-uri", IsRequired = false)]
        [CspDirectiveBaseConfigurationElementValidator]
        public CspDirectiveBaseConfigurationElement<CspSourceConfigurationElement> BaseUri
        {
            get => (CspDirectiveBaseConfigurationElement<CspSourceConfigurationElement>)this["base-uri"];
            set => this["base-uri"] = value;
        }

        [ConfigurationProperty("child-src", IsRequired = false)]
        [CspDirectiveBaseConfigurationElementValidator]
        public CspDirectiveBaseConfigurationElement<CspSourceConfigurationElement> ChildSrc
        {
            get => (CspDirectiveBaseConfigurationElement<CspSourceConfigurationElement>)this["child-src"];
            set => this["child-src"] = value;
        }

        [ConfigurationProperty("form-action", IsRequired = false)]
        [CspDirectiveBaseConfigurationElementValidator]
        public CspDirectiveBaseConfigurationElement<CspSourceConfigurationElement> FormAction
        {
            get => (CspDirectiveBaseConfigurationElement<CspSourceConfigurationElement>)this["form-action"];
            set => this["form-action"] = value;
        }

        [ConfigurationProperty("frame-ancestors", IsRequired = false)]
        [CspDirectiveBaseConfigurationElementValidator]
        public CspDirectiveBaseConfigurationElement<CspSourceConfigurationElement> FrameAncestors
        {
            get => (CspDirectiveBaseConfigurationElement<CspSourceConfigurationElement>)this["frame-ancestors"];
            set => this["frame-ancestors"] = value;
        }

        [ConfigurationProperty("manifest-src", IsRequired = false)]
        [CspDirectiveBaseConfigurationElementValidator]
        public CspDirectiveBaseConfigurationElement<CspSourceConfigurationElement> ManifestSrc
        {
            get => (CspDirectiveBaseConfigurationElement<CspSourceConfigurationElement>)this["manifest-src"];
            set => this["manifest-src"] = value;
        }

        [ConfigurationProperty("worker-src", IsRequired = false)]
        [CspDirectiveBaseConfigurationElementValidator]
        public CspDirectiveBaseConfigurationElement<CspSourceConfigurationElement> WorkerSrc
        {
            get => (CspDirectiveBaseConfigurationElement<CspSourceConfigurationElement>)this["worker-src"];
            set => this["worker-src"] = value;
        }

        [ConfigurationProperty("sandbox", IsRequired = false)]
        public CspSandboxDirectiveConfigurationElement Sandbox
        {
            get => (CspSandboxDirectiveConfigurationElement)this["sandbox"];
            set => this["sandbox"] = value;
        }

        [ConfigurationProperty("plugin-types", IsRequired = false)]
        public CspPluginTypesDirectiveConfigurationElement PluginTypes
        {
            get => (CspPluginTypesDirectiveConfigurationElement)this["plugin-types"];
            set => this["plugin-types"] = value;
        }

        [ConfigurationProperty("upgrade-insecure-requests", IsRequired = false)]
        public CspUpgradeDirectiveConfigurationElement UpgradeInsecureRequests
        {
            get => (CspUpgradeDirectiveConfigurationElement)this["upgrade-insecure-requests"];
            set => this["upgrade-insecure-requests"] = value;
        }

        [ConfigurationProperty("block-all-mixed-content", IsRequired = false)]
        public CspMixedContentConfigurationElement MixedContent
        {
            get => (CspMixedContentConfigurationElement)this["block-all-mixed-content"];
            set => this["block-all-mixed-content"] = value;
        }

        [ConfigurationProperty("report-uri", IsRequired = false)]
        public CspReportUriDirectiveConfigurationElement ReportUri
        {
            get => (CspReportUriDirectiveConfigurationElement)this["report-uri"];
            set => this["report-uri"] = value;
        }

        public ICspDirectiveConfiguration DefaultSrcDirective
        {
            get => DefaultSrc;
            set => throw new NotImplementedException();
        }
        public ICspDirectiveConfiguration ScriptSrcDirective
        {
            get => ScriptSrc;
            set => throw new NotImplementedException();
        }
        public ICspDirectiveConfiguration ObjectSrcDirective
        {
            get => ObjectSrc;
            set => throw new NotImplementedException();
        }
        public ICspDirectiveConfiguration StyleSrcDirective
        {
            get => StyleSrc;
            set => throw new NotImplementedException();
        }
        public ICspDirectiveConfiguration ImgSrcDirective
        {
            get => ImgSrc;
            set => throw new NotImplementedException();
        }
        public ICspDirectiveConfiguration MediaSrcDirective
        {
            get => MediaSrc;
            set => throw new NotImplementedException();
        }
        public ICspDirectiveConfiguration FrameSrcDirective
        {
            get => FrameSrc;
            set => throw new NotImplementedException();
        }
        public ICspDirectiveConfiguration FontSrcDirective
        {
            get => FontSrc;
            set => throw new NotImplementedException();
        }
        public ICspDirectiveConfiguration ConnectSrcDirective
        {
            get => ConnectSrc;
            set => throw new NotImplementedException();
        }

        public ICspDirectiveConfiguration BaseUriDirective
        {
            get => BaseUri;
            set => throw new NotImplementedException();
        }
        public ICspDirectiveConfiguration ChildSrcDirective
        {
            get => ChildSrc;
            set => throw new NotImplementedException();
        }
        public ICspDirectiveConfiguration FormActionDirective
        {
            get => FormAction;
            set => throw new NotImplementedException();
        }

        public ICspDirectiveConfiguration FrameAncestorsDirective
        {
            get => FrameAncestors;
            set => throw new NotImplementedException();
        }

        public ICspDirectiveConfiguration ManifestSrcDirective
        {
            get => ManifestSrc;
            set => throw new NotImplementedException();
        }

        public ICspDirectiveConfiguration WorkerSrcDirective
        {
            get => WorkerSrc;
            set => throw new NotImplementedException();
        }

        public ICspPluginTypesDirectiveConfiguration PluginTypesDirective
        {
            get => PluginTypes;
            set => throw new NotImplementedException();
        }

        public ICspSandboxDirectiveConfiguration SandboxDirective
        {
            get => Sandbox;
            set => throw new NotImplementedException();
        }

        public ICspUpgradeDirectiveConfiguration UpgradeInsecureRequestsDirective
        {
            get => UpgradeInsecureRequests;
            set => throw new NotImplementedException();
        }

        public ICspMixedContentDirectiveConfiguration MixedContentDirective
        {
            get => MixedContent;
            set => throw new NotImplementedException();
        }

        public ICspReportUriDirectiveConfiguration ReportUriDirective
        {
            get => ReportUri;
            set => throw new NotImplementedException();
        }
    }
}
