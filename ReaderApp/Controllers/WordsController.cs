using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReaderApp.Commands.Commands.Words;
using ReaderApp.Commands.Queries.Words;
using ReaderApp.Data.DTOs.Word;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReaderApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WordsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WordsController(IMediator mediatr) => _mediator = mediatr;

        [Authorize]
        [HttpGet("unknown")]
        public async Task<ActionResult<IEnumerable<string>>> GetUnknown([FromQuery]Guid fileId)
        {
            return Ok(await _mediator.Send(new GetUnknownWordsQuery() { FileId = fileId }));
        }

        [Authorize]
        [HttpPost("learned")]
        public async Task<ActionResult> AddToLearned([FromBody]ICollection<string> learnedWords)
        {
            return Ok(await _mediator.Send(new AddToLearnedCommand() { Words = learnedWords }));
        }

        [Authorize]
        [HttpGet("{word}/info")]
        public async Task<ActionResult<WordInfoDto>> GetWordInfo(string word)
        {
            return Ok(await _mediator.Send(new GetWordInfoQuery() { Word = word }));
        }
    }
}
