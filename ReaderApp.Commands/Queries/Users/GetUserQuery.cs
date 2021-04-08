using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ReaderApp.Commands.Infrastructure;
using ReaderApp.Data;
using ReaderApp.Data.DTOs.User;
using ReaderApp.Data.Exceptions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ReaderApp.Commands.Queries.Users
{
    public class GetUserQuery : BaseRequest, IRequest<UserDto>
    {
    }

    public class LoginUserQuery : BaseRequest, IRequest<UserDto>
    {
        public LoginDto Credential { get; set; }
    }

    public class GetUserHandler : QueryHandlerBase,
        IRequestHandler<GetUserQuery, UserDto>,
        IRequestHandler<LoginUserQuery, UserDto>
    {
        public GetUserHandler(
            ReaderAppContext dbContext,
            IAuthorizationService authService,
            IMapper mapper) : base(dbContext, authService, mapper) { }

        public async Task<UserDto> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _dbContext.Users.FindAsync(request.UserId);
            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> Handle(LoginUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _dbContext.Users.Where(user => user.Email == request.Credential.Email).FirstOrDefaultAsync();
            if (user == null)
                throw new BadRequestException("User with provided email not found");

            if (user.Password != request.Credential.Password)
                throw new BadRequestException("Wrong password");

            return _mapper.Map<UserDto>(user);
        }
    }
}
