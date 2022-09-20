using AutoMapper;
using Kodlama.Io.Devs.Application.Features.GitHubs.Dtos;
using Kodlama.Io.Devs.Application.Features.GitHubs.Rules;
using Kodlama.Io.Devs.Application.Services.Repositories;
using Kodlama.Io.Devs.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Kodlama.Io.Devs.Application.Features.GitHubs.Queries.GetByIdGitHub
{
    public partial class GetByIdGitHubQuery : IRequest<GetByIdGitHubDto>
    {
        public int Id { get; set; }
        public class GetByIdGitHubQueryHandler : IRequestHandler<GetByIdGitHubQuery, GetByIdGitHubDto>
        {
            private readonly IGitHubRepository _gitHubRepository;
            private readonly IMapper _mapper;
            private readonly GitHubBusinessRules _gitHubBusinessRules;

            public GetByIdGitHubQueryHandler(IGitHubRepository gitHubRepository, IMapper mapper, GitHubBusinessRules gitHubBusinessRules)
            {
                _gitHubRepository = gitHubRepository;
                _mapper = mapper;
                _gitHubBusinessRules = gitHubBusinessRules;
            }

            public async Task<GetByIdGitHubDto> Handle(GetByIdGitHubQuery request, CancellationToken cancellationToken)
            {
                await _gitHubBusinessRules.GitHubShouldBeExistWhenRequested(request.Id);

                GitHub? gitHub = await _gitHubRepository.GetAsync(l => l.Id == request.Id,
                                                               include: x => x.Include(g => g.User),
                                                               enableTracking: false);
                GetByIdGitHubDto mappedGitHubDto = _mapper.Map<GetByIdGitHubDto>(gitHub);

                return mappedGitHubDto;
            }
        }
    }
}
