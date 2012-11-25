using System.Configuration;

namespace NWebsec.Modules.Configuration.Csp
{
    public class CspDirectiveUnsafeInlineUnsafeEvalConfigurationElement : CspDirectiveUnsafeInlineConfigurationElement
    {
        [ConfigurationProperty("allowUnsafeEval", IsRequired = false, DefaultValue = false)]
        public bool AllowUnsafeEval
        {
            get
            {
                return (bool)this["allowUnsafeEval"];
            }
            set
            {
                this["allowUnsafeEval"] = value;
            }
        }
    }
}
