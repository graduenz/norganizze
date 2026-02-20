using System;
using System.Net;

namespace NOrganizze
{
    public class NOrganizzeException : Exception
    {
        public HttpStatusCode StatusCode { get; }
        public string ResponseContent { get; }

        public NOrganizzeException(string message, HttpStatusCode statusCode, string responseContent)
            : base(message)
        {
            StatusCode = statusCode;
            ResponseContent = responseContent;
        }

        public NOrganizzeException(string message, HttpStatusCode statusCode, string responseContent, Exception innerException)
            : base(message, innerException)
        {
            StatusCode = statusCode;
            ResponseContent = responseContent;
        }

        public NOrganizzeException() : base()
        {
        }

        public NOrganizzeException(string message) : base(message)
        {
        }

        public NOrganizzeException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
