// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using NWebsec.Core.HttpHeaders.Configuration;

namespace NWebsec.Owin
{
    public class RedirectValidationOptions : IRedirectValidationConfiguration, IFluentRedirectValidationOptions
    {
        public RedirectValidationOptions()
        {
            Enabled = true;
            AllowedUris = new string[0];
        }

        public bool Enabled { get; set; }
        public IEnumerable<string> AllowedUris { get; set; }
        public ISameHostHttpsRedirectConfiguration SameHostRedirectConfiguration { get; set; }

        public void AllowedDestinations(params string[] uris)
        {
            if (uris.Length == 0) throw new ArgumentException("You must supply at least one redirect URI.");

            var validatedUris = new List<string>();

            foreach (var uri in uris)
            {
                Uri result;
                if (!Uri.TryCreate(uri, UriKind.Absolute, out result))
                {
                    throw new ArgumentException("Redirect URIs must be well formed absolute URIs. Offending URI: " + uri);
                }
                validatedUris.Add(result.AbsoluteUri);
            }

            AllowedUris = validatedUris.ToArray();
        }
    }
}