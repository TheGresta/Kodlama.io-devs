using Core.CrossCuttingConcerns.Exceptions;
using Core.Persistence.Paging;
using Kodlama.Io.Devs.Application.Features.Languages.Costants;
using Kodlama.Io.Devs.Application.Services.Repositories;
using Kodlama.Io.Devs.Domain.Entities;

namespace Kodlama.Io.Devs.Application.Features.Languages.Rules
{
    public class LanguageBusinessRules
    {
        private readonly ILanguageRepository _languageRepository;
        private readonly LanguageBusinessRulesMessages _messages;

        public LanguageBusinessRules(ILanguageRepository languageRepository, LanguageBusinessRulesMessages messages)
        {
            _languageRepository = languageRepository;
            _messages = messages;
        }

        public async Task LanguageNameCanNotBeDuplicatedWhenInsertedOrUpdated(string name)
        {
            IPaginate<Language> result = await _languageRepository.GetListAsync(l => l.Name == name,  enableTracking:false);
            if (result.Items.Any()) throw new BusinessException(_messages.LanguageNameAlreadyExist);
        }

        public async Task LanguageShouldBeExistWhenRequested(int id)
        {
            Language? language = await _languageRepository.GetAsync(l => l.Id == id);
            if(language == null) throw new BusinessException(_messages.LanguageDoesNotExist);
        }

        public async Task ShouldBeSomeDataInTheLanguageTableWhenRequested(IPaginate<Language> languages)
        {
            if (!languages.Items.Any()) throw new BusinessException(_messages.LanguageDataNotExist);
        }
    }
}
