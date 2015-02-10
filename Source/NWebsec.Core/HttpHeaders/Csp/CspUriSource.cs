// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace NWebsec.Core.HttpHeaders.Csp
{
    public class CspUriSource
    {
        private const string HostRegex = @"^(\*\.)?([\p{Ll}\p{Lu}0-9]+)(\.[\p{Ll}\p{Lu}0-9\-]+)*$";
        private static readonly string SchemeOnlyRegex = "^[a-zA-Z]*[a-zA-Z0-9" + Regex.Escape("+.-") + "]:$";
        private readonly string _source;

        private CspUriSource(string source)
        {
            _source = source;
        }

        // Returns the source as a string encoded according to the CSP spec.
        public override string ToString()
        {
            return _source;

        }

        public static CspUriSource Parse(string source)
        {
            if (String.IsNullOrEmpty(source)) throw new ArgumentException("Value was null or empty", "source");

            if (source.Equals("*")) return new CspUriSource(source);

            //Scheme only source
            if (Regex.IsMatch(source, SchemeOnlyRegex)) return new CspUriSource(source.ToLower());

            var parseResult = ParseSourceComponents(source);
            var sb = new StringBuilder();

            if (!String.IsNullOrEmpty(parseResult.Scheme))
            {
                if (!Regex.IsMatch(parseResult.Scheme, SchemeOnlyRegex))
                {
                    throw new InvalidCspSourceException("Invalid scheme in CSP source: " + source);
                }
                sb.Append(parseResult.Scheme.ToLower()).Append("//");
            }

            if (String.IsNullOrEmpty(parseResult.Host))
            {
                throw new InvalidCspSourceException("Could not parse host in CSP source: " + source);
            }

            if (!Regex.IsMatch(parseResult.Host, HostRegex))
            {
                throw new InvalidCspSourceException("Invalid host in CSP source: " + source);
                
            }

            sb.Append(EncodeHostname(parseResult.Host.ToLower()));

            if (!String.IsNullOrEmpty(parseResult.Port))
            {
                if (!ValidatePort(parseResult.Port))
                {
                    throw new InvalidCspSourceException("Invalid port in CSP source: " + source);                    
                }
                sb.Append(":").Append(parseResult.Port);
            }

            if (!String.IsNullOrEmpty(parseResult.PathAndQuery))
            {
                sb.Append(EncodePath(parseResult.PathAndQuery));
            }

            return new CspUriSource(sb.ToString());
        }

        private static CspSourceParseResult ParseSourceComponents(string uri)
        {
            const string regex = @"^((?<scheme>.*?:)\/\/)?" + // match anything up to ://
                                 @"(?<host>.*?[^:\/])" + //then match anything up to a : or /
                                 @"(:(?<port>(.*?[^\/])))?" + //then match port if exists up to a /
                                 @"(?<pathAndQuery>\/.*)?$"; //grab the rest

            var re = new Regex(regex, RegexOptions.ExplicitCapture);
            var result = re.Match(uri);

            if (!result.Success)
            {
                throw new InvalidCspSourceException("Malformed CSP source: " + uri);
            }

            return new CspSourceParseResult
            {
                Scheme = result.Groups["scheme"].Value,
                Host = result.Groups["host"].Value,
                Port = result.Groups["port"].Value,
                PathAndQuery = result.Groups["pathAndQuery"].Value
            };
        }

        public static string EncodeHostname(string hostname)
        {
            var idn = new IdnMapping();
            return idn.GetAscii(hostname);
        }

        public static string EncodePath(string pathAndQuery)
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

        public static string EncodeUri(Uri uri)
        {
            if (!uri.IsWellFormedOriginalString())
            {
                throw new ArgumentException("Uri was not well formed.", "uri");
            }

            if (!uri.IsAbsoluteUri)
            {
                return EncodePath(uri.ToString());
            }

            var ub = new UriBuilder(uri);
            ub.Host = EncodeHostname(ub.Host);
            ub.Path = EncodePath(ub.Path);
            ub.Query = EncodePath(ub.Query);
            return ub.Uri.AbsoluteUri;
        }
        
        private static bool ValidatePort(string port)
        {
            if (port.Equals("*")) return true;

            int portNumber;
            var isInt = Int32.TryParse(port, out portNumber);
            return isInt && portNumber > 0 && portNumber <= 65535;
        }
    }
}