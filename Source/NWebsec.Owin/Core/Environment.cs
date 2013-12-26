// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace NWebsec.Owin.Core
{
    internal class Environment
    {
        private readonly IDictionary<string, object> _environment;

        internal Environment(IDictionary<string, object> env)
        {
            _environment = env;
            ResponseHeaders = new ResponseHeaders((IDictionary<string, string[]>)_environment[OwinKeys.ResponseHeaders]);
        }

        internal ResponseHeaders ResponseHeaders { get; private set; }
    }
}
