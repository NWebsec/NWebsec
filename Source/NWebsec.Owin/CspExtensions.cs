// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using NWebsec.Core.HttpHeaders.Configuration;
using NWebsec.HttpHeaders;
using NWebsec.Owin.Middleware;
using Owin;

namespace NWebsec.Owin
{
    public static class CspExtensions
    {
        public static IAppBuilder UseCsp(this IAppBuilder app, CspOptions options)
        {
            return app.Use(typeof(CspMiddleware));
        }
    }

    internal class Testing
    {
        void SomeMethod(IAppBuilder app)
        {
            
            //cspOptions.Directives.ConfigureDefaultSrc(d => d.Self().CustomSources(new[]{""}))
            //    .ScriptSrc
            //cspOptions.Directives.ConfigureDefaultSrc.None();
            //cspOptions.Directives.ConfigureDefaultSrc.Self().CustomSources(new[] {""});

            // Srcs(new[] { "http://www.vg.no" });
            app.UseCsp(new CspOptions()
                .WithDefaultSrc(d =>
                {
                    d.Self();
                    d.CustomSources(new[] {""});
                })
                .WithScriptSrc(d => d.Self().UnsafeInline()));

        }
    }
    public class CspOptions
    {
        public CspOptions()
        {
            DefaultSrcDirective = new CspDirective();
            ScriptSrc = new CspDirectiveUnsafeEval();
            ObjectSrc = new CspDirective();
            StyleSrc = new CspDirectiveUnsafeInline();
            ImgSrc = new CspDirective();
            MediaSrc = new CspDirective();
            FrameSrc = new CspDirective();
            FontSrc = new CspDirective();
            ConnectSrc = new CspDirective();
        }

        internal CspDirective DefaultSrcDirective { get; set; }
        public CspDirectiveUnsafeEval ScriptSrc { get; set; }
        public CspDirective ObjectSrc { get; set; }
        public CspDirectiveUnsafeInline StyleSrc { get; set; }
        public CspDirective ImgSrc { get; set; }
        public CspDirective MediaSrc { get; set; }
        public CspDirective FrameSrc { get; set; }
        public CspDirective FontSrc { get; set; }
        public CspDirective ConnectSrc { get; set; }

        public CspOptions WithDefaultSrc(Action<CspDirective> action)
        {
            action(DefaultSrcDirective);
            return this;
        }

        public CspOptions WithScriptSrc(Action<CspDirectiveUnsafeEval> action)
        {
            action(ScriptSrc);
            return this;
        }

        public CspOptions WithObjectSources(Action<CspDirective> action)
        {
            action(ObjectSrc);
            return this;
        }
        public CspOptions WithStyleSrc(Action<CspDirectiveUnsafeInline> action)
        {
            action(StyleSrc);
            return this;
        }
        public CspOptions WithImageSrc(Action<CspDirective> action)
        {
            action(ImgSrc);
            return this;
        }
        public CspOptions WithMediaSrc(Action<CspDirective> action)
        {
            action(MediaSrc);
            return this;
        }
        public CspOptions WithFrameSrc(Action<CspDirective> action)
        {
            action(FrameSrc);
            return this;
        }
        public CspOptions WithFontSrc(Action<CspDirective> action)
        {
            action(FontSrc);
            return this;
        }

        public CspOptions WithConnectSrc(Action<CspDirective> action)
        {
            action(ConnectSrc);
            return this;
        }

    }


    public class CspDirective
    {
        internal bool NoneSrc { get; set; }
        internal bool SelfSrc { get; set; }

        internal IEnumerable<string> Sources { get; set; }
    }

    public class CspDirectiveUnsafeInline : CspDirective
    {
        internal bool UnsafeInlineSrc { get; set; }

    }

    public class CspDirectiveUnsafeEval : CspDirectiveUnsafeInline
    {
        internal bool UnsafeEvalSrc { get; set; }

    }

    public static class CspDirectiveExtensions
    {
        /// <summary>
        /// Sets the "none" source for the CSP directive. This source cannot be combined with other sources on a CSP directive.
        /// </summary>
        /// <typeparam name="T">The type of the CSP directive configuration object.</typeparam>
        /// <param name="directive">The CSP directive configuration object.</param>
        public static void None<T>(this T directive) where T : CspDirective
        {
            directive.NoneSrc = true;
        }


        /// <summary>
        /// Sets the "self" source for the CSP directive.
        /// </summary>
        /// <typeparam name="T">The type of the CSP directive configuration object.</typeparam>
        /// <param name="directive">The CSP directive configuration object.</param>
        /// <returns>The CSP directive configuration object.</returns>
        public static T Self<T>(this T directive) where T :  CspDirective
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
        public static T CustomSources<T>(this T directive, IEnumerable<string> sources) where T : CspDirective
        {
            directive.Sources = sources;
            return directive;
        }

        /// <summary>
        /// Sets "unsafe-inline" source for the CSP directive. 
        /// </summary>
        /// <typeparam name="T">The type of the CSP directive configuration object.</typeparam>
        /// <param name="directive">The CSP directive configuration object.</param>
        /// <returns>The CSP directive configuration object.</returns>
        public static T UnsafeInline<T>(this T directive) where T : CspDirectiveUnsafeInline
        {
            directive.UnsafeInlineSrc = true;
            return directive;
        }

        /// <summary>
        /// Sets "unsafe-eval" source for the CSP directive. 
        /// </summary>
        /// <typeparam name="T">The type of the CSP directive configuration object.</typeparam>
        /// <param name="directive">The CSP directive configuration object.</param>
        /// <returns>The CSP directive configuration object.</returns>
        public static T UnsafeEval<T>(this T directive) where T : CspDirectiveUnsafeEval
        {
            directive.UnsafeEvalSrc = true;
            return directive;
        }
    }
}