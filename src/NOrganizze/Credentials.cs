using System;

namespace NOrganizze
{
    /// <summary>Organizze API credentials (email and API key). Pass to <see cref="NOrganizzeClient"/> via constructor or <see cref="RequestOptions.CredentialsProvider"/>.</summary>
    public class Credentials
    {
        /// <summary>Default application name used in the User-Agent header.</summary>
        public const string DefaultName = "NOrganizze API Client";

        /// <summary>Creates credentials with the given email, API key, and optional application name.</summary>
        /// <param name="email">Organizze account email.</param>
        /// <param name="apiKey">Organizze API key.</param>
        /// <param name="name">Optional application name for User-Agent. Defaults to <see cref="DefaultName"/>.</param>
        public Credentials(string email, string apiKey, string name = DefaultName)
        {
            Email = !string.IsNullOrWhiteSpace(email) ? email : throw new ArgumentNullException(nameof(email));
            ApiKey = !string.IsNullOrWhiteSpace(apiKey) ? apiKey : throw new ArgumentNullException(nameof(apiKey));
            Name = !string.IsNullOrWhiteSpace(name) ? name : throw new ArgumentNullException(nameof(name));
        }

        /// <summary>Organizze account email.</summary>
        public string Email { get; }
        /// <summary>Organizze API key.</summary>
        public string ApiKey { get; }
        /// <summary>Application name used in the User-Agent header.</summary>
        public string Name { get; }

        /// <summary>Returns the Base64-encoded value for HTTP Basic authentication.</summary>
        public string ToBasicAuthHeaderValue()
        {
            return Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{Email}:{ApiKey}"));
        }

        /// <summary>Returns the User-Agent header value (name and email).</summary>
        public string ToUserAgentHeaderValue()
        {
            return $"{Name} ({Email})";
        }
    }
}
