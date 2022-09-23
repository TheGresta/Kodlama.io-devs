using AutoMapper;
using Core.Security.Extensions;
using Kodlama.Io.Devs.Application.Features.Developers.Dtos;
using Kodlama.Io.Devs.Application.Features.Developers.Rules;
using Kodlama.Io.Devs.Application.Services.Repositories;
using Kodlama.Io.Devs.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Kodlama.Io.Devs.Application.Features.Developers.Queries.GetByIdDeveloperSelfProfile
{
    public partial class GetByIdDeveloperSelfProfileQuery : IRequest<DeveloperProfileDto>
    {
        public class GetByIdDeveloperSelfProfileQueryHandler : IRequestHandler<GetByIdDeveloperSelfProfileQuery, DeveloperProfileDto>
        {
            private readonly IDeveloperRepository _developerRepository;
            private readonly IMapper _mapper;
            private readonly DeveloperBusinessRules _developerBusinessRules;
            private readonly IHttpContextAccessor _httpContextAccessor;

            public GetByIdDeveloperSelfProfileQueryHandler(IDeveloperRepository developerRepository, 
                                                           IMapper mapper, 
                                                           DeveloperBusinessRules developerBusinessRules, 
                                                           IHttpContextAccessor httpContextAccessor)
            {
                _developerRepository = developerRepository;
                _mapper = mapper;
                _developerBusinessRules = developerBusinessRules;
                _httpContextAccessor = httpContextAccessor;
            }

            public async Task<DeveloperProfileDto> Handle(GetByIdDeveloperSelfProfileQuery request, CancellationToken cancellationToken)
            {
                int developerId = _httpContextAccessor.HttpContext.User.GetUserId();

                await _developerBusinessRules.DeveloperShouldBeExistWithGivenUserId(developerId);

                Developer? developer = await _developerRepository.GetAsync(u => u.Id == developerId);

                DeveloperProfileDto mappetUserDto = _mapper.Map<DeveloperProfileDto>(developer);

                return mappetUserDto;
            }
        }
    }
}
