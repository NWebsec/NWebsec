using NWebsec.AspNetCore.Core.HttpHeaders.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace NWebsec.AspNetCore.Middleware.Tests
{
    public class ExpectCtOptionsTest
    {
        private readonly ExpectCtOptions _options;

        public ExpectCtOptionsTest()
        {
            _options = new ExpectCtOptions();
        }

        [Fact]
        public void Ctor_Enforce_DefaultFalse()
        {
            Assert.False(((IExpectCtConfiguration)_options).Enforce);
        }

        [Fact]
        public void MaxAge_ValidMaxage_SetsMaxage()
        {
            _options.MaxAge(minutes: 30);

            Assert.True(new TimeSpan(0, 30, 0) == ((IExpectCtConfiguration)_options).MaxAge);
        }

        [Fact]
        public void MaxAge_ZeroMaxage_SetsMaxage()
        {
            _options.MaxAge();

            Assert.True(TimeSpan.Zero == ((IExpectCtConfiguration)_options).MaxAge);
        }

        [Fact]
        public void MaxAge_NegativeValues_ThrowsException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _options.MaxAge(0, 0, 0, -1));
            Assert.Throws<ArgumentOutOfRangeException>(() => _options.MaxAge(0, 0, -1));
            Assert.Throws<ArgumentOutOfRangeException>(() => _options.MaxAge(0, -1));
            Assert.Throws<ArgumentOutOfRangeException>(() => _options.MaxAge(-1));
        }
    }
}
