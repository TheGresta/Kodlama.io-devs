using AutoMapper;
using Core.Security.Dtos;
using Core.Security.Entities;
using Core.Security.JWT;
using Kodlama.Io.Devs.Application.Features.Authorizations.Commands.Login;
using Kodlama.Io.Devs.Application.Features.Users.Rules;
using Kodlama.Io.Devs.Application.Services.Repositories;
using MediatR;

namespace Kodlama.Io.Devs.Application.Features.Users.Commands.CreateUser
{
    public partial class CreateUserCommand : IRequest<AccessToken>
    {
        public UserForRegisterDto UserForRegisterDto { get; set; }
        public IList<int> OperationClaimIdList { get; set; }
        public string GitHubName { get; set; }

        public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, AccessToken>
        {
            private readonly IUserRepository _userRepository;
            private readonly IMapper _mapper;            
            private readonly IMediator _mediator;
            private readonly UserBusinessRules _userBusinessRules;
            private readonly UserCommandCustomFunctions _userCommandCustomFunctions;

            public CreateUserCommandHandler(IUserRepository userRepository, IMapper mapper,
                                            UserBusinessRules userBusinessRules, IMediator mediator, 
                                            UserCommandCustomFunctions userCommandCustomFunctions)
            {
                _userRepository = userRepository;
                _mapper = mapper;
                _userBusinessRules = userBusinessRules;
                _mediator = mediator;
                _userCommandCustomFunctions = userCommandCustomFunctions;
            }

            public async Task<AccessToken> Handle(CreateUserCommand request, CancellationToken cancellationToken)
            {
                await _userBusinessRules.UserEmailCanNotBeDuplicatedWhenInsertedOrUpdated(request.UserForRegisterDto.Email);

                User user = new();
                _userCommandCustomFunctions.SetUserPasswordWhenUserCreatedOrUpdated(request.UserForRegisterDto, out user);

                User addedUser = await _userRepository.AddAsync(user);

                await _userCommandCustomFunctions.CreateOrUpdateGitHubAsync(addedUser.Id, request.GitHubName, cancellationToken);
                await _userCommandCustomFunctions.CreateOrUpdateOperationClaimsAsync(request.OperationClaimIdList, addedUser.Id, cancellationToken);

                UserForLoginDto userForLoginDto = _mapper.Map<UserForLoginDto>(request.UserForRegisterDto);
                LoginCommand loginCommand = new() { UserForLoginDto = userForLoginDto };

                AccessToken accessToken = await _mediator.Send(loginCommand, cancellationToken);
                return accessToken;
            }
        }
    }
}
