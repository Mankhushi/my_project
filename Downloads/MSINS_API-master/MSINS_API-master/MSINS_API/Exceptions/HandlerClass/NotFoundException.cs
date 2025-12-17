using System.Net;

namespace MSINS_API.Exceptions.HandlerClass
{
    public class NotFoundException : BaseException
    {
        public NotFoundException(string message)
        : base(message, HttpStatusCode.NotFound)
        {
        }
    }
}
