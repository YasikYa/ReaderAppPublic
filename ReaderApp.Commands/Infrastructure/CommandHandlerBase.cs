using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using ReaderApp.Data;

namespace ReaderApp.Commands.Infrastructure
{
    public abstract class CommandHandlerBase : HandlerBase
    {
        protected CommandHandlerBase(
            ReaderAppContext dbContext,
            IAuthorizationService authservice,
            IMapper mapper) : base(dbContext, authservice, mapper) { }
    }
}
