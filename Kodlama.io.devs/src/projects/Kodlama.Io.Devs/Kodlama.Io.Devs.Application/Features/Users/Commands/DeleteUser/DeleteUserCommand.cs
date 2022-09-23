using AutoMapper;
using Core.Persistence.Paging;
using Core.Security.Entities;
using Kodlama.Io.Devs.Application.Features.Users.Dtos;
using Kodlama.Io.Devs.Application.Features.Users.Rules;
using Kodlama.Io.Devs.Application.Services.Repositories;
using Kodlama.Io.Devs.Domain.Entities;
using MediatR;

namespace Kodlama.Io.Devs.Application.Features.Users.Commands.DeleteUser
{
    public partial class DeleteUserCommand : IRequest<DeletedUserDto>
    {
        public int Id { get; set; }
        public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, DeletedUserDto>
        {
            private readonly IUserRepository _userRepository;
            private readonly IMapper _mapper;
            private readonly UserBusinessRules _userBusinessRules;
            private readonly IGitHubRepository _gitHubRepository;
            private readonly IUserOperationClaimRepository _userOperationClaimRepository;

            public DeleteUserCommandHandler(IUserRepository userRepository,
                                            IMapper mapper,
                                            UserBusinessRules userBusinessRules,
                                            IGitHubRepository gitHubRepository,
                                            IUserOperationClaimRepository userOperationClaimRepository)
            {
                _userRepository = userRepository;
                _mapper = mapper;
                _userBusinessRules = userBusinessRules;
                _gitHubRepository = gitHubRepository;
                _userOperationClaimRepository = userOperationClaimRepository;
            }

            public async Task<DeletedUserDto> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
            {
                await _userBusinessRules.UserShouldBeExistWhenRequested(request.Id);

                User? user = await _userRepository.GetAsync(u => u.Id == request.Id);
                User deletedUser = await _userRepository.DeleteAsync(user);

                await DeleteGitHubAddressWhenUserDeleted(deletedUser.Id);
                await DeleteAllUserOperationClaimsWhenUserDeleted(deletedUser.Id);

                DeletedUserDto mappedUserDto = _mapper.Map<DeletedUserDto>(deletedUser);
                return mappedUserDto;
            }

            private async Task DeleteGitHubAddressWhenUserDeleted(int userId)
            {
                GitHub? gitHub = _gitHubRepository.Get(g => g.UserId == userId);

                if (gitHub != null) await _gitHubRepository.DeleteAsync(gitHub);
            }

            private async Task DeleteAllUserOperationClaimsWhenUserDeleted(int userId)
            {
                IPaginate<UserOperationClaim> userOperationClaims = _userOperationClaimRepository.GetList(u => u.UserId == userId);

                foreach (UserOperationClaim userOperationClaim in userOperationClaims.Items)
                {
                    await _userOperationClaimRepository.DeleteAsync(userOperationClaim);
                }
            }
        }
    }
}
