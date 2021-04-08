using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.StaticFiles;
using ReaderApp.Commands.AuthOperations;
using ReaderApp.Commands.Exceptions;
using ReaderApp.Commands.Infrastructure;
using ReaderApp.Data;
using ReaderApp.Data.Domain;
using ReaderApp.Data.Exceptions;
using ReaderApp.Services.Abstract;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ReaderApp.Commands.Queries.Files
{
    public class LoadFileResponse
    {
        public string ContentType { get; set; }

        public byte[] Content { get; set; }
    }

    public class LoadFileQuery : BaseRequest, IRequest<LoadFileResponse>
    {
        public Guid FileId { get; set; }
    }

    public class LoadFileQueryHandler : QueryHandlerBase, IRequestHandler<LoadFileQuery, LoadFileResponse>
    {
        private readonly IUserFileStore _userFilesStore;

        public LoadFileQueryHandler(
            ReaderAppContext dbContext,
            IAuthorizationService authService,
            IMapper mapper,
            IUserFileStore userFilesStore) : base(dbContext, authService, mapper) => _userFilesStore = userFilesStore;

        public async Task<LoadFileResponse> Handle(LoadFileQuery request, CancellationToken cancellationToken)
        {
            var file = await _dbContext.TextFiles.FindAsync(request.FileId);
            if (file == null)
                throw new EntityNotFoundException(nameof(TextFile), request.FileId.ToString());

            var authResult = await _authService.AuthorizeAsync(request.User, file, FilesAuthOperations.Read);
            if (!authResult.Succeeded)
                throw new ForbidException();

            new FileExtensionContentTypeProvider().TryGetContentType(file.FileName, out var contentType);
            byte[] fileContent = null;
            using (var stream = _userFilesStore.CreateReadStream(request.UserId, request.FileId))
            {
                using (var memoryStream = new MemoryStream())
                {
                    await stream.CopyToAsync(memoryStream);
                    fileContent = memoryStream.ToArray();
                }
            }

            return new LoadFileResponse
            {
                Content = fileContent,
                ContentType = contentType
            };
        }
    }
}
