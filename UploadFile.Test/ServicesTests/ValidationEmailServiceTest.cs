using BlazorApp1.Server.Services;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UploadFile.Test.ServicesTests
{
    public class ValidationEmailServiceTest
    {
        [Theory]
        [InlineData("test@example.com", true)]
        [InlineData("user123@domain.co.uk", true)]
        [InlineData("invalid-email", false)]
        [InlineData("missingat.com", false)]
        [InlineData("toomany@at@symbols.com", false)]
        public void IsValidateEmail_ValidEmailFormats_ReturnsExpectedResult(string email, bool expected)
        {
            // Arrange
            var validationService = new ValidationEmailService();

            // Act
            var result = validationService.IsValidateEmail(email);

            // Assert
            result.Should().Be(expected);
        }
    }
}
