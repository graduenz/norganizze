using System;

namespace NOrganizze
{
    /// <summary>
    /// Optional per-request overrides. Pass to service methods (e.g. List, Get, Create) when you need to override the base URL, credentials, or user agent for a single call.
    /// </summary>
    public class RequestOptions
    {
        /// <summary>Override the base API URL for this request only.</summary>
        public string BaseUrl { get; set; }
        /// <summary>Override the credentials (email + API key) used for this request only.</summary>
        public Func<Credentials> CredentialsProvider { get; set; }
        /// <summary>Override the User-Agent header for this request only.</summary>
        public string UserAgent { get; set; }
    }
}
