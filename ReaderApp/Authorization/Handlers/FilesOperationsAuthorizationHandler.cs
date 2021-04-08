using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using ReaderApp.Commands.AuthOperations;
using ReaderApp.Data;
using ReaderApp.Data.Domain;
using System.Threading.Tasks;

namespace ReaderApp.Authorization.Handlers
{
    public class FilesOperationsAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, TextFile>
    {
        private readonly ReaderAppContext _dbContext;

        public FilesOperationsAuthorizationHandler(ReaderAppContext dbContext) => _dbContext = dbContext;

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement, TextFile resource)
        {
            switch (requirement.Name)
            {
                case FilesAuthOperationsNames.Delete:
                case FilesAuthOperationsNames.Read:
                    var userId = context.User.GetId();
                    if (resource.UserId == userId)
                        context.Succeed(requirement);
                    break;
            }

            return Task.CompletedTask;
        }
    }
}
