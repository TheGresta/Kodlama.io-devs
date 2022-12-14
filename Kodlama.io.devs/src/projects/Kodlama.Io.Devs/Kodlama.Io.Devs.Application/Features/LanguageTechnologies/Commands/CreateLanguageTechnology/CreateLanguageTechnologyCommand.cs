using AutoMapper;
using Core.Application.Pipelines.Authorization;
using Kodlama.Io.Devs.Application.Features.LanguageTechnologies.Dtos;
using Kodlama.Io.Devs.Application.Features.LanguageTechnologies.Rules;
using Kodlama.Io.Devs.Application.Services.Repositories;
using Kodlama.Io.Devs.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Kodlama.Io.Devs.Application.Features.LanguageTechnologies.Commands.CreateLanguageTechnology
{
    public partial class CreateLanguageTechnologyCommand : IRequest<CreatedLanguageTechnologyDto>, ISecuredRequest
    {
        public int LanguageId { get; set; }
        public string Name { get; set; }

        public string[] Roles => new[] { "Admin" };
    }

    public class CreateLanguageTechnologyCommandHandler : IRequestHandler<CreateLanguageTechnologyCommand, CreatedLanguageTechnologyDto>
    {
        private readonly ILanguageTechnologyRepository _languageTechnologyRepository;
        private readonly IMapper _mapper;
        private readonly LanguageTechnologyBusinessRules _languageTechnologyBusinessRules;

        public CreateLanguageTechnologyCommandHandler(ILanguageTechnologyRepository languageTechnologyRepository, 
                                                        IMapper mapper, 
                                                        LanguageTechnologyBusinessRules languageTechnologyBusinessRules)
        {
            _languageTechnologyRepository = languageTechnologyRepository;
            _mapper = mapper;
            _languageTechnologyBusinessRules = languageTechnologyBusinessRules;
        }

        public async Task<CreatedLanguageTechnologyDto> Handle(CreateLanguageTechnologyCommand request, CancellationToken cancellationToken)
        {
            await _languageTechnologyBusinessRules.LanguageShouldBeExistWhenRequested(request.LanguageId);
            await _languageTechnologyBusinessRules.LanguageTechnologyNameCanNotBeDuplicatedWhenInserted(request.Name);

            LanguageTechnology languageTechnology = _mapper.Map<LanguageTechnology>(request);
            LanguageTechnology? addedLanguageTechnology = await _languageTechnologyRepository.AddAsync(languageTechnology, include: x => x.Include(l => l.Language));
            CreatedLanguageTechnologyDto mappedLanguageTechnology = _mapper.Map<CreatedLanguageTechnologyDto>(addedLanguageTechnology);

            return mappedLanguageTechnology;
        }
    }
}
