// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace NWebsec.Core.Common.HttpHeaders.PermissionsPolicy
{
    public class PermissionsPolicyUriSource
    {
        private const string HostRegex = @"^(\*\.)?([\p{Ll}\p{Lu}0-9\-]+)(\.[\p{Ll}\p{Lu}0-9\-]+)*$";
        private static readonly string SchemeOnlyRegex = "^[a-zA-Z][a-zA-Z0-9+.-]*:$";
        private static readonly string[] KnownSchemes = { "http", "https", "ws", "wss" };
        private readonly string _source;

        private PermissionsPolicyUriSource(string source)
        {
            _source = source;
        }
        
        public override string ToString()
        {
            return _source;

        }

        public static string EncodeUri(Uri uri)
        {

            if (!uri.IsAbsoluteUri)
            {
                var uriString = uri.IsWellFormedOriginalString() ? uri.ToString() : Uri.EscapeUriString(uri.ToString());
                return EscapeReservedPermissionsPolicyChars(uriString);
            }

            var host = uri.Host;
            var encodedHost = EncodeHostname(host);

            var needsReplacement = !host.Equals(encodedHost);

            var authority = uri.GetComponents(UriComponents.SchemeAndServer, UriFormat.SafeUnescaped);

            if (needsReplacement)
            {
                authority = authority.Replace(host, encodedHost);
            }

            if (uri.PathAndQuery.Equals("/"))
            {
                return authority;
            }

            return authority + EscapeReservedPermissionsPolicyChars(uri.PathAndQuery);
        }

        public static PermissionsPolicyUriSource Parse(string source)
        {
            if (String.IsNullOrEmpty(source)) throw new ArgumentException("Value was null or empty", "source");

            if (source.Equals("*")) return new PermissionsPolicyUriSource(source);

            Uri uriResult; //TODO figure out what happened to known schemes.
            if (Uri.TryCreate(source, UriKind.Absolute, out uriResult) && KnownSchemes.Contains(uriResult.Scheme))
            {
                return new PermissionsPolicyUriSource(EncodeUri(uriResult));
            }

            //Scheme only source
            if (Regex.IsMatch(source, SchemeOnlyRegex)) return new PermissionsPolicyUriSource(source.ToLower());

            var parseResult = ParseSourceComponents(source);
            var sb = new StringBuilder();

            if (!String.IsNullOrEmpty(parseResult.Scheme))
            {
                if (!Regex.IsMatch(parseResult.Scheme, SchemeOnlyRegex))
                {
                    throw new InvalidPermissionsPolicySourceException("Invalid scheme in PermissionsPolicy source: " + source);
                }
                sb.Append(parseResult.Scheme.ToLower()).Append("//");
            }

            if (String.IsNullOrEmpty(parseResult.Host))
            {
                throw new InvalidPermissionsPolicySourceException("Could not parse host in PermissionsPolicy source: " + source);
            }

            if (!Regex.IsMatch(parseResult.Host, HostRegex))
            {
                throw new InvalidPermissionsPolicySourceException("Invalid host in PermissionsPolicy source: " + source);
            }

            sb.Append(EncodeHostname(parseResult.Host.ToLower()));

            if (!String.IsNullOrEmpty(parseResult.Port))
            {
                if (!ValidatePort(parseResult.Port))
                {
                    throw new InvalidPermissionsPolicySourceException("Invalid port in PermissionsPolicy source: " + source);
                }
                sb.Append(":").Append(parseResult.Port);
            }

            if (!String.IsNullOrEmpty(parseResult.PathAndQuery))
            {
                sb.Append(EscapeReservedPermissionsPolicyChars(Uri.EscapeUriString(parseResult.PathAndQuery)));
            }

            return new PermissionsPolicyUriSource(sb.ToString());
        }

        private static PermissionsPolicySourceParseResult ParseSourceComponents(string uri)
        {
            const string regex = @"^((?<scheme>.*?:)\/\/)?" + // match anything up to ://
                                 @"(?<host>.*?[^:\/])" + //then match anything up to a : or /
                                 @"(:(?<port>(.*?[^\/])))?" + //then match port if exists up to a /
                                 @"(?<pathAndQuery>\/.*)?$"; //grab the rest

            var re = new Regex(regex, RegexOptions.ExplicitCapture);
            var result = re.Match(uri);

            if (!result.Success)
            {
                throw new InvalidPermissionsPolicySourceException("Malformed PermissionsPolicy source: " + uri);
            }

            return new PermissionsPolicySourceParseResult
            {
                Scheme = result.Groups["scheme"].Value,
                Host = result.Groups["host"].Value,
                Port = result.Groups["port"].Value,
                PathAndQuery = result.Groups["pathAndQuery"].Value
            };
        }

        private static string EncodeHostname(string hostname)
        {
            var idn = new IdnMapping();

            return idn.GetAscii(hostname);
        }

        //TODO: Add parentheses and equals to escaped chars?
        private static string EscapeReservedPermissionsPolicyChars(string pathAndQuery)
        {
            char[] encodeChars = { ';', ',' };

            if (pathAndQuery.IndexOfAny(encodeChars) == -1)
            {
                return pathAndQuery;
            }

            var sb = new StringBuilder(pathAndQuery);
            sb.Replace(";", "%3B");
            sb.Replace(",", "%2C");

            return sb.ToString();
        }

        private static bool ValidatePort(string port)
        {
            if (port.Equals("*")) return true;

            var isInt = Int32.TryParse(port, out var portNumber);
            return isInt && portNumber > 0 && portNumber <= 65535;
        }
    }
}