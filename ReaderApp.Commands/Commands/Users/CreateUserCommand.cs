using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ReaderApp.Commands.Infrastructure;
using ReaderApp.Data;
using ReaderApp.Data.Domain;
using ReaderApp.Data.DTOs.User;
using ReaderApp.Data.Exceptions;
using ReaderApp.Services.Abstract;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ReaderApp.Commands.Commands.Users
{
    public class CreateUserCommand : IRequest<UserDto>
    {
        public CreateUserDto User { get; set; }
    }

    public class CreateUserHandler : CommandHandlerBase, IRequestHandler<CreateUserCommand, UserDto>
    {
        private readonly IUserDictionary _userDictionary;

        public CreateUserHandler(
            ReaderAppContext dbContext,
            IAuthorizationService authService,
            IMapper mapper,
            IUserDictionary userDictionary) : base(dbContext, authService, mapper) => _userDictionary = userDictionary;

        public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _dbContext.Users.Where(user => user.Email == request.User.Email).FirstOrDefaultAsync();
            if (user != null)
                throw new BadRequestException("User with the same email already exists");

            user = _mapper.Map<User>(request.User);
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
            _userDictionary.Create(user.Id);

            return _mapper.Map<UserDto>(user);
        }
    }
}
