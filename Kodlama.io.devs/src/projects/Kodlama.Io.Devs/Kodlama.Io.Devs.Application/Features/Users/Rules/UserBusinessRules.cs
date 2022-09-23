using Core.CrossCuttingConcerns.Exceptions;
using Core.Persistence.Paging;
using Core.Security.Entities;
using Core.Security.Hashing;
using Kodlama.Io.Devs.Application.Features.Users.Constants;
using Kodlama.Io.Devs.Application.Services.Repositories;

namespace Kodlama.Io.Devs.Application.Features.Users.Rules
{
    public class UserBusinessRules
    {
        private readonly IUserRepository _userRepository;
        private readonly UserBusinessRulesMessages _messages;

        public UserBusinessRules(IUserRepository userRepository, UserBusinessRulesMessages messages)
        {
            _userRepository = userRepository;
            _messages = messages;
        }

        public async Task UserShouldBeExistWithGivenUserId(int id)
        {
            User? user = await _userRepository.GetAsync(u => u.Id == id);
            if (user == null) throw new BusinessException(_messages.IdDoesNotExist);
        }

        public async Task ThereShouldBeNoOtherUserWithGivenEmailWhenUserUpdated(int id, string email)
        {
            User? user = await _userRepository.GetAsync(u => u.Email.ToLower() == email.ToLower() && u.Id != id);
            if (user != null) throw new BusinessException(_messages.EmailAlreadyExist);
        }

        public async Task PasswordShouldBeValidWhenUserTryingToUpdateProfile(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            if (!HashingHelper.VerifyPasswordHash(password, passwordHash, passwordSalt))
                throw new BusinessException(_messages.PasswordIsIncorrect);
        }

        public async Task UserShouldBeExistWithGivenEmailAddress(string email)
        {
            User? user = await _userRepository.GetAsync(u => u.Email.ToLower() == email.ToLower());
            if (user == null) throw new BusinessException(_messages.EmailDoesNotExist);
        }

        public async Task ThereShouldBeSomeDataInUserListAsRequired(IPaginate<User> users)
        {
            if (!users.Items.Any()) throw new BusinessException(_messages.ThereIsNoAnyDataInList);
        }
    }
}
