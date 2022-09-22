using AutoMapper;
using Core.Security.Dtos;
using Core.Security.Entities;
using Kodlama.Io.Devs.Application.Features.Users.Dtos;
using Kodlama.Io.Devs.Application.Features.Users.Rules;
using Kodlama.Io.Devs.Application.Services.Repositories;
using MediatR;

namespace Kodlama.Io.Devs.Application.Features.Users.Commands.UpdateUser
{
    public partial class UpdateUserCommand : IRequest<CommandUserDto>
    {
        public int Id { get; set; }
        public UserForRegisterDto UserForRegisterDto { get; set; }
        public IList<int> OperationClaimIdList { get; set; }
        public string GitHubName { get; set; }

        public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, CommandUserDto>
        {
            private readonly IUserRepository _userRepository;
            private readonly IMapper _mapper;
            private readonly IMediator _mediator;
            private readonly UserBusinessRules _userBusinessRules;
            private readonly UserCustomFunctions _userCustomFunctions;

            public UpdateUserCommandHandler(IUserRepository userRepository, IMapper mapper,
                                            UserBusinessRules userBusinessRules, IMediator mediator,
                                            UserCustomFunctions userCommandCustomFunctions)
            {
                _userRepository = userRepository;
                _mapper = mapper;
                _userBusinessRules = userBusinessRules;
                _mediator = mediator;
                _userCustomFunctions = userCommandCustomFunctions;
            }

            public async Task<CommandUserDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
            {
                await _userBusinessRules.UserShouldBeExistWhenRequested(request.Id);
                await _userBusinessRules.UserEmailCanNotBeDuplicatedWhenUpdated(request.Id, request.UserForRegisterDto.Email);

                User? user = await _userRepository.GetAsync(u => u.Id == request.Id);                
                _userCustomFunctions.SetUserPasswordWhenUserUpdated(request.UserForRegisterDto, ref user);
                User updatedUser = await _userRepository.UpdateAsync(user);

                await _userCustomFunctions.CreateOrUpdateGitHubAsync(updatedUser.Id, request.GitHubName, cancellationToken);
                await _userCustomFunctions.CreateOrUpdateOperationClaimsAsync(request.OperationClaimIdList, updatedUser.Id, cancellationToken);

                CommandUserDto mappedUserDto = _mapper.Map<CommandUserDto>(updatedUser);

                _userCustomFunctions.SetCommandUserDtoWhenRequested(updatedUser.Id, ref mappedUserDto);               

                return mappedUserDto;
            }
        }
    }
}
