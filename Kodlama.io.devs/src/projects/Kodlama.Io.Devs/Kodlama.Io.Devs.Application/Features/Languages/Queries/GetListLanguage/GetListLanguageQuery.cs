using AutoMapper;
using Core.Persistence.Paging;
using Core.Application.Requests;
using MediatR;
using Kodlama.Io.Devs.Application.Features.Languages.Models;
using Kodlama.Io.Devs.Application.Services.Repositories;
using Kodlama.Io.Devs.Domain.Entities;
using Kodlama.Io.Devs.Application.Features.Languages.Rules;
using Microsoft.EntityFrameworkCore;
using Core.Application.Pipelines.Authorization;

namespace Kodlama.Io.Devs.Application.Features.Languages.Queries.GetListLanguage
{
    public class GetListLanguageQuery : IRequest<LanguageListModel>, ISecuredRequest
    {
        public PageRequest PageRequest { get; set; }

        public string[] Roles => new[] { "Admin", "User" };

        public class GetListLanguageQueryHandler : IRequestHandler<GetListLanguageQuery, LanguageListModel>
        {
            private readonly ILanguageRepository _languageRepository;
            private readonly IMapper _mapper;
            private readonly LanguageBusinessRules _languageBusinessRules;

            public GetListLanguageQueryHandler(ILanguageRepository languageRepository, IMapper mapper, LanguageBusinessRules languageBusinessRules)
            {
                _languageRepository = languageRepository;
                _mapper = mapper;
                _languageBusinessRules = languageBusinessRules;
            }

            public async Task<LanguageListModel> Handle(GetListLanguageQuery request, CancellationToken cancellationToken)
            {
                IPaginate<Language> languages = await _languageRepository.GetListAsync(include: x => x.Include(g => g.LanguageTechnologies), 
                                                                                       index: request.PageRequest.Page, 
                                                                                       size: request.PageRequest.PageSize,
                                                                                       enableTracking: false);

                await _languageBusinessRules.ShouldBeSomeDataInTheLanguageTableWhenRequested(languages);

                LanguageListModel mappedLanguageListModel = _mapper.Map<LanguageListModel>(languages);

                return mappedLanguageListModel;
            }
        }
    }
}
