// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Runtime.Versioning;
using Microsoft.Extensions.PlatformAbstractions;

namespace NWebsec.Mvc.FunctionalTests.Plumbing
{
    public class TestEnvironment : IApplicationEnvironment
    {
        private readonly IApplicationEnvironment _environment;

        public TestEnvironment(IApplicationEnvironment env, string basePath)
        {
            _environment = env;
            ApplicationBasePath = basePath;
        }
        public object GetData(string name)
        {
            return _environment.GetData(name);
        }

        public void SetData(string name, object value)
        {
            _environment.SetData(name, value);
        }

        public string ApplicationName => _environment.ApplicationName;
        public string ApplicationVersion => _environment.ApplicationName;
        public string ApplicationBasePath { get; }
        public string Configuration => _environment.Configuration;
        public FrameworkName RuntimeFramework => _environment.RuntimeFramework;
    }
}