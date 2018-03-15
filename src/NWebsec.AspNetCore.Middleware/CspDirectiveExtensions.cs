// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Linq;
using NWebsec.Core.Common.HttpHeaders.Configuration;
using NWebsec.Core.Common.HttpHeaders.Csp;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Builder
{
    public static class CspDirectiveExtensions
    {
        /// <summary>
        ///     Sets the "none" source for the CSP directive. This source cannot be combined with other sources on a CSP directive.
        /// </summary>
        /// <typeparam name="T">The type of the CSP directive configuration object.</typeparam>
        /// <param name="directive">The CSP directive configuration object.</param>
        /// <exception cref="InvalidOperationException">Thrown when sources have already been configured for the directive.</exception>
        public static void None<T>(this T directive) where T : class, ICspDirectiveBasicConfiguration
        {
            if (directive == null) throw new ArgumentNullException(nameof(directive));

            ValidateBeforeSettingNoneSource(directive);
            directive.NoneSrc = true;
        }

        /// <summary>
        ///     Sets the "self" source for the CSP directive.
        /// </summary>
        /// <typeparam name="T">The type of the CSP directive configuration object.</typeparam>
        /// <param name="directive">The CSP directive configuration object.</param>
        /// <returns>The CSP directive configuration object.</returns>
        public static T Self<T>(this T directive) where T : class, ICspDirectiveBasicConfiguration
        {
            if (directive == null) throw new ArgumentNullException(nameof(directive));

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
        public static T CustomSources<T>(this T directive, params string[] sources) where T : class, ICspDirectiveBasicConfiguration
        {
            if (directive == null) throw new ArgumentNullException(nameof(directive));
            if (sources.Length == 0) throw new ArgumentException("You must supply at least one source.", nameof(sources));

            try
            {
                var type = typeof(T);
                var enableHashes = type == typeof(ICspDirectiveConfiguration) || type == typeof(ICspDirectiveUnsafeInlineConfiguration);
                directive.CustomSources = sources
                    .Select(s => (enableHashes ? CspHashSource.Parse(s) : null) ?? CspUriSource.Parse(s).ToString())
                    .ToArray();
            }
            catch (InvalidCspSourceException e)
            {
                throw new ArgumentException("Invalid source. Details: " + e.Message, nameof(sources), e);
            }

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
            if (directive == null) throw new ArgumentNullException(nameof(directive));

            directive.UnsafeInlineSrc = true;
            return directive;
        }

        /// <summary>
        ///     Sets the "unsafe-eval" source for the CSP directive.
        /// </summary>
        /// <typeparam name="T">The type of the CSP directive configuration object.</typeparam>
        /// <param name="directive">The CSP directive configuration object.</param>
        /// <returns>The CSP directive configuration object.</returns>
        public static T UnsafeEval<T>(this T directive) where T : class, ICspDirectiveConfiguration
        {
            if (directive == null) throw new ArgumentNullException(nameof(directive));

            directive.UnsafeEvalSrc = true;
            return directive;
        }

        /// <summary>
        ///     Sets the "strict-dynamic" source for the CSP directive.
        /// </summary>
        /// <typeparam name="T">The type of the CSP directive configuration object.</typeparam>
        /// <param name="directive">The CSP directive configuration object.</param>
        /// <returns>The CSP directive configuration object.</returns>
        public static T StrictDynamic<T>(this T directive) where T : class, ICspDirectiveConfiguration
        {
            if (directive == null) throw new ArgumentNullException(nameof(directive));

            directive.StrictDynamicSrc = true;
            return directive;
        }

        private static void ValidateBeforeSettingNoneSource(ICspDirectiveBasicConfiguration basicDirective)
        {
            if (basicDirective.SelfSrc || (basicDirective.CustomSources != null && basicDirective.CustomSources.Any()))
            {
                throw new InvalidOperationException("It is a logical error to combine the \"None\" source with other sources.");
            }

            if (basicDirective is ICspDirectiveUnsafeInlineConfiguration unsafeInline  && unsafeInline.UnsafeInlineSrc)
            {
                throw new InvalidOperationException("It is a logical error to combine the \"None\" source with other sources.");
            }

            if (basicDirective is ICspDirectiveConfiguration directive && (directive.UnsafeEvalSrc || directive.StrictDynamicSrc))
            {
                throw new InvalidOperationException("It is a logical error to combine the \"None\" source with other sources.");
            }
        }
    }
}