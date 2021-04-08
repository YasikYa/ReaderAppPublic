using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ReaderApp.Commands.AuthOperations;
using ReaderApp.Commands.Exceptions;
using ReaderApp.Commands.Infrastructure;
using ReaderApp.Data;
using ReaderApp.Data.Domain;
using ReaderApp.Data.DTOs.File;
using ReaderApp.Data.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ReaderApp.Commands.Queries.Files
{
    public class GetUserFilesQuery : BaseRequest, IRequest<IEnumerable<FileDto>>
    {
        public Guid RequestedUserId { get; set; }
    }

    public class FilesQueriesHandler : QueryHandlerBase, IRequestHandler<GetUserFilesQuery, IEnumerable<FileDto>>
    {
        public FilesQueriesHandler(
            ReaderAppContext dbContext,
            IAuthorizationService authService,
            IMapper mapper) : base(dbContext, authService, mapper) {}

        public async Task<IEnumerable<FileDto>> Handle(GetUserFilesQuery request, CancellationToken cancellationToken)
        {
            var user = await _dbContext.Users.FindAsync(request.RequestedUserId);
            if (user == null)
                throw new EntityNotFoundException(nameof(User), request.RequestedUserId.ToString());

            var authResult = await _authService.AuthorizeAsync(request.User, user, UsersAuthOperations.GetAllFiles);
            if (!authResult.Succeeded)
                throw new ForbidException();

             return await _dbContext.TextFiles
                .Where(file => file.UserId == request.UserId)
                .Select(file => _mapper.Map<FileDto>(file))
                .ToListAsync();
        }
    }
}
