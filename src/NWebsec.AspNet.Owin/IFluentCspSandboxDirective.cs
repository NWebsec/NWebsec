// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using NWebsec.Core.Common.Fluent;

namespace NWebsec.Owin
{
    public interface IFluentCspSandboxDirective : IFluentInterface
    {
        /// <summary>
        ///     Sets the 'allow-forms' source for the CSP sandbox directive.
        /// </summary>
        IFluentCspSandboxDirective AllowForms();

        /// <summary>
        ///     Sets the 'allow-modals' source for the CSP sandbox directive.
        /// </summary>
        IFluentCspSandboxDirective AllowModals();

        /// <summary>
        ///     Sets the 'allow-orientation-lock' source for the CSP sandbox directive.
        /// </summary>
        IFluentCspSandboxDirective AllowOrientationLock();

        /// <summary>
        ///     Sets the 'allow-pointer-lock' source for the CSP sandbox directive.
        /// </summary>
        IFluentCspSandboxDirective AllowPointerLock();

        /// <summary>
        ///     Sets the 'allow-popups' source for the CSP sandbox directive.
        /// </summary>
        IFluentCspSandboxDirective AllowPopups();

        /// <summary>
        ///     Sets the 'allow-popups-to-escape-sandbox' source for the CSP sandbox directive.
        /// </summary>
        IFluentCspSandboxDirective AllowPopupsToEscapeSandbox();
        /// <summary>
        ///     Sets the 'allow-presentation' source for the CSP sandbox directive.
        /// </summary>
        IFluentCspSandboxDirective AllowPresentation();

        /// <summary>
        ///     Sets the 'allow-same-origin' source for the CSP sandbox directive.
        /// </summary>
        IFluentCspSandboxDirective AllowSameOrigin();

        /// <summary>
        ///     Sets the 'allow-scripts' source for the CSP sandbox directive.
        /// </summary>
        IFluentCspSandboxDirective AllowScripts();

        /// <summary>
        ///     Sets the 'allow-top-navigation' source for the CSP sandbox directive.
        /// </summary>
        IFluentCspSandboxDirective AllowTopNavigation();
    }
}