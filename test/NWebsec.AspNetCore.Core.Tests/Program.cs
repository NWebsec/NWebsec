// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Reflection;
using NUnit.Common;
using NUnitLite;

namespace NWebsec.AspNetCore.Core.Tests
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