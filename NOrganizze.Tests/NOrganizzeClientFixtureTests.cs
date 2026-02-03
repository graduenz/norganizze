using System.Linq;
using Xunit;

namespace NOrganizze.Tests
{
    public class NOrganizzeClientFixtureTests
    {
        private readonly NOrganizzeClientFixture _fixture;

        public NOrganizzeClientFixtureTests(NOrganizzeClientFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void Test_ResolveApiKey()
        {
            // Act
            var credentials = _fixture.Client.CredentialsProvider();

            // Assert
            Assert.Matches(@"^((?!\.)[\w\-_.]*[^.])(@\w+)(\.\w+(\.\w+)?[^.\W])$", credentials.Email);
            Assert.Equal(40, credentials.ApiKey.Length);
            Assert.True(credentials.ApiKey.All(c => char.IsLetterOrDigit(c)));
        }
    }
}
