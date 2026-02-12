using System;

namespace NOrganizze
{
    public class Credentials
    {
        public const string DefaultName = "NOrganizze API Client";

        public Credentials(string email, string apiKey, string name = DefaultName)
        {
            Email = !string.IsNullOrWhiteSpace(email) ? email : throw new ArgumentNullException(nameof(email));
            ApiKey = !string.IsNullOrWhiteSpace(apiKey) ? apiKey : throw new ArgumentNullException(nameof(apiKey));
            Name = !string.IsNullOrWhiteSpace(name) ? name : throw new ArgumentNullException(nameof(name));
        }

        public string Email { get; }
        public string ApiKey { get; }
        public string Name { get; }

        public string ToBasicAuthHeaderValue()
        {
            return Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{Email}:{ApiKey}"));
        }

        public string ToUserAgentHeaderValue()
        {
            return $"{Name} ({Email})";
        }
    }
}
