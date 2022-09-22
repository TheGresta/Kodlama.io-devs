using AutoMapper;
using Core.Security.Dtos;
using Core.Security.Entities;
using Core.Security.Extensions;
using Kodlama.Io.Devs.Application.Features.Users.Dtos;
using Kodlama.Io.Devs.Application.Features.Users.Rules;
using Kodlama.Io.Devs.Application.Services.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Kodlama.Io.Devs.Application.Features.Users.Commands.UpdateUserSelfProfile
{
    public partial class UpdateUserSelfProfileCommand : IRequest<CommandUserDto>
    {
        public UserForRegisterDto UserForRegisterDto { get; set; }
        public string GitHubName { get; set; }

        public class UpdateUserSelfProfileCommandHandler : IRequestHandler<UpdateUserSelfProfileCommand, CommandUserDto>
        {
            private readonly IUserRepository _userRepository;
            private readonly IMapper _mapper;
            private readonly UserBusinessRules _userBusinessRules;
            private readonly UserCustomFunctions _userCustomFunctions;
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly IOperationClaimRepository _operationClaimRepository;

            public UpdateUserSelfProfileCommandHandler(IUserRepository userRepository,
                                                       IMapper mapper,
                                                       UserBusinessRules userBusinessRules,
                                                       UserCustomFunctions userCommandCustomFunctions,
                                                       IHttpContextAccessor httpContextAccessor,
                                                       IOperationClaimRepository operationClaimRepository)
            {
                _userRepository = userRepository;
                _mapper = mapper;
                _userBusinessRules = userBusinessRules;
                _userCustomFunctions = userCommandCustomFunctions;
                _httpContextAccessor = httpContextAccessor;
                _operationClaimRepository = operationClaimRepository;
            }

            public async Task<CommandUserDto> Handle(UpdateUserSelfProfileCommand request, CancellationToken cancellationToken)
            {
                IList<int> operationClaimIdList = new List<int>() { _operationClaimRepository.Get(o => o.Name == "User").Id };
                int userId = _httpContextAccessor.HttpContext.User.GetUserId();

                await _userBusinessRules.UserShouldBeExistWhenRequested(userId);
                await _userBusinessRules.UserEmailCanNotBeDuplicatedWhenUpdated(userId, request.UserForRegisterDto.Email);

                User? user = await _userRepository.GetAsync(u => u.Id == userId);
                _userCustomFunctions.SetUserPasswordWhenUserUpdated(request.UserForRegisterDto, ref user);
                User updatedUser = await _userRepository.UpdateAsync(user);

                await _userCustomFunctions.CreateOrUpdateGitHubAsync(updatedUser.Id, request.GitHubName, cancellationToken);
                await _userCustomFunctions.CreateOrUpdateOperationClaimsAsync(operationClaimIdList, updatedUser.Id, cancellationToken);

                CommandUserDto mappedUserDto = _mapper.Map<CommandUserDto>(updatedUser);

                _userCustomFunctions.SetCommandUserDtoWhenRequested(updatedUser.Id, ref mappedUserDto);

                return mappedUserDto;
            }
        }
    }
}
