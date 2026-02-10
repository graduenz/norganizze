using Microsoft.Extensions.Configuration;
using NOrganizze.Tests;
using System;
using Xunit;

[assembly: AssemblyFixture(typeof(NOrganizzeClientFixture))]

namespace NOrganizze.Tests
{
    public class NOrganizzeClientFixture : IDisposable
    {
        private static Credentials ResolveCredentials()
        {
            var configuration = new ConfigurationBuilder()
                .AddUserSecrets<NOrganizzeClientFixture>()
                .Build();

            return new Credentials(
                configuration["Organizze:Email"],
                configuration["Organizze:ApiKey"]
            );
        }

        public NOrganizzeClientFixture()
        {
            Client = new NOrganizzeClient(ResolveCredentials);
        }

        public NOrganizzeClient Client { get; }

        public void Dispose()
        {
            Client?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
