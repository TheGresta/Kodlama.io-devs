using AutoMapper;
using Core.Security.Dtos;
using Core.Security.Entities;
using Core.Security.Enums;
using Core.Security.Hashing;
using Core.Security.JWT;
using Kodlama.Io.Devs.Application.Features.Authorizations.Commands.Login;
using Kodlama.Io.Devs.Application.Features.Authorizations.Rules;
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
            private readonly IMapper _mapper;
            private readonly IUserRepository _userRepository;
            private readonly IMediator _mediator;
            private readonly IUserOperationClaimRepository _userOperationClaimRepository;
            private readonly IOperationClaimRepository _operationClaimRepository;

            public RegisterCommandHandler(AuthorizationBusinessRules authorizationBusinessRules, 
                                          IMapper mapper, 
                                          IUserRepository userRepository, 
                                          IMediator mediator, 
                                          IUserOperationClaimRepository userOperationClaimRepository, 
                                          IOperationClaimRepository operationClaimRepository)
            {
                _authorizationBusinessRules = authorizationBusinessRules;
                _mapper = mapper;
                _userRepository = userRepository;
                _mediator = mediator;
                _userOperationClaimRepository = userOperationClaimRepository;
                _operationClaimRepository = operationClaimRepository;
            }
            public async Task<AccessToken> Handle(RegisterCommand request, CancellationToken cancellationToken)
            {
                await _authorizationBusinessRules.UserShouldNotExistsWhenRegister(request.UserForRegisterDto.Email);

                User user = new();
                SetUserPasswordWhenUserCreated(request.UserForRegisterDto, out user);

                User addedUser = await _userRepository.AddAsync(user);

                await SetUserOperationClaimAsUserWhenRegister(addedUser.Id);

                UserForLoginDto userForLoginDto = _mapper.Map<UserForLoginDto>(request.UserForRegisterDto);
                LoginCommand loginCommand = new() { UserForLoginDto = userForLoginDto };

                AccessToken accessToken = await _mediator.Send(loginCommand, cancellationToken);

                return accessToken;
            }

            private void SetUserPasswordWhenUserCreated(UserForRegisterDto userForRegisterDto, out User user)
            {
                byte[] passwordHash, passwordSalt;
                HashingHelper.CreatePasswordHash(userForRegisterDto.Password, out passwordHash, out passwordSalt);

                user = _mapper.Map<User>(userForRegisterDto);
                user.Status = true;
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
                user.AuthenticatorType = AuthenticatorType.Email;
            }

            private async Task SetUserOperationClaimAsUserWhenRegister(int userId)
            {
                OperationClaim operationClaim = await _operationClaimRepository.GetAsync(o => o.Name == "User");

                await _authorizationBusinessRules.OperationClaimShouldBeExistForUserClaimWhenRegister(operationClaim);

                UserOperationClaim userOperationClaim = new() { UserId = userId, OperationClaimId = operationClaim.Id };

                UserOperationClaim addedUserOperationClaim = await _userOperationClaimRepository.AddAsync(userOperationClaim);

                await _authorizationBusinessRules.UserOperationClaimShouldBeAddedForUserClaimWhenRegister(addedUserOperationClaim);
            }
        }
    }
}
