using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using ReaderApp.Commands.AuthOperations;
using ReaderApp.Data;
using ReaderApp.Data.Domain;
using System.Threading.Tasks;

namespace ReaderApp.Authorization.Handlers
{
    public class UsersOperationsAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, User>
    {
        private readonly ReaderAppContext _dbContext;

        public UsersOperationsAuthorizationHandler(ReaderAppContext dbContext) => _dbContext = dbContext;

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement, User resource)
        {
            switch (requirement.Name)
            {
                case UserAuthOperationsNames.GetAllFiles:
                    var requestUserId = context.User.GetId();
                    if (resource.Id == requestUserId)
                        context.Succeed(requirement);

                    break;
            }

            return Task.CompletedTask;
        }
    }
}
