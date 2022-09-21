using AutoMapper;
using Core.Security.Dtos;
using Core.Security.Entities;
using Core.Security.JWT;
using Kodlama.Io.Devs.Application.Features.Authorizations.Commands.Login;
using Kodlama.Io.Devs.Application.Features.Users.Dtos;
using Kodlama.Io.Devs.Application.Features.Users.Rules;
using Kodlama.Io.Devs.Application.Services.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Kodlama.Io.Devs.Application.Features.Users.Commands.UpdateUser
{
    public partial class UpdateUserCommand : IRequest<CommandUserDto>
    {
        public int Id { get; set; }
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
                await _userBusinessRules.UserShouldBeExistWhenRequested(request.Id);
                await _userBusinessRules.UserEmailCanNotBeDuplicatedWhenUpdated(request.Id, request.UserForRegisterDto.Email);

                User? user = await _userRepository.GetAsync(u => u.Id == request.Id);                
                _userCommandCustomFunctions.SetUserPasswordWhenUserUpdated(request.UserForRegisterDto, ref user);
                User updatedUser = await _userRepository.UpdateAsync(user);

                await _userCommandCustomFunctions.CreateOrUpdateGitHubAsync(updatedUser.Id, request.GitHubName, cancellationToken);
                await _userCommandCustomFunctions.CreateOrUpdateOperationClaimsAsync(request.OperationClaimIdList, updatedUser.Id, cancellationToken);

                CommandUserDto mappedUserDto = _mapper.Map<CommandUserDto>(updatedUser);

                _userCommandCustomFunctions.SetCommandUserDtoWhenRequested(updatedUser.Id, ref mappedUserDto);               

                return mappedUserDto;
            }
        }
    }
}
