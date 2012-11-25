using System.Configuration;

namespace NWebsec.Modules.Configuration.Csp
{
    public class CspDirectiveUnsafeInlineConfigurationElement : CspDirectiveBaseConfigurationElement
    {
        [ConfigurationProperty("allowUnsafeInline", IsRequired = false, DefaultValue = false)]
        public bool AllowUnsafeInline
        {
            get
            {
                return (bool)this["allowUnsafeInline"];
            }
            set
            {
                this["allowUnsafeInline"] = value;
            }
        }
    }
}
