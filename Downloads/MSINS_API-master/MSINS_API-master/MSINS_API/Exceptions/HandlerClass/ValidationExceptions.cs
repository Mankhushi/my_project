using System.Net;

namespace MSINS_API.Exceptions.HandlerClass
{
    public class ValidationExceptions : BaseException
    {
        public List<string> Errors { get; }

        public ValidationExceptions(string message, List<string> errors) : base(message, HttpStatusCode.BadRequest)
        {
            Errors = errors;
        }
    }
}
