// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using NUnit.Framework;
using NWebsec.AspNetCore.Middleware.Core;

namespace NWebsec.AspNetCore.Middleware.Tests.Core
{
    //TODO get rid of these?
    [TestFixture]
    public class OwinEnvironmentTests
    {
        //Comments from the OWIN 1.0 spec.
        private const string RequestSchemeKey = "owin.RequestScheme";      //A string containing the URI scheme used for the request (e.g., "http", "https");
        private const string RequestPathBaseKey = "owin.RequestPathBase";  //A string containing the portion of the request path corresponding to the "root" of the application delegate
        private const string RequestPathKey = "owin.RequestPath";          //A string containing the request path. The path MUST be relative to the "root" of the application delegate.
        private const string RequestHeaderKey = "owin.RequestHeaders";  //An IDictionary<string, string[]> of request headers.
        private const string ResponseHeaderKey = "owin.ResponseHeaders";//An IDictionary<string, string[]> of response headers.
        private const string ResponseStatusCodeKey = "owin.ResponseStatusCode"; //An optional int containing the HTTP response status code as defined in RFC 2616 section 6.1.1. The default is 200.
        
        private IDictionary<string, object> _env;
        private OwinEnvironment _owinEnvironment;

        [SetUp]
        public void Setup()
        {
            _env = new Dictionary<string, object>
            {
                [RequestHeaderKey] = new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase), //Per OWIN 1.0 spec.
                [ResponseHeaderKey] = new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase) //Per OWIN 1.0 spec.
            };
            
            _owinEnvironment = new OwinEnvironment(_env);
        }

        [Test]
        public void RequestScheme_ReturnsRequestScheme([Values("http","https")] string scheme)
        {
            _env[RequestSchemeKey] = scheme;
         
            Assert.AreEqual(scheme, _owinEnvironment.RequestScheme);
        }

        [Test]
        public void ResponseStatusCodeGet_ReturnsStatusCode([Values(200,302)] int statusCode)
        {
            _env[ResponseStatusCodeKey] = statusCode;
            Assert.AreEqual(statusCode, _owinEnvironment.ResponseStatusCode);
        }

        [Test]
        public void ResponseStatusCodeSet_SetsStatusCode()
        {
            _env[ResponseStatusCodeKey] = 200;

            _owinEnvironment.ResponseStatusCode = 302;

            Assert.AreEqual(302, _env[ResponseStatusCodeKey]);
        }

        [Test]
        public void RequestPathBase_ReturnsPathBase()
        {
            _env[RequestPathBaseKey] = "/reqPathBase";
            Assert.AreEqual("/reqPathBase", _owinEnvironment.RequestPathBase);
        }

        [Test]
        public void RequestPath_ReturnsPath()
        {
            _env[RequestPathKey] = "/reqPath/";
            Assert.AreEqual("/reqPath/", _owinEnvironment.RequestPath);
        }
    }
}