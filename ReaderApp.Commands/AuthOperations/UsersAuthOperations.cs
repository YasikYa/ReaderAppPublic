using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace ReaderApp.Commands.AuthOperations
{
    public class UserAuthOperationsNames
    {
        public const string GetAllFiles = "GetAllFiles";
    }

    public static class UsersAuthOperations
    {
        public static OperationAuthorizationRequirement GetAllFiles { get; set; } = new OperationAuthorizationRequirement { Name = UserAuthOperationsNames.GetAllFiles };
    }
}
