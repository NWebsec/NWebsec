// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using NWebsec.Fluent;

namespace NWebsec.Owin
{
    /// <summary>
    /// Fluent interface to configure options for redirect validation.
    /// </summary>
    public interface IFluentRedirectValidationOptions : IFluentInterface
    {
        /// <summary>
        /// Configures the allowed redirect destinations. These must be well formed absolute URIs.
        /// </summary>
        /// <param name="uris">Allowed redirect destinations.</param>
        void AllowedDestinations(params string[] uris);
    }
}