using System.Net;

namespace ReaderApp.Data.Exceptions
{
    public class EntityNotFoundException : AppExceptionBase
    {
        public EntityNotFoundException(string entityName, string entityId) : base($"{entityName} with id {entityId} not found") { }

        public override HttpStatusCode StatusCode => HttpStatusCode.NotFound;

        public EntityNotFoundException(string message)
            : base(message) { }
    }
}
