using Core.CrossCuttingConcerns.Exceptions;
using Core.Persistence.Paging;
using Core.Security.Entities;
using Kodlama.Io.Devs.Application.Services.Repositories;

namespace Kodlama.Io.Devs.Application.Features.Users.Rules
{
    public class UserBusinessRules
    {
        private readonly IUserRepository _userRepository;
        private readonly IGitHubRepository _gitHubRepository;
        private readonly IUserOperationClaimRepository _userOperationClaimRepository;
        private readonly UserBusinessRulesMessages _messages;

        public UserBusinessRules(IUserRepository userRepository, IGitHubRepository gitHubRepository, 
            IUserOperationClaimRepository userOperationClaimRepository, UserBusinessRulesMessages messages)
        {
            _userRepository = userRepository;
            _gitHubRepository = gitHubRepository;
            _userOperationClaimRepository = userOperationClaimRepository;
            _messages = messages;
        }

        public async Task UserEmailCanNotBeDuplicatedWhenInserted(string email)
        {
            User? user = await _userRepository.GetAsync(u => u.Email.ToLower() == email.ToLower());
            if (user != null) throw new BusinessException(_messages.UserEmailAlreadExist);
        }

        public async Task UserEmailCanNotBeDuplicatedWhenUpdated(int userId, string email)
        {
            User? user = await _userRepository.GetAsync(u => u.Email.ToLower() == email.ToLower() && u.Id != userId);
            if (user != null) throw new BusinessException(_messages.UserEmailAlreadExist);
        }

        public async Task UserShouldBeExistWhenRequested(int id)
        {
            User? user = await _userRepository.GetAsync(u => u.Id == id);
            if (user == null) throw new BusinessException(_messages.UserDoesNotFound);
        }

        public  async Task ShouldBeSomeDataInTheUserTableWhenRequested(IPaginate<User> users)
        {
            if (!users.Items.Any()) throw new BusinessException(_messages.UserDataDoesNotExist);
        }
    }
}
