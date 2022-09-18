using AutoMapper;
using Core.Application.Requests;
using Core.Persistence.Dynamic;
using Core.Persistence.Paging;
using Kodlama.Io.Devs.Application.Features.Languages.Models;
using Kodlama.Io.Devs.Application.Features.Languages.Rules;
using Kodlama.Io.Devs.Application.Services.Repositories;
using Kodlama.Io.Devs.Domain.Entities;
using MediatR;

namespace Kodlama.Io.Devs.Application.Features.Languages.Queries.GetListLanguageByDynamic
{
    public class GetListLanguageByDynamicQuery : IRequest<LanguageListModel>
    {
        public Dynamic Dynamic { get; set; }
        public PageRequest PageRequest { get; set; }

        public class GetListLanguageByDynamicQueryHandler : IRequestHandler<GetListLanguageByDynamicQuery, LanguageListModel>
        {
            private readonly IMapper _mapper;
            private readonly ILanguageRepository _languageRepository;
            private readonly LanguageBusinessRules _languageBusinessRules;

            public GetListLanguageByDynamicQueryHandler(IMapper mapper, ILanguageRepository languageRepository, LanguageBusinessRules languageBusinessRules)
            {
                _mapper = mapper;
                _languageRepository = languageRepository;
                _languageBusinessRules = languageBusinessRules;
            }

            public async Task<LanguageListModel> Handle(GetListLanguageByDynamicQuery request, CancellationToken cancellationToken)
            {
                IPaginate<Language> languages = await _languageRepository.GetListByDynamicAsync(
                    request.Dynamic, index: request.PageRequest.Page, size: request.PageRequest.PageSize);

                await _languageBusinessRules.ShouldBeSomeDataInTheLanguageTableWhenRequested(languages);

                LanguageListModel mappedLanguageListModels = _mapper.Map<LanguageListModel>(languages);
                return mappedLanguageListModels;
            }
        }
    }
}
