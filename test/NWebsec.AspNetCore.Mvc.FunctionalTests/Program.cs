using System;
using NUnit.Common;
using NUnitLite;
using System.Reflection;

namespace NWebsec.AspNetCore.Mvc.FunctionalTests
{
    public class Program
    {
        public static int Main(string[] args)
        {
            return new AutoRun(typeof(Program).GetTypeInfo().Assembly)
                            .Execute(args, new ExtendedTextWrapper(Console.Out), Console.In);
        }
    }
}
