// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Web.Mvc;
using NWebsec.Core.Web;
using NWebsec.Csp;
using NWebsec.Mvc.Common.Csp;
using NWebsec.Mvc.Common.Helpers;
using NWebsec.Mvc.Helpers;
using NWebsec.Mvc.HttpHeaders.Internals;

namespace NWebsec.Mvc.HttpHeaders.Csp.Internals
{
    /// <summary>
    /// This class is abstract and cannot be used directly.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public abstract class CspSandboxAttributeBase : HttpHeaderAttributeBase
    {
        private readonly CspSandboxOverride _directive;
        private readonly CspConfigurationOverrideHelper _configurationOverrideHelper;
        private readonly HeaderOverrideHelper _headerOverrideHelper;

        protected CspSandboxAttributeBase()
        {
            _directive = new CspSandboxOverride { Enabled = true };
            _configurationOverrideHelper = new CspConfigurationOverrideHelper();
            _headerOverrideHelper = new HeaderOverrideHelper(new CspReportHelper());
        }

        internal sealed override string ContextKeyIdentifier => ReportOnly ? "CspReportOnly" : "Csp";

        /// <summary>
        /// Sets whether the sandbox directive is enabled in the CSP header. The default is true.
        /// </summary>
        public bool Enabled { get => _directive.Enabled; set => _directive.Enabled = value; }

        /// <summary>
        /// Sets whether the allow-forms flag is included in the sandbox directive. The default is false.
        /// </summary>
        public bool AllowForms { get => throw new NotSupportedException(); set => _directive.AllowForms = value; }

        /// <summary>
        /// Sets whether the allow-modals flag is included in the sandbox directive. The default is false.
        /// </summary>
        public bool AllowModals { get => throw new NotSupportedException(); set => _directive.AllowModals = value; }

        /// <summary>
        /// Sets whether the allow-orientation-lock flag is included in the sandbox directive. The default is false.
        /// </summary>
        public bool AllowOrientationLock { get => throw new NotSupportedException(); set => _directive.AllowOrientationLock = value; }

        /// <summary>
        /// Sets whether the allow-pointer-lock flag is included in the sandbox directive. The default is false.
        /// </summary>
        public bool AllowPointerLock { get => throw new NotSupportedException(); set => _directive.AllowPointerLock = value; }

        /// <summary>
        /// Sets whether the allow-popups flag is included in the sandbox directive. The default is false.
        /// </summary>
        public bool AllowPopups { get => throw new NotSupportedException(); set => _directive.AllowPopups = value; }

        /// <summary>
        /// Sets whether the allow-popups-to-escape-sandbox flag is included in the sandbox directive. The default is false.
        /// </summary>
        public bool AllowPopupsToEscapeSandbox { get => throw new NotSupportedException(); set => _directive.AllowPopupsToEscapeSandbox = value; }

        /// <summary>
        /// Sets whether the allow-presentation flag is included in the sandbox directive. The default is false.
        /// </summary>
        public bool AllowPresentation { get => throw new NotSupportedException(); set => _directive.AllowPresentation = value; }

        /// <summary>
        /// Sets whether the allow-same-origin flag is included in the sandbox directive. The default is false.
        /// </summary>
        public bool AllowSameOrigin { get => throw new NotSupportedException(); set => _directive.AllowSameOrigin = value; }

        /// <summary>
        /// Sets whether the allow-scripts flag is included in the sandbox directive. The default is false.
        /// </summary>
        public bool AllowScripts { get => throw new NotSupportedException(); set => _directive.AllowScripts = value; }

        /// <summary>
        /// Sets whether the allow-top-navigation flag is included in the sandbox directive. The default is false.
        /// </summary>
        public bool AllowTopNavigation { get => throw new NotSupportedException(); set => _directive.AllowTopNavigation = value; }

        protected abstract bool ReportOnly { get; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            _configurationOverrideHelper.SetCspSandboxOverride(new HttpContextWrapper(filterContext.HttpContext), _directive, ReportOnly);
            base.OnActionExecuting(filterContext);
        }

        public sealed override void SetHttpHeadersOnActionExecuted(ActionExecutedContext filterContext)
        {
            _headerOverrideHelper.SetCspHeaders(new HttpContextWrapper(filterContext.HttpContext), ReportOnly);
        }
    }
}
