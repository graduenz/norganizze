using System;
using System.Net;

namespace NOrganizze
{
    /// <summary>Exception thrown when an Organizze API request fails. Contains the HTTP status code and response body when available.</summary>
    public class NOrganizzeException : Exception
    {
        /// <summary>HTTP status code of the failed response, when the exception was created from an API response.</summary>
        public HttpStatusCode StatusCode { get; }
        /// <summary>Response body of the failed request, when available.</summary>
        public string ResponseContent { get; }

        /// <summary>Creates an exception with message, status code, and response content.</summary>
        public NOrganizzeException(string message, HttpStatusCode statusCode, string responseContent)
            : base(message)
        {
            StatusCode = statusCode;
            ResponseContent = responseContent;
        }

        /// <summary>Creates an exception with message, status code, response content, and inner exception.</summary>
        public NOrganizzeException(string message, HttpStatusCode statusCode, string responseContent, Exception innerException)
            : base(message, innerException)
        {
            StatusCode = statusCode;
            ResponseContent = responseContent;
        }

        /// <summary>Creates an exception with default message.</summary>
        public NOrganizzeException() : base()
        {
        }

        /// <summary>Creates an exception with the given message.</summary>
        public NOrganizzeException(string message) : base(message)
        {
        }

        /// <summary>Creates an exception with the given message and inner exception.</summary>
        public NOrganizzeException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
