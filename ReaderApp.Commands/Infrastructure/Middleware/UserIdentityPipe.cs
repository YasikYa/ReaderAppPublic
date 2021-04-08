using MediatR;
using Microsoft.AspNetCore.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ReaderApp.Commands.Infrastructure.Middleware
{
    public class UserIdentityPipe<TIn, TOut> : IPipelineBehavior<TIn, TOut>
    {
        private readonly HttpContext _httpContext;

        public UserIdentityPipe(IHttpContextAccessor httpContextAccessor) => _httpContext = httpContextAccessor.HttpContext;

        public Task<TOut> Handle(TIn request, CancellationToken cancellationToken, RequestHandlerDelegate<TOut> next)
        {
            if (request is BaseRequest baseRequest)
                baseRequest.User = _httpContext.User;

            return next();
        }
    }
}
