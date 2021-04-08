using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using ReaderApp.Data;

namespace ReaderApp.Commands.Infrastructure
{
    public abstract class HandlerBase
    {
        protected readonly ReaderAppContext _dbContext;

        protected readonly IAuthorizationService _authService;

        protected readonly IMapper _mapper;

        protected HandlerBase(
            ReaderAppContext dbContext,
            IAuthorizationService authservice,
            IMapper mapper) => (_dbContext, _authService, _mapper) = (dbContext, authservice, mapper);
    }
}
