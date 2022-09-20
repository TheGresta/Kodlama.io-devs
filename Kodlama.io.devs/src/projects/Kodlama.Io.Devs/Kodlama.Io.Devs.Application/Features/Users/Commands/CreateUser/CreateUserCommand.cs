using AutoMapper;
using Core.Application.Pipelines.Authorization;
using Core.Security.Dtos;
using Core.Security.Entities;
using Kodlama.Io.Devs.Application.Features.Users.Dtos;
using Kodlama.Io.Devs.Application.Features.Users.Rules;
using Kodlama.Io.Devs.Application.Services.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Kodlama.Io.Devs.Application.Features.Users.Commands.CreateUser
{
    public partial class CreateUserCommand : IRequest<CommandUserDto>
    {
        public UserForRegisterDto UserForRegisterDto { get; set; }
        public IList<int> OperationClaimIdList { get; set; }
        public string? GitHubName { get; set; }

        public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, CommandUserDto>
        {
            private readonly IUserRepository _userRepository;
            private readonly IMapper _mapper;            
            private readonly UserBusinessRules _userBusinessRules;
            private readonly UserCommandCustomFunctions _userCommandCustomFunctions;

            public CreateUserCommandHandler(IUserRepository userRepository, IMapper mapper,
                                            UserBusinessRules userBusinessRules, 
                                            UserCommandCustomFunctions userCommandCustomFunctions)
            {
                _userRepository = userRepository;
                _mapper = mapper;
                _userBusinessRules = userBusinessRules;
                _userCommandCustomFunctions = userCommandCustomFunctions;
            }

            public async Task<CommandUserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
            {
                await _userBusinessRules.UserEmailCanNotBeDuplicatedWhenInserted(request.UserForRegisterDto.Email);

                User user = new();
                CommandUserDto mappedUserDto = new();
                _userCommandCustomFunctions.SetUserPasswordWhenUserCreatedOrUpdated(request.UserForRegisterDto, out user);

                User addedUser = await _userRepository.AddAsync(user, include: x => x.Include(u => u.UserOperationClaims));

                await _userCommandCustomFunctions.CreateOrUpdateGitHubAsync(addedUser.Id, request.GitHubName, cancellationToken);
                await _userCommandCustomFunctions.CreateOrUpdateOperationClaimsAsync(request.OperationClaimIdList, addedUser.Id, cancellationToken);
                _userCommandCustomFunctions.SetCommandUserDtoWhenRequested(addedUser.Id, out mappedUserDto);

                mappedUserDto = _mapper.Map<CommandUserDto>(addedUser);

                return mappedUserDto;
            }
        }
    }
}
