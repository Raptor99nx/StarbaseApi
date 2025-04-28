using System.Net;

namespace StargateAPI.Services
{
    public class ApiException : Exception
    {
        public HttpStatusCode StatusCode;

        public ApiException()
        {

        }

        public ApiException(string message, HttpStatusCode httpStatusCode)
            : base(message)
        {
            StatusCode = httpStatusCode;
        }

        public ApiException(string message, Exception inner, HttpStatusCode httpStatusCode)
            : base(message, inner)
        {
            StatusCode = httpStatusCode;
        }
    }
}
