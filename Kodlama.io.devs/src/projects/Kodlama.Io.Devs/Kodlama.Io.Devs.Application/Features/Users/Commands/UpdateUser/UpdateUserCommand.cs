using AutoMapper;
using Core.Security.Dtos;
using Core.Security.Entities;
using Core.Security.JWT;
using Kodlama.Io.Devs.Application.Features.Authorizations.Commands.Login;
using Kodlama.Io.Devs.Application.Features.Users.Dtos;
using Kodlama.Io.Devs.Application.Features.Users.Rules;
using Kodlama.Io.Devs.Application.Services.Repositories;
using MediatR;

namespace Kodlama.Io.Devs.Application.Features.Users.Commands.UpdateUser
{
    public partial class UpdateUserCommand : IRequest<CommandUserDto>
    {
        public int UserId { get; set; }
        public UserForRegisterDto UserForRegisterDto { get; set; }
        public IList<int> OperationClaimIdList { get; set; }
        public string GitHubName { get; set; }

        public class CreateUserCommandHandler : IRequestHandler<UpdateUserCommand, CommandUserDto>
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

            public async Task<CommandUserDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
            {
                await _userBusinessRules.UserShouldBeExistWhenRequested(request.UserId);
                await _userBusinessRules.UserEmailCanNotBeDuplicatedWhenUpdated(request.UserId, request.UserForRegisterDto.Email);

                User user = new();
                CommandUserDto mappedUserDto = new();
                _userCommandCustomFunctions.SetUserPasswordWhenUserCreatedOrUpdated(request.UserForRegisterDto, out user);

                User updatedUser = await _userRepository.UpdateAsync(user);

                await _userCommandCustomFunctions.CreateOrUpdateGitHubAsync(updatedUser.Id, request.GitHubName, cancellationToken);
                await _userCommandCustomFunctions.CreateOrUpdateOperationClaimsAsync(request.OperationClaimIdList, updatedUser.Id, cancellationToken);
                _userCommandCustomFunctions.SetCommandUserDtoWhenRequested(updatedUser.Id, out mappedUserDto);

                mappedUserDto =  _mapper.Map<CommandUserDto>(updatedUser);

                return mappedUserDto;
            }
        }
    }
}
