using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReaderApp.Data.DTOs.File;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using ReaderApp.Commands.Queries.Files;
using ReaderApp.Commands.Commands.Files;

namespace ReaderApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly IMediator _mediatr;

        public FilesController(IMediator mediatr) => _mediatr = mediatr;

        [Authorize]
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<FileDto>>> GetUserFiles([FromQuery]Guid userId)
        {
            return Ok(await _mediatr.Send(new GetUserFilesQuery() { RequestedUserId = userId }));
        }

        [Authorize]
        [HttpGet("load/{fileId}")]
        public async Task<ActionResult> LoadFile([FromRoute]Guid fileId)
        {
            var fileResult = await _mediatr.Send(new LoadFileQuery() { FileId = fileId });
            return File(fileResult.Content, fileResult.ContentType);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<UploadFileResponse>> Upload([FromForm]IFormFile file)
        {
            return Ok(await _mediatr.Send(new UploadFileCommand() { File = file }));
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute]Guid id)
        {
            return Ok(await _mediatr.Send(new DeleteFileCommand() { FileId = id }));
        }
    }
}
