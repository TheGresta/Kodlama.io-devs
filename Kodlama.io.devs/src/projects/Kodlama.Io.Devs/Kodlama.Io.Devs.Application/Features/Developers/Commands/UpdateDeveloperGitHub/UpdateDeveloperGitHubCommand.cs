using AutoMapper;
using Core.Application.Pipelines.Authorization;
using Core.Security.Extensions;
using Kodlama.Io.Devs.Application.Features.Developers.Dtos;
using Kodlama.Io.Devs.Application.Features.Developers.Rules;
using Kodlama.Io.Devs.Application.Services.Repositories;
using Kodlama.Io.Devs.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Kodlama.Io.Devs.Application.Features.Developers.Commands.UpdateDeveloperGitHub
{
    public partial class UpdateDeveloperGitHubCommand : IRequest<DeveloperProfileDto>, ISecuredRequest
    {
        public string GitHub { get; set; }

        public string[] Roles => new[] {"Admin", "Developer"};

        public class UpdateDeveloperGitHubCommandHandler : IRequestHandler<UpdateDeveloperGitHubCommand, DeveloperProfileDto>
        {
            private readonly IDeveloperRepository _developerRepository;
            private readonly IMapper _mapper;
            private readonly DeveloperBusinessRules _developerBusinessRules;
            private readonly IHttpContextAccessor _httpContextAccessor;

            public UpdateDeveloperGitHubCommandHandler(IDeveloperRepository developerRepository, 
                                                       IMapper mapper, 
                                                       DeveloperBusinessRules developerBusinessRules, 
                                                       IHttpContextAccessor httpContextAccessor)
            {
                _developerRepository = developerRepository;
                _mapper = mapper;
                _developerBusinessRules = developerBusinessRules;
                _httpContextAccessor = httpContextAccessor;
            }

            public async Task<DeveloperProfileDto> Handle(UpdateDeveloperGitHubCommand request, CancellationToken cancellationToken)
            {
                int developerId = _httpContextAccessor.HttpContext.User.GetUserId();

                await _developerBusinessRules.DeveloperShouldBeExistWithGivenUserId(developerId);

                Developer developer = await _developerRepository.GetAsync(d => d.Id == developerId);

                _mapper.Map(request, developer);

                Developer updatedDeveloper = await _developerRepository.UpdateAsync(developer);
                DeveloperProfileDto mappedDeveloperDto = _mapper.Map<DeveloperProfileDto>(updatedDeveloper);

                return mappedDeveloperDto;
            }
        }
    }
}
