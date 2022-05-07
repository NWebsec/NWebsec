// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Linq;
using NWebsec.Core.Common.HttpHeaders.Configuration;
using NWebsec.Core.Common.HttpHeaders.PermissionsPolicy;

namespace NWebsec.AspNetCore.Middleware
{
    public static class PermissionsPolicyPermissionExtensions
    {
        /// <summary>
        ///     Sets the "all" source for the PermissionsPolicy permission.
        /// </summary>
        /// <typeparam name="T">The type of the PermissionsPolicy permission configuration object.</typeparam>
        /// <param name="permission">The PermissionsPolicy permission configuration object.</param>
        /// <exception cref="InvalidOperationException">Thrown when sources have already been configured for the permission.</exception>
        public static void All<T>(this T permission) where T : class, IPermissionsPolicyPermissionConfiguration
        {
            if (permission == null) throw new ArgumentNullException(nameof(permission));

            permission.AllSrc = true;
        }

        /// <summary>
        ///     Sets the "none" source for the PermissionsPolicy permission. This source cannot be combined with other sources on a PermissionsPolicy permission.
        /// </summary>
        /// <typeparam name="T">The type of the PermissionsPolicy permission configuration object.</typeparam>
        /// <param name="permission">The PermissionsPolicy permission configuration object.</param>
        /// <exception cref="InvalidOperationException">Thrown when sources have already been configured for the permission.</exception>
        public static void None<T>(this T permission) where T : class, IPermissionsPolicyPermissionConfiguration
        {
            if (permission == null) throw new ArgumentNullException(nameof(permission));

            ValidateBeforeSettingNoneSource(permission);
            permission.NoneSrc = true;
        }

        /// <summary>
        ///     Sets the "self" source for the PermissionsPolicy permission.
        /// </summary>
        /// <typeparam name="T">The type of the PermissionsPolicy permission configuration object.</typeparam>
        /// <param name="permission">The PermissionsPolicy permission configuration object.</param>
        /// <returns>The PermissionsPolicy permission configuration object.</returns>
        public static T Self<T>(this T permission) where T : class, IPermissionsPolicyPermissionConfiguration
        {
            if (permission == null) throw new ArgumentNullException(nameof(permission));

            permission.SelfSrc = true;
            return permission;
        }

        /// <summary>
        ///     Sets custom sources for the PermissionsPolicy permission.
        /// </summary>
        /// <typeparam name="T">The type of the PermissionsPolicy permission configuration object.</typeparam>
        /// <param name="permission">The PermissionsPolicy permission configuration object.</param>
        /// <param name="sources">One or more custom sources.</param>
        /// <returns>The PermissionsPolicy permission configuration object.</returns>
        public static T CustomSources<T>(this T permission, params string[] sources) where T : class, IPermissionsPolicyPermissionConfiguration
        {
            if (permission == null) throw new ArgumentNullException(nameof(permission));
            if (sources.Length == 0) throw new ArgumentException("You must supply at least one source.", nameof(sources));

            try
            {
                var type = typeof(T);
                permission.CustomSources = sources
                    .Select(s => PermissionsPolicyUriSource.Parse(s).ToString())
                    .ToArray();
            }
            catch (InvalidPermissionsPolicySourceException e)
            {
                throw new ArgumentException("Invalid source. Details: " + e.Message, nameof(sources), e);
            }

            return permission;
        }

        private static void ValidateBeforeSettingNoneSource(IPermissionsPolicyPermissionConfiguration permission)
        {
            if (permission.AllSrc || permission.SelfSrc || (permission.CustomSources != null && permission.CustomSources.Any()))
            {
                throw new InvalidOperationException("It is a logical error to combine the \"None\" source with other sources.");
            }
        }
    }
}
