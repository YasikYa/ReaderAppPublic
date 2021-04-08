using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace ReaderApp.Commands.AuthOperations
{
    public static class FilesAuthOperationsNames
    {
        public const string Delete = "Delete";

        public const string Read = "Read";

        public const string GetAll = "GetAll";
    }

    public static class FilesAuthOperations
    {
        public static OperationAuthorizationRequirement Delete { get; set; } = new OperationAuthorizationRequirement { Name = FilesAuthOperationsNames.Delete };

        public static OperationAuthorizationRequirement Read { get; set; } = new OperationAuthorizationRequirement { Name = FilesAuthOperationsNames.Read };

        public static OperationAuthorizationRequirement GetAll { get; set; } = new OperationAuthorizationRequirement { Name = FilesAuthOperationsNames.GetAll };

    }
}
