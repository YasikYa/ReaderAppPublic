using ReaderApp.Data.Exceptions;
using System.Net;

namespace ReaderApp.Commands.Exceptions
{
    public class ForbidException : AppExceptionBase
    {
        private static string ForbidMessage => "Access not allowed";

        public override HttpStatusCode StatusCode => HttpStatusCode.Forbidden;

        public ForbidException(string message = null)
            : base(message ?? ForbidMessage) { }
    }
}
