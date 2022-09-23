using AutoMapper;
using Core.Application.Pipelines.Authorization;
using Core.Security.Entities;
using Core.Security.Extensions;
using Kodlama.Io.Devs.Application.Features.Users.Dtos;
using Kodlama.Io.Devs.Application.Features.Users.Rules;
using Kodlama.Io.Devs.Application.Services.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Kodlama.Io.Devs.Application.Features.Users.Queries.GetByIdUserSelfProfile
{
    public class GetByIdUserSelfProfileQuery : IRequest<GetByIdUserDto>, ISecuredRequest
    {
        public string[] Roles => new[] { "Admin", "User" };

        public class GetByIdUserSelfProfileQueryHandler : IRequestHandler<GetByIdUserSelfProfileQuery, GetByIdUserDto>
        {
            private readonly IUserRepository _userRepository;
            private readonly IMapper _mapper;
            private readonly UserBusinessRules _userBusinessRules;
            private readonly IHttpContextAccessor _httpContextAccessor;

            public GetByIdUserSelfProfileQueryHandler(IUserRepository userRepository,
                                                      IMapper mapper,
                                                      UserBusinessRules userBusinessRules,
                                                      IHttpContextAccessor httpContextAccessor)
            {
                _userRepository = userRepository;
                _mapper = mapper;
                _userBusinessRules = userBusinessRules;
                _httpContextAccessor = httpContextAccessor;
            }
            public async Task<GetByIdUserDto> Handle(GetByIdUserSelfProfileQuery request, CancellationToken cancellationToken)
            {
                int userId = _httpContextAccessor.HttpContext.User.GetUserId();

                await _userBusinessRules.UserShouldBeExistWithGivenUserId(userId);

                User? user = await _userRepository.GetAsync(u => u.Id == userId,
                                                            include: x => x.Include(u => u.UserOperationClaims)
                                                                           .ThenInclude(o => o.OperationClaim),
                                                            enableTracking: true);

                GetByIdUserDto mappetUserDto = _mapper.Map<GetByIdUserDto>(user);

                return mappetUserDto;
            }
        }
    }
}