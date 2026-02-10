using System;

namespace NOrganizze
{
    public class Credentials
    {
        public Credentials(string email, string apiKey)
        {
            Email = email ?? throw new ArgumentNullException(nameof(email));
            ApiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
        }

        public string Email { get; }
        public string ApiKey { get; }

        public string ToBasicAuthHeaderValue()
        {
            return Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{Email}:{ApiKey}"));
        }
    }
}
