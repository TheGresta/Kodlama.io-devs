using Core.CrossCuttingConcerns.Exceptions;
using Core.Persistence.Paging;
using Core.Security.Entities;
using Kodlama.Io.Devs.Application.Services.Repositories;
using Kodlama.Io.Devs.Domain.Entities;

namespace Kodlama.Io.Devs.Application.Features.GitHubs.Rules
{
    public class GitHubBusinessRules
    {
        private readonly IGitHubRepository _gitHubRepository;
        private readonly GitHubBusinessRulesMessages _messages;
        private readonly IUserRepository _userRepository;

        public GitHubBusinessRules(GitHubBusinessRulesMessages messages, IGitHubRepository gitHubRepository, IUserRepository userRepository)
        {
            _messages = messages;
            _gitHubRepository = gitHubRepository;
            _userRepository = userRepository;
        }

        public async Task GitHubUserNameCanNotBeDuplicatedWhenInsertedOrUpdated(string name)
        {
            IPaginate<GitHub> result = await _gitHubRepository.GetListAsync(g => g.Name == name, enableTracking: false);
            if (result.Items.Any()) throw new BusinessException(_messages.GitHubUserNameAlreadyExist);
        }

        public async Task GitHubShouldBeExistWhenRequested(int id)
        {
            GitHub? gitHub = await _gitHubRepository.GetAsync(g => g.Id == id);
            if (gitHub == null) throw new BusinessException(_messages.GitHubDoesNotExist);
        }

        public async Task UserShouldBeExistWhenGitHubInsertedOrUpdated(int id)
        {
            User? user = await _userRepository.GetAsync(u => u.Id == id);
            if (user == null) throw new BusinessException(_messages.UserDoesNotExist);
        }

        public async Task ShouldBeSomeDataInTheGitHubTableWhenRequested(IPaginate<GitHub> gitHubs)
        {
            if (!gitHubs.Items.Any()) throw new BusinessException(_messages.GitHubDataDoesNotExist);
        }

        public async Task AnUserShouldHaveOnlyOneGitHubUserName(int userId)
        {
            GitHub? gitHub = await _gitHubRepository.GetAsync(u => u.UserId == userId);
            if (gitHub == null) throw new BusinessException(_messages.UserCanNotHaveMultipleGitHubAddress);
        }

        public async Task AnUserShouldHaveOnlyOneGitHubUserNameWhenGitHubUpdated(int id, int userId)
        {
            GitHub? gitHub = await _gitHubRepository.GetAsync(u => u.UserId == userId && u.Id != id);
            if (gitHub != null) throw new BusinessException(_messages.UserCanNotHaveMultipleGitHubAddress);
        }
    }
}
