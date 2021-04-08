using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using ReaderApp.Commands.AuthOperations;
using ReaderApp.Commands.Exceptions;
using ReaderApp.Commands.Infrastructure;
using ReaderApp.Data;
using ReaderApp.Data.Domain;
using ReaderApp.Data.Exceptions;
using ReaderApp.Services.Abstract;
using ReaderApp.Services.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ReaderApp.Commands.Queries.Words
{
    public class GetUnknownWordsQuery : BaseRequest, IRequest<IEnumerable<string>>
    {
        public Guid FileId { get; set; }
    }

    public class GetUnknownWordsHandler : QueryHandlerBase, IRequestHandler<GetUnknownWordsQuery, IEnumerable<string>>
    {
        private readonly IFileProcessedWordsCache _filesWordsCache;
        private readonly IUserDictionary _userDictionary;

        public GetUnknownWordsHandler(
            ReaderAppContext dbContext,
            IAuthorizationService authService,
            IMapper mapper,
            IFileProcessedWordsCache filesWordsCache,
            IUserDictionary userDictionary) : base(dbContext, authService, mapper) => (_filesWordsCache, _userDictionary) = (filesWordsCache, userDictionary);

        public async Task<IEnumerable<string>> Handle(GetUnknownWordsQuery request, CancellationToken cancellationToken)
        {
            var file = await _dbContext.TextFiles.FindAsync(request.FileId);
            if (file == null)
                throw new EntityNotFoundException(nameof(TextFile), request.FileId.ToString());

            var authResult = await _authService.AuthorizeAsync(request.User, file, FilesAuthOperations.Read);
            if (!authResult.Succeeded)
                throw new ForbidException();

            var fileLemmaGroups = await _filesWordsCache.ExtractCompressedAsync(request.UserId, request.FileId);
            var unknownWords = await _userDictionary.SelectUnknownWordsFromLemmaGroups(request.UserId, fileLemmaGroups);
            return unknownWords.ToList();
        }
    }
}
