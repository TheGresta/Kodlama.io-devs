using AutoMapper;
using Core.Application.Requests;
using Core.Persistence.Dynamic;
using Core.Persistence.Paging;
using Kodlama.Io.Devs.Application.Features.GitHubs.Models;
using Kodlama.Io.Devs.Application.Features.GitHubs.Rules;
using Kodlama.Io.Devs.Application.Services.Repositories;
using Kodlama.Io.Devs.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Kodlama.Io.Devs.Application.Features.GitHubs.Queries.GetListGitHubByDynamic
{
    public partial class GetListGitHubByDynamicQuery : IRequest<GitHubListModel>
    {
        public PageRequest PageRequest { get; set; }
        public Dynamic  Dynamic { get; set; }
        public class GetListGitHubByDynamicQueryHandler : IRequestHandler<GetListGitHubByDynamicQuery, GitHubListModel>
        {
            private readonly IGitHubRepository _gitHubRepository;
            private readonly IMapper _mapper;
            private readonly GitHubBusinessRules _gitHubBusinessRules;

            public GetListGitHubByDynamicQueryHandler(IGitHubRepository gitHubRepository, IMapper mapper, GitHubBusinessRules gitHubBusinessRules)
            {
                _gitHubRepository = gitHubRepository;
                _mapper = mapper;
                _gitHubBusinessRules = gitHubBusinessRules;
            }

            public async Task<GitHubListModel> Handle(GetListGitHubByDynamicQuery request, CancellationToken cancellationToken)
            {
                IPaginate<GitHub> gitHubs = await _gitHubRepository.GetListByDynamicAsync(request.Dynamic, 
                                                                                          include: x => x.Include(g => g.User), 
                                                                                          index: request.PageRequest.Page, 
                                                                                          size: request.PageRequest.PageSize,
                                                                                          enableTracking: false);

                await _gitHubBusinessRules.ShouldBeSomeDataInTheGitHubTableWhenRequested(gitHubs);

                GitHubListModel mappedGitHubListModel = _mapper.Map<GitHubListModel>(gitHubs);

                return mappedGitHubListModel;
            }
        }
    }
}
