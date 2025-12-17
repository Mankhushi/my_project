using System.Net;

namespace MSINS_API.Exceptions.HandlerClass
{
    public class BaseException : Exception
    {
        public HttpStatusCode StatusCode { get; }
        public string ErrorCode { get; }
        public IDictionary<string, object> AdditionalData { get; }

        public BaseException(
            string message,
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError,
            string errorCode = null,
            IDictionary<string, object> additionalData = null)
            : base(message)
        {
            StatusCode = statusCode;
            ErrorCode = errorCode ?? statusCode.ToString();
            AdditionalData = additionalData ?? new Dictionary<string, object>();
        }
    }
}