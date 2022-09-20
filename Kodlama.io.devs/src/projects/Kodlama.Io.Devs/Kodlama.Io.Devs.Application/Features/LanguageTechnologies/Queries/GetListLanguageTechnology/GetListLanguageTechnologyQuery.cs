using AutoMapper;
using Core.Application.Requests;
using Core.Persistence.Paging;
using Kodlama.Io.Devs.Application.Features.LanguageTechnologies.Models;
using Kodlama.Io.Devs.Application.Features.LanguageTechnologies.Rules;
using Kodlama.Io.Devs.Application.Services.Repositories;
using Kodlama.Io.Devs.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Kodlama.Io.Devs.Application.Features.LanguageTechnologies.Queries.GetListLanguageTechnology
{
    public partial class GetListLanguageTechnologyQuery : IRequest<LanguageTechnologyListModel>
    {
        public PageRequest PageRequest { get; set; }

        public class GetListLanguageTechnologyQueryHandler : IRequestHandler<GetListLanguageTechnologyQuery, LanguageTechnologyListModel>
        {
            private readonly ILanguageTechnologyRepository _languageTechnologyRepository;
            private readonly IMapper _mapper;
            private readonly LanguageTechnologyBusinessRules _languageTechnologyBusinessRules;

            public GetListLanguageTechnologyQueryHandler(ILanguageTechnologyRepository languageTechnologyRepository, 
                                                            IMapper mapper, 
                                                            LanguageTechnologyBusinessRules languageTechnologyBusinessRules)
            {
                _languageTechnologyRepository = languageTechnologyRepository;
                _mapper = mapper;
                _languageTechnologyBusinessRules = languageTechnologyBusinessRules;
            }

            public async Task<LanguageTechnologyListModel> Handle(GetListLanguageTechnologyQuery request, CancellationToken cancellationToken)
            {
                IPaginate<LanguageTechnology> languageTechnologies = 
                    await _languageTechnologyRepository.GetListAsync(include: x => x.Include(l => l.Language), 
                                                                     index: request.PageRequest.Page, 
                                                                     size: request.PageRequest.PageSize,
                                                                     enableTracking: false);

                await _languageTechnologyBusinessRules.LanguageTechnologyDataShouldBeExistWhenRequested(languageTechnologies);

                LanguageTechnologyListModel mappedLanguageTechnologyListModel = _mapper.Map<LanguageTechnologyListModel>(languageTechnologies);

                return mappedLanguageTechnologyListModel;
            }
        }
    }
}
