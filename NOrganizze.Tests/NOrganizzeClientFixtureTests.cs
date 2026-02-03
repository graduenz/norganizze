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
            var apiKey = _fixture.Client.ResolveApiKey();

            // Assert
            Assert.Equal(40, apiKey.Length);
            Assert.True(apiKey.All(c => char.IsLetterOrDigit(c)));
        }
    }
}
