using Microsoft.Extensions.Configuration;
using NOrganizze.Tests;
using System;
using Xunit;

[assembly: AssemblyFixture(typeof(NOrganizzeClientFixture))]

namespace NOrganizze.Tests
{
    public class NOrganizzeClientFixture : IDisposable
    {
        private static string ResolveApiKey()
        {
            var configuration = new ConfigurationBuilder()
                .AddUserSecrets<NOrganizzeClientFixture>()
                .Build();

            return configuration["Organizze:ApiKey"];
        }

        public NOrganizzeClientFixture()
        {
            Client = new NOrganizzeClient(ResolveApiKey);
        }

        public NOrganizzeClient Client { get; }

        public void Dispose()
        {
            Client?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
