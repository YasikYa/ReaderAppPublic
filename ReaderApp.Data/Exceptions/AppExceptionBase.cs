using System;
using System.Net;

namespace ReaderApp.Data.Exceptions
{
    public abstract class AppExceptionBase : Exception
    {
        public abstract HttpStatusCode StatusCode { get; }

        protected AppExceptionBase(string message) : base(message) {}
    }
}
