// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NWebsec.Owin.MiddleWare
{
    public class RedirectValidation
    {
        private readonly Func<IDictionary<string, object>, Task> _next;

        public RedirectValidation(Func<IDictionary<string, object>, Task> next)
        {
            _next = next;
        }

        public async Task Invoke(IDictionary<string, object> environment)
        {
            if (_next != null)
            {
                await _next(environment);
            }

            Console.WriteLine("Doing work here!");
            throw new Exception("It works!");
        }
    }
}
