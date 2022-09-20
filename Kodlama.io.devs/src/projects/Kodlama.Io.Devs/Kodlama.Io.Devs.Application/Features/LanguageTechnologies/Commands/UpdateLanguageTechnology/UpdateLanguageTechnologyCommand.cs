using AutoMapper;
using Kodlama.Io.Devs.Application.Features.LanguageTechnologies.Dtos;
using Kodlama.Io.Devs.Application.Features.LanguageTechnologies.Rules;
using Kodlama.Io.Devs.Application.Services.Repositories;
using Kodlama.Io.Devs.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Kodlama.Io.Devs.Application.Features.LanguageTechnologies.Commands.UpdateLanguageTechnology
{
    public partial class UpdateLanguageTechnologyCommand : IRequest<UpdatedLanguageTechnologyDto>
    {
        public int Id { get; set; }
        public int LanguageId { get; set; }
        public string Name { get; set; }

        public class UpdateLanguageTechnologyCommandHandler : IRequestHandler<UpdateLanguageTechnologyCommand, UpdatedLanguageTechnologyDto>
        {
            private readonly ILanguageTechnologyRepository _languageTechnologyRepository;
            private readonly IMapper _mapper;
            private readonly LanguageTechnologyBusinessRules _languageTechnologyBusinessRules;

            public UpdateLanguageTechnologyCommandHandler(ILanguageTechnologyRepository languageTechnologyRepository, 
                                                            IMapper mapper, 
                                                            LanguageTechnologyBusinessRules languageTechnologyBusinessRules)
            {
                _languageTechnologyRepository = languageTechnologyRepository;
                _mapper = mapper;
                _languageTechnologyBusinessRules = languageTechnologyBusinessRules;
            }

            public async Task<UpdatedLanguageTechnologyDto> Handle(UpdateLanguageTechnologyCommand request, CancellationToken cancellationToken)
            {
                await _languageTechnologyBusinessRules.LanguageTechnologyShouldBeExistWhenRequested(request.Id);
                await _languageTechnologyBusinessRules.LanguageShouldBeExistWhenRequested(request.LanguageId);
                await _languageTechnologyBusinessRules.LanguageTechnologyNameCanNotBeDuplicatedWhenUpdated(request.Id, request.Name);

                LanguageTechnology languageTechnology = await _languageTechnologyRepository.GetAsync(l => l.Id == request.Id);
                _mapper.Map(request, languageTechnology);

                LanguageTechnology? updatedLanguageTechnology = await _languageTechnologyRepository.UpdateAsync(languageTechnology);

                updatedLanguageTechnology = await _languageTechnologyRepository.GetAsync(l => l.Id == updatedLanguageTechnology.Id,
                                                                                        include: x => x.Include(l => l.Language),
                                                                                        enableTracking: false);

                UpdatedLanguageTechnologyDto mappedLanguageTechnology = _mapper.Map<UpdatedLanguageTechnologyDto>(updatedLanguageTechnology);

                return mappedLanguageTechnology;
            }
        }
    }
}
