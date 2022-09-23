using AutoMapper;
using Core.Security.Dtos;
using Core.Security.Entities;
using Core.Security.Enums;
using Core.Security.Hashing;
using Kodlama.Io.Devs.Application.Features.DeveloperAuths.Dtos;
using Kodlama.Io.Devs.Application.Features.DeveloperAuths.Rules;
using Kodlama.Io.Devs.Application.Features.Developers.Commands.CreateDeveloper;
using Kodlama.Io.Devs.Application.Features.UserAuths.Commands.CreateAccessTokeUserAuth;
using Kodlama.Io.Devs.Application.Features.UserAuths.Dtos;
using Kodlama.Io.Devs.Application.Services.Repositories;
using Kodlama.Io.Devs.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization.Infrastructure;

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

            public RegisterDeveloperAuthCommandHandler(IMediator mediator, IMapper mapper, DeveloperAuthBusinessRules developerAuthBusinessRules, IUserOperationClaimRepository userOperationClaimRepository, IOperationClaimRepository operationClaimRepository)
            {
                _mediator = mediator;
                _mapper = mapper;
                _developerAuthBusinessRules = developerAuthBusinessRules;
                _userOperationClaimRepository = userOperationClaimRepository;
                _operationClaimRepository = operationClaimRepository;
            }

            public async Task<RegisterDeveloperAuthResultDto> Handle(RegisterDeveloperAuthCommand request, CancellationToken cancellationToken)
            {
                await _developerAuthBusinessRules.EmailShouldNotBeAlreadyExistWhenDeveloperRegister(request.UserForRegisterDto.Email);

                Developer developer = CreateDeveloper(request.UserForRegisterDto);
                CreateDeveloperCommand createDeveloperCommand = new() { Developer = developer };

                await _mediator.Send(createDeveloperCommand);
                await CreateUserOperationClaimForCreatedDeveloper(developer.Id);

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

            private async Task CreateUserOperationClaimForCreatedDeveloper(int userId)
            {
                OperationClaim? operationClaim = await _operationClaimRepository.GetAsync(o => o.Name == "Developer");

                await _developerAuthBusinessRules.OperationClaimShouldBeExistAfterDeveloperCreated(operationClaim);

                UserOperationClaim userOperationClaim = new() { OperationClaimId = operationClaim.Id, UserId = userId };

                await _userOperationClaimRepository.AddAsync(userOperationClaim);
            }
        }
    }
}
