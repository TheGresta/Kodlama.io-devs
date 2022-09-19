using Core.CrossCuttingConcerns.Exceptions;
using Core.Persistence.Paging;
using Kodlama.Io.Devs.Application.Services.Repositories;
using Kodlama.Io.Devs.Domain.Entities;

namespace Kodlama.Io.Devs.Application.Features.LanguageTechnologies.Rules
{
    public class LanguageTechnologyBusinessRules
    {
        private readonly ILanguageTechnologyRepository _languageTechnologyRepository;
        private readonly ILanguageRepository _languageRepository;
        private readonly LanguageTechnologyBusinessRulesMessages _messages;

        public LanguageTechnologyBusinessRules(ILanguageTechnologyRepository languageTechnologyRepository, 
                                                ILanguageRepository languageRepository, 
                                                LanguageTechnologyBusinessRulesMessages messages)
        {
            _languageTechnologyRepository = languageTechnologyRepository;
            _languageRepository = languageRepository;
            _messages = messages;
        }

        public async Task LanguageTechnologyNameCanNotBeDuplicatedWhenInserted(string name)
        {
            LanguageTechnology? languageTechnology = _languageTechnologyRepository.Get(l => l.Name == name);
            if (languageTechnology != null) throw new BusinessException(_messages.LanguageTechnologyNameAlreadyTaken);
        }

        public async Task LanguageTechnologyNameCanNotBeDuplicatedWhenUpdated(int id, string name)
        {
            LanguageTechnology? languageTechnology = _languageTechnologyRepository.Get(l => l.Name == name && l.Id != id);
            if (languageTechnology != null) throw new BusinessException(_messages.LanguageTechnologyNameAlreadyTaken);
        }
        
        public async Task LanguageShouldBeExistWhenRequested(int id)
        {
            Language? language = _languageRepository.Get(l => l.Id == id);
            if (language == null) throw new BusinessException(_messages.LanguageDoesNotExist);
        }

        public async Task LanguageTechnologyShouldBeExistWhenRequested(int id)
        {
            LanguageTechnology? languageTechnology = _languageTechnologyRepository.Get(l => l.Id != id);
            if (languageTechnology == null) throw new BusinessException(_messages.LanguageTechnologyDoesNotExist);
        }

        public async Task LanguageTechnologyDataShouldBeExistWhenRequested(IPaginate<LanguageTechnology> languageTechnologies)
        {
            if(!languageTechnologies.Items.Any()) throw new BusinessException(_messages.LanguageTechnologyDataDoesNotExist);
        }
    }
}
