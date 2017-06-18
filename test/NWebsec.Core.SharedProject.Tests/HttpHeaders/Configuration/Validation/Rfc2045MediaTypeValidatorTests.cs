// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using NWebsec.AspNetCore.Core.HttpHeaders.Configuration.Validation;
using Xunit;

namespace NWebsec.Core.SharedProject.Tests.HttpHeaders.Configuration.Validation
{
    public class Rfc2045MediaTypeValidatorTests
    {
        private readonly Rfc2045MediaTypeValidator _validator;

        public Rfc2045MediaTypeValidatorTests()
        {
            _validator = new Rfc2045MediaTypeValidator();
        }

        [Fact]
        public void Validate_CommonValidMediaTypes_NoException()
        {
            var mediaTypes = new[]
            {
                "application/pdf",
                "APPLICATION/PDF",
                "application/IOTP",
                "application/resource-lists-diff+xml",
                "application/media_control+xml",
                "application/vnd.ms-excel",
                "audio/mp4",
                "image/png",
                "model/x3d-vrml",
                "text/css",
                "video/mp4",
                "video/vnd.sealedmedia.softseal-mov"
            };

            foreach (var mediaType in mediaTypes)
            {
                _validator.Validate(mediaType);
            }
        }

        [Fact]
        public void Validate_InvalidType_ThrowsException()
        {
            var mediaType = "nwebsec/pdf";

            Assert.Throws<Exception>(() => _validator.Validate(mediaType));
        }

        [Fact]
        public void Validate_WhiteSpace_ThrowsException()
        {
            var mediaTypes = new[]
            {
                " application/pdf",
                "application /pdf",
                "application/ pdf",
                "application/pdf "
            };

            foreach (var mediaType in mediaTypes)
            {
                Assert.Throws<Exception>(() => _validator.Validate(mediaType));
            }
        }

        [Fact]
        public void Validate_TSpecials_ThrowsException()
        {
            var tspecials = "()<>@,;:\"\\/[]?=";
            var mediaType = "application/pdf";

            foreach (var specialChar in tspecials)
            {
                var invalidType = mediaType + specialChar;
                Assert.Throws<Exception>(() => _validator.Validate(invalidType));
            }
        }
    }
}