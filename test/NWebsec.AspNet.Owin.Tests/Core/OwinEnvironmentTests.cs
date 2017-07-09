// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using NWebsec.Owin.Core;
using Xunit;

namespace NWebsec.AspNet.Owin.Tests.Core
{
    public class OwinEnvironmentTests
    {
        //Comments from the OWIN 1.0 spec.
        private const string RequestSchemeKey = "owin.RequestScheme";      //A string containing the URI scheme used for the request (e.g., "http", "https");
        private const string RequestPathBaseKey = "owin.RequestPathBase";  //A string containing the portion of the request path corresponding to the "root" of the application delegate
        private const string RequestPathKey = "owin.RequestPath";          //A string containing the request path. The path MUST be relative to the "root" of the application delegate.
        private const string RequestHeaderKey = "owin.RequestHeaders";  //An IDictionary<string, string[]> of request headers.
        private const string ResponseHeaderKey = "owin.ResponseHeaders";//An IDictionary<string, string[]> of response headers.
        private const string ResponseStatusCodeKey = "owin.ResponseStatusCode"; //An optional int containing the HTTP response status code as defined in RFC 2616 section 6.1.1. The default is 200.

        private readonly IDictionary<string, object> _env;
        private readonly OwinEnvironment _owinEnvironment;

        public OwinEnvironmentTests()
        {
            _env = new Dictionary<string, object>
            {
                [RequestHeaderKey] = new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase), //Per OWIN 1.0 spec.
                [ResponseHeaderKey] = new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase) //Per OWIN 1.0 spec.
            };

            _owinEnvironment = new OwinEnvironment(_env);
        }

        [Theory]
        [InlineData("http")]
        [InlineData("https")]
        public void RequestScheme_ReturnsRequestScheme(string scheme)
        {
            _env[RequestSchemeKey] = scheme;

            Assert.Equal(scheme, _owinEnvironment.RequestScheme);
        }

        [Theory]
        [InlineData(200)]
        [InlineData(302)]
        public void ResponseStatusCodeGet_ReturnsStatusCode(int statusCode)
        {
            _env[ResponseStatusCodeKey] = statusCode;
            Assert.Equal(statusCode, _owinEnvironment.ResponseStatusCode);
        }

        [Fact]
        public void ResponseStatusCodeSet_SetsStatusCode()
        {
            _env[ResponseStatusCodeKey] = 200;

            _owinEnvironment.ResponseStatusCode = 302;

            Assert.Equal(302, _env[ResponseStatusCodeKey]);
        }

        [Fact]
        public void RequestPathBase_ReturnsPathBase()
        {
            _env[RequestPathBaseKey] = "/reqPathBase";
            Assert.Equal("/reqPathBase", _owinEnvironment.RequestPathBase);
        }

        [Fact]
        public void RequestPath_ReturnsPath()
        {
            _env[RequestPathKey] = "/reqPath/";
            Assert.Equal("/reqPath/", _owinEnvironment.RequestPath);
        }
    }
}