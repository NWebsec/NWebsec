namespace NWebsec.AspNetCore.Core.HttpHeaders.Configuration
{
    public class ReferrerPolicyConfiguration : IReferrerPolicyConfiguration
    {
        public ReferrerPolicy Policy { get; set; }
    }
}