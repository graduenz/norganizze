using System;

namespace NOrganizze
{
    public class RequestOptions
    {
        public string BaseUrl { get; set; }
        public Func<Credentials> CredentialsProvider { get; set; }
        public string UserAgent { get; set; }
    }
}
