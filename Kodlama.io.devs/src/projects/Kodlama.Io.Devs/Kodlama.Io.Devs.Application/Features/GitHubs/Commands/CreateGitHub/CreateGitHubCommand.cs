﻿using AutoMapper;
using Kodlama.Io.Devs.Application.Features.GitHubs.Dtos;
using Kodlama.Io.Devs.Application.Features.GitHubs.Rules;
using Kodlama.Io.Devs.Application.Services.Repositories;
using Kodlama.Io.Devs.Domain.Entities;
using MediatR;

namespace Kodlama.Io.Devs.Application.Features.GitHubs.Commands.CreateGitHub
{
    public partial class UpdatedGitHubCommand : IRequest<CreatedGitHubDto>
    {
        public string Name { get; set; }
        public int UserId { get; set; }

        public class CreateGitHubCommandHandler : IRequestHandler<UpdatedGitHubCommand, CreatedGitHubDto>
        {
            private readonly IGitHubRepository _gitHubRepository;
            private readonly IMapper _mapper;
            private readonly GitHubBusinessRules _gitHubBusinessRules;

            public CreateGitHubCommandHandler(IGitHubRepository gitHubRepository, IMapper mapper, GitHubBusinessRules gitHubBusinessRules)
            {
                _gitHubRepository = gitHubRepository;
                _mapper = mapper;
                _gitHubBusinessRules = gitHubBusinessRules;
            }

            public async Task<CreatedGitHubDto> Handle(UpdatedGitHubCommand request, CancellationToken cancellationToken)
            {
                await _gitHubBusinessRules.UserShouldBeExistWhenGitHubInsertedOrUpdated(request.UserId);
                await _gitHubBusinessRules.GitHubUserNameCanNotBeDuplicatedWhenInsertedOrUpdated(request.Name);

                GitHub gitHub = _mapper.Map<GitHub>(request);
                GitHub createdGitHub = await _gitHubRepository.AddAsync(gitHub);
                CreatedGitHubDto mappedGitHubDto = _mapper.Map<CreatedGitHubDto>(createdGitHub);

                return mappedGitHubDto;
            }
        }
    }
}
