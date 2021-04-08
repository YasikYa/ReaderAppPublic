using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using ReaderApp.Data;

namespace ReaderApp.Commands.Infrastructure
{
    public abstract class QueryHandlerBase : HandlerBase
    {
        protected QueryHandlerBase(
            ReaderAppContext dbContext,
            IAuthorizationService authservice,
            IMapper mapper) : base(dbContext, authservice, mapper)
        {
            dbContext.ChangeTracker.QueryTrackingBehavior = Microsoft.EntityFrameworkCore.QueryTrackingBehavior.NoTracking;
        }
    }
}
