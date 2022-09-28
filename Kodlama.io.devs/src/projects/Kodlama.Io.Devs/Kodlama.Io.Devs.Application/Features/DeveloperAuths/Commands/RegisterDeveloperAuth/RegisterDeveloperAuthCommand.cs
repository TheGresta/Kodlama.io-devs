using AutoMapper;
using Core.Security.Dtos;
using Core.Security.Entities;
using Core.Security.Enums;
using Core.Security.Hashing;
using Kodlama.Io.Devs.Application.Features.DeveloperAuths.Constants;
using Kodlama.Io.Devs.Application.Features.DeveloperAuths.Dtos;
using Kodlama.Io.Devs.Application.Features.DeveloperAuths.Rules;
using Kodlama.Io.Devs.Application.Features.Developers.Commands.CreateDeveloper;
using Kodlama.Io.Devs.Application.Features.UserAuths.Commands.CreateAccessTokeUserAuth;
using Kodlama.Io.Devs.Application.Features.UserAuths.Dtos;
using Kodlama.Io.Devs.Application.Features.UserOperationClaims.Commands.CreateMultipleUserOperationClaim;
using Kodlama.Io.Devs.Application.Services.Repositories;
using Kodlama.Io.Devs.Domain.Entities;
using MediatR;

namespace Kodlama.Io.Devs.Application.Features.DeveloperAuths.Commands.RegisterDeveloperAuth
{
    public partial class RegisterDeveloperAuthCommand : IRequest<RegisterDeveloperAuthResultDto>
    {
        public UserForRegisterDto UserForRegisterDto { get; set; }

        public class RegisterDeveloperAuthCommandHandler : IRequestHandler<RegisterDeveloperAuthCommand, RegisterDeveloperAuthResultDto>
        {
            private readonly IMediator _mediator;
            private readonly IMapper _mapper;
            private readonly DeveloperAuthBusinessRules _developerAuthBusinessRules;
            private readonly IUserOperationClaimRepository _userOperationClaimRepository;
            private readonly IOperationClaimRepository _operationClaimRepository;
            private readonly DeveloperOperationClaims _developerOperationClaims;

            public RegisterDeveloperAuthCommandHandler(IMediator mediator, 
                                                       IMapper mapper, 
                                                       DeveloperAuthBusinessRules developerAuthBusinessRules, 
                                                       IUserOperationClaimRepository userOperationClaimRepository, 
                                                       IOperationClaimRepository operationClaimRepository, 
                                                       DeveloperOperationClaims developerOperationClaims)
            {
                _mediator = mediator;
                _mapper = mapper;
                _developerAuthBusinessRules = developerAuthBusinessRules;
                _userOperationClaimRepository = userOperationClaimRepository;
                _operationClaimRepository = operationClaimRepository;
                _developerOperationClaims = developerOperationClaims;
            }

            public async Task<RegisterDeveloperAuthResultDto> Handle(RegisterDeveloperAuthCommand request, CancellationToken cancellationToken)
            {
                await _developerAuthBusinessRules.EmailShouldNotBeAlreadyExistWhenDeveloperRegister(request.UserForRegisterDto.Email);

                Developer developer = CreateDeveloper(request.UserForRegisterDto);
                CreateDeveloperCommand createDeveloperCommand = new() { Developer = developer };
                CreateMultipleUserOperationClaimCommand createMultipleUserOperationClaimCommand = new() { UserId = developer.Id, RoleNames = _developerOperationClaims.Roles };

                await _mediator.Send(createDeveloperCommand);
                await _mediator.Send(createMultipleUserOperationClaimCommand);

                User user = _mapper.Map<User>(developer);

                CreateAccessTokenUserAuthCommand createAccessTokenUserAuthCommand = new() { User = user };
                CreateAccessTokenUserAuthResultDto createAccessTokenUserAuthResultDto = await _mediator.Send(createAccessTokenUserAuthCommand);

                RegisterDeveloperAuthResultDto registerDeveloperAuthResultDto = _mapper.Map<RegisterDeveloperAuthResultDto>(createAccessTokenUserAuthResultDto);

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
