using AutoMapper;
using Core.Application.Pipelines.Authorization;
using Core.Persistence.Paging;
using Kodlama.Io.Devs.Application.Features.Languages.Dtos;
using Kodlama.Io.Devs.Application.Features.Languages.Rules;
using Kodlama.Io.Devs.Application.Services.Repositories;
using Kodlama.Io.Devs.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Kodlama.Io.Devs.Application.Features.Languages.Commands.DeleteLanguage
{
    public partial class DeleteLanguageCommand : IRequest<DeletedLanguageDto>, ISecuredRequest
    {
        public int Id { get; set; }

        public string[] Roles => new[] { "Admin" };

        public class DeleteLanguageCommandHandler : IRequestHandler<DeleteLanguageCommand, DeletedLanguageDto>
        {
            private readonly ILanguageRepository _languageRepository;
            private readonly IMapper _mapper;
            private readonly LanguageBusinessRules _languageBusinessRules;

            public DeleteLanguageCommandHandler(ILanguageRepository languageRepository, IMapper mapper, LanguageBusinessRules languageBusinessRules)
            {
                _languageRepository = languageRepository;
                _mapper = mapper;
                _languageBusinessRules = languageBusinessRules;
            }

            public async Task<DeletedLanguageDto> Handle(DeleteLanguageCommand request, CancellationToken cancellationToken)
            {
                await _languageBusinessRules.LanguageShouldBeExistWhenRequested(request.Id);

                Language? language = await _languageRepository.GetAsync(l => l.Id == request.Id);
                Language? deletedLanguage = await _languageRepository.DeleteAsync(language, include: x => x.Include(g => g.LanguageTechnologies));

                DeletedLanguageDto deletedLanguageDto = _mapper.Map<DeletedLanguageDto>(deletedLanguage);                

                return deletedLanguageDto;
            }
        }
    }
}
