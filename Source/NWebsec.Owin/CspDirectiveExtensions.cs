// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Linq;
using NWebsec.Core.HttpHeaders.Configuration;
using NWebsec.Core.HttpHeaders.Configuration.Validation;

namespace NWebsec.Owin
{
    public static class CspDirectiveExtensions
    {
        /// <summary>
        ///     Sets the "none" source for the CSP directive. This source cannot be combined with other sources on a CSP directive.
        /// </summary>
        /// <typeparam name="T">The type of the CSP directive configuration object.</typeparam>
        /// <param name="directive">The CSP directive configuration object.</param>
        /// <exception cref="InvalidOperationException">Thrown when sources have already been configured for the directive.</exception>
        public static void None<T>(this T directive) where T : class, ICspDirectiveConfiguration
        {
            if (directive == null) throw new ArgumentNullException("directive");

            ValidateBeforeSettingNoneSource(directive);
            directive.NoneSrc = true;
        }

        /// <summary>
        ///     Sets the "self" source for the CSP directive.
        /// </summary>
        /// <typeparam name="T">The type of the CSP directive configuration object.</typeparam>
        /// <param name="directive">The CSP directive configuration object.</param>
        /// <returns>The CSP directive configuration object.</returns>
        public static T Self<T>(this T directive) where T : class, ICspDirectiveConfiguration
        {
            if (directive == null) throw new ArgumentNullException("directive");

            directive.SelfSrc = true;
            return directive;
        }

        /// <summary>
        ///     Sets custom sources for the CSP directive.
        /// </summary>
        /// <typeparam name="T">The type of the CSP directive configuration object.</typeparam>
        /// <param name="directive">The CSP directive configuration object.</param>
        /// <param name="sources">One or more custom sources.</param>
        /// <returns>The CSP directive configuration object.</returns>
        public static T CustomSources<T>(this T directive, params string[] sources) where T : class, ICspDirectiveConfiguration
        {
            if (directive == null) throw new ArgumentNullException("directive");
            if (sources.Length == 0) throw new ArgumentException("You must supply at least one source.", "sources");

            var validator = new CspSourceValidator();

            foreach (var source in sources)
            {
                try
                {
                    validator.Validate(source);
                }
                catch (InvalidCspSourceException e)
                {
                    throw new ArgumentException("Invalid source. Details: " + e.Message, "sources", e);
                }
            }

            directive.CustomSources = sources;
            return directive;
        }

        /// <summary>
        ///     Sets the "unsafe-inline" source for the CSP directive.
        /// </summary>
        /// <typeparam name="T">The type of the CSP directive configuration object.</typeparam>
        /// <param name="directive">The CSP directive configuration object.</param>
        /// <returns>The CSP directive configuration object.</returns>
        public static T UnsafeInline<T>(this T directive) where T : class, ICspDirectiveUnsafeInlineConfiguration
        {
            if (directive == null) throw new ArgumentNullException("directive");

            directive.UnsafeInlineSrc = true;
            return directive;
        }

        /// <summary>
        ///     Sets the "unsafe-eval" source for the CSP directive.
        /// </summary>
        /// <typeparam name="T">The type of the CSP directive configuration object.</typeparam>
        /// <param name="directive">The CSP directive configuration object.</param>
        /// <returns>The CSP directive configuration object.</returns>
        public static T UnsafeEval<T>(this T directive) where T : class, ICspDirectiveUnsafeEvalConfiguration
        {
            if (directive == null) throw new ArgumentNullException("directive");

            directive.UnsafeEvalSrc = true;
            return directive;
        }

        private static void ValidateBeforeSettingNoneSource(ICspDirectiveConfiguration directive)
        {
            if (directive.SelfSrc || (directive.CustomSources != null && directive.CustomSources.Any()))
            {
                throw new InvalidOperationException("It is a logical error to combine the \"None\" source with other sources.");
            }

            var unsafeInline = directive as ICspDirectiveUnsafeInlineConfiguration;

            if (unsafeInline != null && unsafeInline.UnsafeInlineSrc)
            {
                throw new InvalidOperationException("It is a logical error to combine the \"None\" source with other sources.");
            }

            var unsafeEval = directive as ICspDirectiveUnsafeEvalConfiguration;

            if (unsafeEval != null && unsafeEval.UnsafeEvalSrc)
            {
                throw new InvalidOperationException("It is a logical error to combine the \"None\" source with other sources.");
            }
        }
    }
}