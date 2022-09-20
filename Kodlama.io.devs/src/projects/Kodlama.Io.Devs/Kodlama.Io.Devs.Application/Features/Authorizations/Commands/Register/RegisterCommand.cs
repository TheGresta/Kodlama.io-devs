using AutoMapper;
using Core.Security.Dtos;
using Core.Security.Entities;
using Core.Security.JWT;
using Kodlama.Io.Devs.Application.Features.Authorizations.Commands.Login;
using Kodlama.Io.Devs.Application.Features.Authorizations.Rules;
using Kodlama.Io.Devs.Application.Features.Users.Commands.CreateUser;
using Kodlama.Io.Devs.Application.Services.Repositories;
using MediatR;

namespace Kodlama.Io.Devs.Application.Features.Authorizations.Commands.Register
{
    public partial class RegisterCommand : IRequest<AccessToken>
    {
        public UserForRegisterDto UserForRegisterDto { get; set; }

        public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AccessToken>
        {
            private readonly AuthorizationBusinessRules _authorizationBusinessRules;
            private readonly IMediator _mediator;
            private readonly IOperationClaimRepository _operationClaimRepository;
            private readonly IMapper _mapper;

            public RegisterCommandHandler(AuthorizationBusinessRules authorizationBusinessRules, IMediator mediator, IOperationClaimRepository operationClaimRepository, IMapper mapper)
            {
                _authorizationBusinessRules = authorizationBusinessRules;
                _mediator = mediator;
                _operationClaimRepository = operationClaimRepository;
                _mapper = mapper;
            }
            public async Task<AccessToken> Handle(RegisterCommand request, CancellationToken cancellationToken)
            {
                await _authorizationBusinessRules.UserShouldNotExistsWhenRegister(request.UserForRegisterDto.Email);

                IList<int> operationClaimIdList = new List<int>() { _operationClaimRepository.Get(o => o.Name == "User").Id };
                CreateUserCommand createUserCommand = new() { UserForRegisterDto = request.UserForRegisterDto,                                                                
                                                              OperationClaimIdList = operationClaimIdList };
                
                await _mediator.Send(createUserCommand, cancellationToken);

                UserForLoginDto userForLoginDto = _mapper.Map<UserForLoginDto>(request.UserForRegisterDto);
                LoginCommand loginCommand = new() { UserForLoginDto = userForLoginDto };

                AccessToken accessToken = await _mediator.Send(loginCommand, cancellationToken);

                return accessToken;
            }
        }
    }
}
