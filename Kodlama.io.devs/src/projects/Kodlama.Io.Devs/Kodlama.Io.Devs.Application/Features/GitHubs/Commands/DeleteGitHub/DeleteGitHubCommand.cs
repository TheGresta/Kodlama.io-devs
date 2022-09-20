using AutoMapper;
using Kodlama.Io.Devs.Application.Features.GitHubs.Dtos;
using Kodlama.Io.Devs.Application.Features.GitHubs.Rules;
using Kodlama.Io.Devs.Application.Services.Repositories;
using Kodlama.Io.Devs.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Kodlama.Io.Devs.Application.Features.GitHubs.Commands.DeleteGitHub
{
    public partial class DeleteGitHubCommand : IRequest<DeletedGitHubDto>
    {
        public int Id { get; set; }

        public class DeleteGitHubCommandHandler : IRequestHandler<DeleteGitHubCommand, DeletedGitHubDto>
        {
            private readonly IGitHubRepository _gitHubRepository;
            private readonly IMapper _mapper;
            private readonly GitHubBusinessRules _gitHubBusinessRules;

            public DeleteGitHubCommandHandler(IGitHubRepository gitHubRepository, IMapper mapper, GitHubBusinessRules gitHubBusinessRules)
            {
                _gitHubRepository = gitHubRepository;
                _mapper = mapper;
                _gitHubBusinessRules = gitHubBusinessRules;
            }
            public async Task<DeletedGitHubDto> Handle(DeleteGitHubCommand request, CancellationToken cancellationToken)
            {
                await _gitHubBusinessRules.GitHubShouldBeExistWhenRequested(request.Id);

                GitHub? gitHub = await _gitHubRepository.GetAsync(g => g.Id == request.Id);
                GitHub? deletedGitHub = await _gitHubRepository.DeleteAsync(gitHub, include: x => x.Include(g => g.User));
                DeletedGitHubDto mappedGitHubDto = _mapper.Map<DeletedGitHubDto>(deletedGitHub);

                return mappedGitHubDto;
            }
        }
    }
}
