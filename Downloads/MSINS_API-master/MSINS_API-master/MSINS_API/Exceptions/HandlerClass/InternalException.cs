using System.Net;

namespace MSINS_API.Exceptions.HandlerClass
{
    public class InternalException : BaseException
    {
        public InternalException(string message)
        : base(message, HttpStatusCode.InternalServerError)
        {
            
        }
    }
}
