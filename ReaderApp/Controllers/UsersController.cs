using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReaderApp.Authorization;
using ReaderApp.Commands.Commands.Users;
using ReaderApp.Commands.Queries.Users;
using ReaderApp.Data.DTOs.User;
using System;
using System.Threading.Tasks;

namespace ReaderApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator) => _mediator = mediator;

        [Authorize]
        [HttpGet("me")]
        public async Task<ActionResult<UserDto>> Me()
        {
            return Ok(await _mediator.Send(new GetUserQuery()));
        }

        [HttpPost("signup")]
        public async Task<ActionResult> CreateUser([FromBody]CreateUserDto userModel)
        {
            return Ok(await _mediator.Send(new CreateUserCommand() { User = userModel }));
        }

        [HttpPost("token")]
        public async Task<ActionResult> Token([FromBody]LoginDto model, [FromServices]IJWTConfig jwtConfig)
        {
            var user = await _mediator.Send(new LoginUserQuery() { Credential = model });
            return Ok(new { access_token = JWTHelper.CreateTokenJson(user, jwtConfig), expires_in = TimeSpan.FromDays(1).TotalSeconds });
        }
    }
}
