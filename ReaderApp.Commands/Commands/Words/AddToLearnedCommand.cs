using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using ReaderApp.Commands.Infrastructure;
using ReaderApp.Data;
using ReaderApp.Services.Abstract;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ReaderApp.Commands.Commands.Words
{
    public class AddToLearnedCommand : BaseRequest, IRequest<Unit>
    {
        public IEnumerable<string> Words { get; set; }
    }

    public class AddToLearnedHandle : CommandHandlerBase, IRequestHandler<AddToLearnedCommand>
    {
        private readonly IUserDictionary _userDictionary;

        public AddToLearnedHandle(
            ReaderAppContext dbContext,
            IAuthorizationService authService,
            IMapper mapper,
            IUserDictionary userDictionary) : base(dbContext, authService, mapper) => _userDictionary = userDictionary;

        public async Task<Unit> Handle(AddToLearnedCommand request, CancellationToken cancellationToken)
        {
            await _userDictionary.AddToLearned(request.UserId, request.Words);
            return Unit.Value;
        }
    }
}
