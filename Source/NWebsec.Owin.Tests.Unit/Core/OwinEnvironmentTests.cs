// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using NUnit.Framework;
using NWebsec.Owin.Core;

namespace NWebsec.Owin.Tests.Unit.Core
{
    [TestFixture]
    public class OwinEnvironmentTests
    {
        //Comments from the OWIN 1.0 spec.
        private const string RequestScheme = "owin.RequestScheme"; //A string containing the URI scheme used for the request (e.g., "http", "https");
        private const string RequestHeaderKey = "owin.RequestHeaders"; //An IDictionary<string, string[]> of request headers.
        private const string ResponseHeaderKey = "owin.ResponseHeaders"; //An IDictionary<string, string[]> of response headers.
        private const string ResponseHeaderStatusCode = "owin.ResponseStatusCode"; //An optional int containing the HTTP response status code as defined in RFC 2616 section 6.1.1. The default is 200.
        
        private IDictionary<string, object> _env;
        private OwinEnvironment _owinEnvironment;

        [SetUp]
        public void Setup()
        {
            _env = new Dictionary<string, object>();
            _env[RequestHeaderKey] = new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase); //Per OWIN 1.0 spec.
            _env[ResponseHeaderKey] = new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase); //Per OWIN 1.0 spec.
            _owinEnvironment = new OwinEnvironment(_env);
        }

        [Test]
        public void RequestScheme_ReturnsRequestScheme()
        {
            _env[RequestScheme] = "https";
         
            Assert.AreEqual("https", _owinEnvironment.RequestScheme);
        }

        [Test]
        public void ResponseStatusCode_ReturnsStatusCode()
        {
            _env[ResponseHeaderStatusCode] = 200;
            Assert.AreEqual(200, _owinEnvironment.ResponseStatusCode);

            _env[ResponseHeaderStatusCode] = 302;
            Assert.AreEqual(302, _owinEnvironment.ResponseStatusCode);
        }
    }
}