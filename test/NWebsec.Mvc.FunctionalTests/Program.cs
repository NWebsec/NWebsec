// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using NUnitLite;

namespace NWebsec.Mvc.FunctionalTests
{
    public class Program
    {
        public static int Main(string[] args)
        {
#if DNX451
            return new AutoRun().Execute(args);
#else
            return new AutoRun().Execute(typeof(Program).GetTypeInfo().Assembly, Console.Out, Console.In, args);
#endif
        }
    }
}
