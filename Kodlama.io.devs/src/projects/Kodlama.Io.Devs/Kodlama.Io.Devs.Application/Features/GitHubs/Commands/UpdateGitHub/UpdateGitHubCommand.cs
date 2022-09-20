using AutoMapper;
using Kodlama.Io.Devs.Application.Features.GitHubs.Dtos;
using Kodlama.Io.Devs.Application.Features.GitHubs.Rules;
using Kodlama.Io.Devs.Application.Services.Repositories;
using Kodlama.Io.Devs.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Kodlama.Io.Devs.Application.Features.GitHubs.Commands.UpdateGitHub
{
    public partial class UpdateGitHubCommand : IRequest<UpdatedGitHubDto>
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }

        public class UpdatedGitHubCommandHandler : IRequestHandler<UpdateGitHubCommand, UpdatedGitHubDto>
        {
            private readonly IGitHubRepository _gitHubRepository;
            private readonly IMapper _mapper;
            private readonly GitHubBusinessRules _gitHubBusinessRules;

            public UpdatedGitHubCommandHandler(IGitHubRepository gitHubRepository, IMapper mapper, GitHubBusinessRules gitHubBusinessRules)
            {
                _gitHubRepository = gitHubRepository;
                _mapper = mapper;
                _gitHubBusinessRules = gitHubBusinessRules;
            }

            public async Task<UpdatedGitHubDto> Handle(UpdateGitHubCommand request, CancellationToken cancellationToken)
            {
                await _gitHubBusinessRules.GitHubShouldBeExistWhenRequested(request.Id);
                await _gitHubBusinessRules.UserShouldBeExistWhenGitHubInsertedOrUpdated(request.UserId);
                await _gitHubBusinessRules.GitHubUserNameCanNotBeDuplicatedWhenInsertedOrUpdated(request.Name);
                await _gitHubBusinessRules.AnUserShouldHaveOnlyOneGitHubUserNameWhenGitHubUpdated(request.Id, request.UserId);

                GitHub? gitHub = await _gitHubRepository.GetAsync(g => g.Id == request.Id);
                _mapper.Map(request, gitHub);
                GitHub? updatedGitHub = await _gitHubRepository.UpdateAsync(gitHub, include: x => x.Include(g => g.User));
                UpdatedGitHubDto mappedGitHubDto = _mapper.Map<UpdatedGitHubDto>(updatedGitHub);

                return mappedGitHubDto;
            }
        }
    }
}
