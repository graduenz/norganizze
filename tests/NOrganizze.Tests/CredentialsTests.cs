using System;
using Xunit;

namespace NOrganizze.Tests
{
    public class CredentialsTests
    {
        private const string TestEmail = "test@example.com";
        private const string TestApiKey = "test-api-key-123";
        private const string TestName = "My Custom App";

        [Fact]
        public void Constructor_WithValidParameters_ShouldSetProperties()
        {
            // Act
            var credentials = new Credentials(TestEmail, TestApiKey, TestName);

            // Assert
            Assert.Equal(TestEmail, credentials.Email);
            Assert.Equal(TestApiKey, credentials.ApiKey);
            Assert.Equal(TestName, credentials.Name);
        }

        [Fact]
        public void Constructor_WithDefaultName_ShouldUseDefaultName()
        {
            // Act
            var credentials = new Credentials(TestEmail, TestApiKey);

            // Assert
            Assert.Equal(TestEmail, credentials.Email);
            Assert.Equal(TestApiKey, credentials.ApiKey);
            Assert.Equal(Credentials.DefaultName, credentials.Name);
            Assert.Equal("NOrganizze API Client", credentials.Name);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Constructor_WithNullOrEmptyEmail_ShouldThrowArgumentNullException(string email)
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => new Credentials(email, TestApiKey));
            Assert.Equal("email", exception.ParamName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Constructor_WithNullOrEmptyApiKey_ShouldThrowArgumentNullException(string apiKey)
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => new Credentials(TestEmail, apiKey));
            Assert.Equal("apiKey", exception.ParamName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Constructor_WithNullOrEmptyName_ShouldThrowArgumentNullException(string name)
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => new Credentials(TestEmail, TestApiKey, name));
            Assert.Equal("name", exception.ParamName);
        }

        [Fact]
        public void ToBasicAuthHeaderValue_ShouldReturnCorrectBase64EncodedValue()
        {
            // Arrange
            const string email = "test@example.com";
            const string apiKey = "my-secret-key";
            var credentials = new Credentials(email, apiKey);

            // Expected: Base64 encoded "test@example.com:my-secret-key"
            var expectedBytes = System.Text.Encoding.ASCII.GetBytes($"{email}:{apiKey}");
            var expectedValue = Convert.ToBase64String(expectedBytes);

            // Act
            var result = credentials.ToBasicAuthHeaderValue();

            // Assert
            Assert.Equal(expectedValue, result);
            Assert.Equal("dGVzdEBleGFtcGxlLmNvbTpteS1zZWNyZXQta2V5", result);
        }

        [Fact]
        public void ToUserAgentHeaderValue_ShouldReturnCorrectFormat()
        {
            // Arrange
            var credentials = new Credentials(TestEmail, TestApiKey, TestName);
            var expectedValue = $"{TestName} ({TestEmail})";

            // Act
            var result = credentials.ToUserAgentHeaderValue();

            // Assert
            Assert.Equal(expectedValue, result);
            Assert.Equal("My Custom App (test@example.com)", result);
        }

        [Fact]
        public void ToUserAgentHeaderValue_WithDefaultName_ShouldReturnCorrectFormat()
        {
            // Arrange
            var credentials = new Credentials(TestEmail, TestApiKey);
            var expectedValue = $"{Credentials.DefaultName} ({TestEmail})";

            // Act
            var result = credentials.ToUserAgentHeaderValue();

            // Assert
            Assert.Equal(expectedValue, result);
            Assert.Equal("NOrganizze API Client (test@example.com)", result);
        }

        [Fact]
        public void DefaultName_ShouldBeCorrectValue()
        {
            // Assert
            Assert.Equal("NOrganizze API Client", Credentials.DefaultName);
        }

        [Fact]
        public void Properties_ShouldBeReadOnly()
        {
            // Arrange
            var credentials = new Credentials(TestEmail, TestApiKey, TestName);

            // Assert - Properties should only have getters (compile-time check)
            // This test verifies the design at runtime
            Assert.Equal(TestEmail, credentials.Email);
            Assert.Equal(TestApiKey, credentials.ApiKey);
            Assert.Equal(TestName, credentials.Name);

            // Properties are readonly, so values cannot be changed after construction
            var emailProperty = typeof(Credentials).GetProperty(nameof(Credentials.Email));
            var apiKeyProperty = typeof(Credentials).GetProperty(nameof(Credentials.ApiKey));
            var nameProperty = typeof(Credentials).GetProperty(nameof(Credentials.Name));

            Assert.Null(emailProperty.SetMethod);
            Assert.Null(apiKeyProperty.SetMethod);
            Assert.Null(nameProperty.SetMethod);
        }
    }
}
