using AutoMapper;
using Kodlama.Io.Devs.Application.Features.LanguageTechnologies.Dtos;
using Kodlama.Io.Devs.Application.Features.LanguageTechnologies.Rules;
using Kodlama.Io.Devs.Application.Services.Repositories;
using Kodlama.Io.Devs.Domain.Entities;
using MediatR;

namespace Kodlama.Io.Devs.Application.Features.LanguageTechnologies.Queries.GetByIdLanguageTechnology
{
    public partial class GetByIdLanguageTechnologyQuery : IRequest<GetByIdLanguageTechnologyDto>
    {
        public int Id { get; set; }
        public class GetByIdLanguageTechnologyQueryHandler : IRequestHandler<GetByIdLanguageTechnologyQuery, GetByIdLanguageTechnologyDto>
        {
            private readonly ILanguageTechnologyRepository _languageTechnologyRepository;
            private readonly IMapper _mapper;
            private readonly LanguageTechnologyBusinessRules _languageTechnologyBusinessRules;

            public GetByIdLanguageTechnologyQueryHandler(ILanguageTechnologyRepository languageTechnologyRepository, 
                                                            IMapper mapper, 
                                                            LanguageTechnologyBusinessRules languageTechnologyBusinessRules)
            {
                _languageTechnologyRepository = languageTechnologyRepository;
                _mapper = mapper;
                _languageTechnologyBusinessRules = languageTechnologyBusinessRules;
            }

            public async Task<GetByIdLanguageTechnologyDto> Handle(GetByIdLanguageTechnologyQuery request, CancellationToken cancellationToken)
            {
                await _languageTechnologyBusinessRules.LanguageTechnologyShouldBeExistWhenRequested(request.Id);

                LanguageTechnology? languageTechnology = await _languageTechnologyRepository.GetAsync(l => l.Id == request.Id);
                GetByIdLanguageTechnologyDto mappedLanguageTechnology = _mapper.Map<GetByIdLanguageTechnologyDto>(languageTechnology);

                return mappedLanguageTechnology;
            }
        }
    }
}
