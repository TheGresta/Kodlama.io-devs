using AutoMapper;
using Core.Security.Dtos;
using Core.Security.Entities;
using Core.Security.Enums;
using Core.Security.Hashing;
using Core.Security.JWT;
using Kodlama.Io.Devs.Application.Features.DeveloperAuths.Constants;
using Kodlama.Io.Devs.Application.Features.DeveloperAuths.Dtos;
using Kodlama.Io.Devs.Application.Features.DeveloperAuths.Rules;
using Kodlama.Io.Devs.Application.Features.Developers.Commands.CreateDeveloper;
using Kodlama.Io.Devs.Application.Features.UserAuths.Dtos;
using Kodlama.Io.Devs.Application.Features.UserOperationClaims.Commands.CreateMultipleUserOperationClaim;
using Kodlama.Io.Devs.Application.Services.Repositories;
using Kodlama.Io.Devs.Application.Services.UserAuthService;
using Kodlama.Io.Devs.Domain.Entities;
using MediatR;

namespace Kodlama.Io.Devs.Application.Features.DeveloperAuths.Commands.RegisterDeveloperAuth
{
    public partial class RegisterDeveloperAuthCommand : IRequest<RegisterDeveloperAuthResultDto>
    {
        public UserForRegisterDto UserForRegisterDto { get; set; }
        public string IpAddress { get; set; }

        public class RegisterDeveloperAuthCommandHandler : IRequestHandler<RegisterDeveloperAuthCommand, RegisterDeveloperAuthResultDto>
        {
            private readonly IMediator _mediator;
            private readonly IMapper _mapper;
            private readonly DeveloperAuthBusinessRules _developerAuthBusinessRules;
            private readonly DeveloperOperationClaims _developerOperationClaims;
            private readonly IUserAuthService _userAuthService;

            public RegisterDeveloperAuthCommandHandler(IMediator mediator,
                                                       IMapper mapper,
                                                       DeveloperAuthBusinessRules developerAuthBusinessRules,
                                                       DeveloperOperationClaims developerOperationClaims,
                                                       IUserAuthService userAuthService)
            {
                _mediator = mediator;
                _mapper = mapper;
                _developerAuthBusinessRules = developerAuthBusinessRules;
                _developerOperationClaims = developerOperationClaims;
                _userAuthService = userAuthService;
            }

            public async Task<RegisterDeveloperAuthResultDto> Handle(RegisterDeveloperAuthCommand request, CancellationToken cancellationToken)
            {
                await _developerAuthBusinessRules.EmailShouldNotBeAlreadyExistWhenDeveloperRegister(request.UserForRegisterDto.Email);

                Developer developer = CreateDeveloper(request.UserForRegisterDto);
                CreateDeveloperCommand createDeveloperCommand = new() { Developer = developer };

                await _mediator.Send(createDeveloperCommand);
                await _userAuthService.AddUserOperationClaimsForCreatedUser(developer.Id, _developerOperationClaims.Roles);

                User user = _mapper.Map<User>(developer);

                AccessToken createdAccessToken = await _userAuthService.CreateAccessToken(user);
                RefreshToken createdRefreshToken = await _userAuthService.CreateRefreshToken(user, request.IpAddress);
                RefreshToken addedRefreshToken = await _userAuthService.AddRefreshToken(createdRefreshToken);

                RegisterDeveloperAuthResultDto registerDeveloperAuthResultDto = new()
                {
                    RefreshToken = addedRefreshToken,
                    AccessToken = createdAccessToken
                };

                return registerDeveloperAuthResultDto;
            }

            private Developer CreateDeveloper(UserForRegisterDto userForRegisterDto)
            {
                byte[] passwordHash, passwordSald;
                Developer developer = _mapper.Map<Developer>(userForRegisterDto);

                HashingHelper.CreatePasswordHash(userForRegisterDto.Password, out passwordHash, out passwordSald);

                developer.PasswordHash = passwordHash;
                developer.PasswordSalt = passwordSald;
                developer.Status = true;
                developer.AuthenticatorType = AuthenticatorType.Email;
                developer.GitHub = "";

                return developer;
            }
        }
    }
}
