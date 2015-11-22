using NUnitLite;

namespace NWebsec.Mvc.Tests
{
    public class Program
    {
        public int Main(string[] args)
        {
#if NET451
            return new AutoRun().Execute(args);
#else
            return new AutoRun().Execute(typeof(Program).GetTypeInfo().Assembly, Console.Out, Console.In, args);
#endif
        }
    }
}
