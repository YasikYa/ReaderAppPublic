using System.Net;

namespace ReaderApp.Data.Exceptions
{
    public class BadRequestException : AppExceptionBase
    {
        public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;

        public BadRequestException(string message)
            : base(message) { }
    }
}
