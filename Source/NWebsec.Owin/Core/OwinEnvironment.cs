// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Collections.Generic;
using NWebsec.Core;

namespace NWebsec.Owin.Core
{
    internal class OwinEnvironment
    {
        private readonly IDictionary<string, object> _environment;

        internal OwinEnvironment(IDictionary<string, object> env)
        {
            _environment = env;
            ResponseHeaders = new ResponseHeaders((IDictionary<string, string[]>) _environment[OwinKeys.ResponseHeaders]);
        }

        internal ResponseHeaders ResponseHeaders { get; private set; }

        internal NWebsecContext NWebsecContext
        {
            get
            {
                if (!_environment.ContainsKey(NWebsecContext.ContextKey))
                {
                    _environment[NWebsecContext.ContextKey] = new NWebsecContext();
                }

                return _environment[NWebsecContext.ContextKey] as NWebsecContext;
            }
        }
    }
}