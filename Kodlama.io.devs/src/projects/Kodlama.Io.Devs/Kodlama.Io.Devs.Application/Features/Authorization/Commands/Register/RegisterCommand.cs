using AutoMapper;
using Core.Security.Dtos;
using Core.Security.JWT;
using Kodlama.Io.Devs.Application.Features.Authorization.Rules;
using Kodlama.Io.Devs.Application.Services.Repositories;
using MediatR;

namespace Kodlama.Io.Devs.Application.Features.Authorization.Commands.Register
{
    public partial class RegisterCommand : IRequest<AccessToken>
    {
        public UserForRegisterDto UserForRegisterDto { get; set; }

        public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AccessToken>
        {
            private readonly IUserRepository _userRepository;
            private readonly IMapper _mapper;
            private readonly AuthorizationBusinessRules _authorizationBusinessRules;
            private readonly IMediator _mediator;

            public RegisterCommandHandler(IUserRepository userRepository, IMapper mapper, AuthorizationBusinessRules authorizationBusinessRules, IMediator mediator)
            {
                _userRepository = userRepository;
                _mapper = mapper;
                _authorizationBusinessRules = authorizationBusinessRules;
                _mediator = mediator;
            }
            public Task<AccessToken> Handle(RegisterCommand request, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }
        }
    }
}
