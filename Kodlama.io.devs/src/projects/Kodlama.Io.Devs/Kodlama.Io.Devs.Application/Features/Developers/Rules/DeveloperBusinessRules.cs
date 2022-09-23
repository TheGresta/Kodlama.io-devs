using Core.CrossCuttingConcerns.Exceptions;
using Core.Security.Entities;
using Kodlama.Io.Devs.Application.Features.Developers.Constants;
using Kodlama.Io.Devs.Application.Features.Users.Constants;
using Kodlama.Io.Devs.Application.Services.Repositories;

namespace Kodlama.Io.Devs.Application.Features.Developers.Rules
{
    public class DeveloperBusinessRules
    {
        private readonly IDeveloperRepository _developerRepository;
        private readonly DeveloperBusinessRulesMessages _messages;

        public DeveloperBusinessRules(IDeveloperRepository developerRepository, DeveloperBusinessRulesMessages messages)
        {
            _developerRepository = developerRepository;
            _messages = messages;
        }

        public async Task DeveloperShouldBeExistWithGivenUserId(int id)
        {
            User? user = await _developerRepository.GetAsync(u => u.Id == id);
            if (user == null) throw new BusinessException(_messages.IdDoesNotExist);
        }
    }
}
