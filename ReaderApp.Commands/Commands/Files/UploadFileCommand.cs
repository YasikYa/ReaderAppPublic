using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using ReaderApp.Commands.Infrastructure;
using ReaderApp.Data;
using ReaderApp.Data.Domain;
using ReaderApp.Data.DTOs.File;
using ReaderApp.Data.Exceptions;
using ReaderApp.Services.Abstract;
using ReaderApp.Services.Concrete.WordsFilters;
using ReaderApp.Services.Extensions;
using ReaderApp.Services.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ReaderApp.Commands.Commands.Files
{
    public class UploadFileResponse
    {
        public IEnumerable<string> UnknownWords { get; set; }

        public FileDto FileInfo { get; set; }
    }

    public class UploadFileCommand : BaseRequest, IRequest<UploadFileResponse>
    {
        public IFormFile File { get; set; }
    }

    public class UploadFileHandler : CommandHandlerBase, IRequestHandler<UploadFileCommand, UploadFileResponse>
    {
        private readonly IUserFileStore _userFilesStore;
        private readonly IFileProcessedWordsCache _filesWordsCache;
        private readonly ILemmatizer _lemmatizer;
        private readonly IUserDictionary _userDictionary;
        private readonly PipeBuilder _pipeBuilder;

        public UploadFileHandler(
            ReaderAppContext dbContext,
            IAuthorizationService authService,
            IMapper mapper,
            IUserFileStore userFilesStore,
            IFileProcessedWordsCache filesWordsCache,
            ILemmatizer lemmatizer,
            IUserDictionary userDictionary,
            PipeBuilder pipeBuilder) : base(dbContext, authService, mapper) => (_userFilesStore, _filesWordsCache, _lemmatizer, _userDictionary, _pipeBuilder) = (userFilesStore, filesWordsCache, lemmatizer, userDictionary, pipeBuilder);

        public async Task<UploadFileResponse> Handle(UploadFileCommand request, CancellationToken cancellationToken)
        {
            ValidateFile(request.File);
            var fileDto = await SaveToDb(request.File, request.UserId);

            using (var fileStream = _userFilesStore.CreateWriteStream(request.UserId, fileDto.Id))
                await request.File.CopyToAsync(fileStream);

            var fileWords = await GetFileWords(request.File);
            var lemmaGroups = await LemmaGroup.GroupLemmas(fileWords, _lemmatizer);
            await _filesWordsCache.CacheCompressedAsync(request.UserId, fileDto.Id, lemmaGroups);
            var unknownWords = await _userDictionary.SelectUnknownWordsFromLemmaGroups(request.UserId, lemmaGroups);
            
            return new UploadFileResponse
            {
                FileInfo = fileDto,
                UnknownWords = unknownWords
            };
        }

        private void ValidateFile(IFormFile file)
        {
            if (file.Length == 0)
                throw new BadRequestException("File is empty");

            var fileExtension = Path.GetExtension(file.FileName);
            if (fileExtension != ".txt")
                throw new BadRequestException("Unsupported file extension");
        }

        private async Task<FileDto> SaveToDb(IFormFile file, Guid userId)
        {
            var fileEntity = new TextFile
            {
                FileName = file.FileName,
                UserId = userId
            };

            _dbContext.TextFiles.Add(fileEntity);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<FileDto>(fileEntity);
        }

        private async Task<IEnumerable<string>> GetFileWords(IFormFile file)
        {
            string content;
            using (var reader = new StreamReader(file.OpenReadStream()))
                content = await reader.ReadToEndAsync();

            var allWords = content.Split(new[] { ' ', '\n' });
            var pipe = await _pipeBuilder.BuildCompressPipe();

            return pipe.Pipe(allWords);
        }
    }
}
