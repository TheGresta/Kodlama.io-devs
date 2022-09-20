using AutoMapper;
using Core.Persistence.Paging;
using Kodlama.Io.Devs.Application.Features.Languages.Dtos;
using Kodlama.Io.Devs.Application.Features.Languages.Rules;
using Kodlama.Io.Devs.Application.Services.Repositories;
using Kodlama.Io.Devs.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Kodlama.Io.Devs.Application.Features.Languages.Commands.DeleteLanguage
{
    public partial class DeleteLanguageCommand : IRequest<DeletedLanguageDto>
    {
        public int Id { get; set; }

        public class DeleteLanguageCommandHandler : IRequestHandler<DeleteLanguageCommand, DeletedLanguageDto>
        {
            private readonly ILanguageRepository _languageRepository;
            private readonly IMapper _mapper;
            private readonly LanguageBusinessRules _languageBusinessRules;
            private readonly ILanguageTechnologyRepository _languageTechnologyRepository;

            public DeleteLanguageCommandHandler(ILanguageRepository languageRepository, IMapper mapper, LanguageBusinessRules languageBusinessRules, ILanguageTechnologyRepository languageTechnologyRepository)
            {
                _languageRepository = languageRepository;
                _mapper = mapper;
                _languageBusinessRules = languageBusinessRules;
                _languageTechnologyRepository = languageTechnologyRepository;
            }

            public async Task<DeletedLanguageDto> Handle(DeleteLanguageCommand request, CancellationToken cancellationToken)
            {
                await _languageBusinessRules.LanguageShouldBeExistWhenRequested(request.Id);

                Language? language = await _languageRepository.GetAsync(l => l.Id == request.Id);
                Language? deletedLanguage = await _languageRepository.DeleteAsync(language, include: x => x.Include(g => g.LanguageTechnologies));

                if (deletedLanguage != null)
                    await this.DeleteAllLanguageTechnologiesWithGivenLanguageId(deletedLanguage.Id);

                DeletedLanguageDto deletedLanguageDto = _mapper.Map<DeletedLanguageDto>(deletedLanguage);                

                return deletedLanguageDto;
            }

            private async Task DeleteAllLanguageTechnologiesWithGivenLanguageId(int id)
            {
                IPaginate<LanguageTechnology> languageTechnologies = await _languageTechnologyRepository.GetListAsync(l => l.LanguageId == id);

                foreach(LanguageTechnology languageTechnology in languageTechnologies.Items)
                {
                    await _languageTechnologyRepository.DeleteAsync(languageTechnology);
                }
            }
        }
    }
}
