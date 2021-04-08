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
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ReaderApp.Commands.Commands.Files
{
    public class DeleteFileCommand : BaseRequest, IRequest<Unit>
    {
        public Guid FileId { get; set; }
    }

    public class DeleteFileHandler : CommandHandlerBase, IRequestHandler<DeleteFileCommand>
    {
        private readonly IUserFileStore _userFilesStore;
        private readonly IFileProcessedWordsCache _filesWordsCache;

        public DeleteFileHandler(
            ReaderAppContext dbContext,
            IAuthorizationService authService,
            IMapper mapper,
            IUserFileStore userFilesStore,
            IFileProcessedWordsCache filesWordsCache) : base(dbContext, authService, mapper) => (_userFilesStore, _filesWordsCache) = (userFilesStore, filesWordsCache);

        public async Task<Unit> Handle(DeleteFileCommand request, CancellationToken cancellationToken)
        {
            var file = await _dbContext.TextFiles.FindAsync(request.FileId);
            if (file == null)
                throw new EntityNotFoundException(nameof(TextFile), request.FileId.ToString());

            var authResult = await _authService.AuthorizeAsync(request.User, file, FilesAuthOperations.Delete);
            if (!authResult.Succeeded)
                throw new ForbidException();

            _dbContext.TextFiles.Remove(file);
            await _dbContext.SaveChangesAsync();

            // TODO: Make store consistency. Transaction delete from DB and other storage
            _userFilesStore.DeleteFile(request.UserId, request.FileId);
            _filesWordsCache.ClearCache(request.UserId, request.FileId);

            return Unit.Value;
        }
    }
}
