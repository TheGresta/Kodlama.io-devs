using Core.CrossCuttingConcerns.Exceptions;
using Core.Security.Entities;
using Kodlama.Io.Devs.Application.Features.DeveloperAuths.Constants;
using Kodlama.Io.Devs.Application.Services.Repositories;

namespace Kodlama.Io.Devs.Application.Features.DeveloperAuths.Rules
{
    public class DeveloperAuthBusinessRules
    {
        private readonly IUserRepository _userRepository; 
        private readonly DeveloperAuthBusinessRulesMessages _messages;

        public DeveloperAuthBusinessRules(IUserRepository userRepository, DeveloperAuthBusinessRulesMessages messages)
        {
            _userRepository = userRepository;
            _messages = messages;
        }

        public async Task EmailShouldNotBeAlreadyExistWhenDeveloperRegister(string email)
        {
            User? user = await _userRepository.GetAsync(u => u.Email.ToLower() == email.ToLower());
            if (user != null) throw new BusinessException(_messages.EmailAlreadyExist);
        }
    }
}
