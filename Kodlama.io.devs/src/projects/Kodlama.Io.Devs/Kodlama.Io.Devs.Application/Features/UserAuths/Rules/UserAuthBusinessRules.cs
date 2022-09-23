using Core.CrossCuttingConcerns.Exceptions;
using Core.Security.Entities;
using Core.Security.Hashing;
using Kodlama.Io.Devs.Application.Features.UserAuths.Constants;
using Kodlama.Io.Devs.Application.Services.Repositories;

namespace Kodlama.Io.Devs.Application.Features.UserAuths.Rules
{
    public class UserAuthBusinessRules
    {
        private readonly IUserRepository _userRepository;
        private readonly UserAuthBusinessRulesMessages _messages;

        public UserAuthBusinessRules(IUserRepository userRepository, UserAuthBusinessRulesMessages messages)
        {
            _userRepository = userRepository;
            _messages = messages;
        }

        public async Task EmailShouldBeExistInTheUserTableWhenUserTryingToLogin(string email)
        {
            User? user = await _userRepository.GetAsync(u => u.Email.ToLower() == email.ToLower());
            if(user == null) throw new BusinessException(_messages.EmailNotFound);
        }

        public async Task PasswordShouldBeValidWhenUserTryingToLogin(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            if (!HashingHelper.VerifyPasswordHash(password, passwordHash, passwordSalt))
                throw new BusinessException(_messages.PasswordIsIncorrect);
        }

        public async Task EmailShouldNotBeAlreadyExistInTheUserTableWhenUserTryingToRegister(string email)
        {
            User? user = await _userRepository.GetAsync(u => u.Email.ToLower() == email.ToLower());
            if (user != null) throw new BusinessException(_messages.EmailAlreadyExist);
        }
    }
}
