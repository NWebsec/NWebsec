// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using NWebsec.Core.HttpHeaders.Configuration;

namespace NWebsec.Owin
{
    public static class CspDirectiveExtensions
    {
        /// <summary>
        /// Sets the "none" source for the CSP directive. This source cannot be combined with other sources on a CSP directive.
        /// </summary>
        /// <typeparam name="T">The type of the CSP directive configuration object.</typeparam>
        /// <param name="directive">The CSP directive configuration object.</param>
        public static void None<T>(this T directive) where T : ICspDirectiveConfiguration
        {
            directive.NoneSrc = true;
        }

        /// <summary>
        /// Sets the "self" source for the CSP directive.
        /// </summary>
        /// <typeparam name="T">The type of the CSP directive configuration object.</typeparam>
        /// <param name="directive">The CSP directive configuration object.</param>
        /// <returns>The CSP directive configuration object.</returns>
        public static T Self<T>(this T directive) where T :  ICspDirectiveConfiguration
        {
            directive.SelfSrc = true;
            return directive;
        }

        /// <summary>
        /// Sets custom sources for the CSP directive. 
        /// </summary>
        /// <typeparam name="T">The type of the CSP directive configuration object.</typeparam>
        /// <param name="directive">The CSP directive configuration object.</param>
        /// <param name="sources"></param>
        /// <returns>The CSP directive configuration object.</returns>
        public static T CustomSources<T>(this T directive, params string[] sources) where T : ICspDirectiveConfiguration
        {
            directive.CustomSources = sources;
            return directive;
        }

        /// <summary>
        /// Sets the "unsafe-inline" source for the CSP directive. 
        /// </summary>
        /// <typeparam name="T">The type of the CSP directive configuration object.</typeparam>
        /// <param name="directive">The CSP directive configuration object.</param>
        /// <returns>The CSP directive configuration object.</returns>
        public static T UnsafeInline<T>(this T directive) where T : ICspDirectiveUnsafeInlineConfiguration
        {
            directive.UnsafeInlineSrc = true;
            return directive;
        }

        /// <summary>
        /// Sets the "unsafe-eval" source for the CSP directive. 
        /// </summary>
        /// <typeparam name="T">The type of the CSP directive configuration object.</typeparam>
        /// <param name="directive">The CSP directive configuration object.</param>
        /// <returns>The CSP directive configuration object.</returns>
        public static T UnsafeEval<T>(this T directive) where T : ICspDirectiveUnsafeEvalConfiguration
        {
            directive.UnsafeEvalSrc = true;
            return directive;
        }
    }
}