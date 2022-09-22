using AutoMapper;
using Core.Application.Pipelines.Authorization;
using Core.Security.Entities;
using Core.Security.Extensions;
using Kodlama.Io.Devs.Application.Features.Users.Dtos;
using Kodlama.Io.Devs.Application.Features.Users.Rules;
using Kodlama.Io.Devs.Application.Services.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Kodlama.Io.Devs.Application.Features.Users.Queries.GetByIdUserSelfProfile
{
    public partial class GetByIdUserSelfProfileQuery : IRequest<CommandUserDto>, ISecuredRequest
    {
        public string[] Roles => new[] { "User", "Admin" };

        public class GetByIdUserSelfProfileQueryHandler : IRequestHandler<GetByIdUserSelfProfileQuery, CommandUserDto>
        {
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly IUserRepository _userRepository;
            private readonly IMapper _mapper;
            private readonly UserCustomFunctions _userFunctions;
            private readonly UserBusinessRules _userBusinessRules;

            public GetByIdUserSelfProfileQueryHandler(IHttpContextAccessor httpContextAccessor,
                                                      IUserRepository userRepository,
                                                      IMapper mapper,
                                                      UserCustomFunctions userCustomFunctions,
                                                      UserBusinessRules userBusinessRules)
            {
                _httpContextAccessor = httpContextAccessor;
                _userRepository = userRepository;
                _mapper = mapper;
                _userFunctions = userCustomFunctions;
                _userBusinessRules = userBusinessRules;
            }

            public async Task<CommandUserDto> Handle(GetByIdUserSelfProfileQuery request, CancellationToken cancellationToken)
            {
                int userId = _httpContextAccessor.HttpContext.User.GetUserId();

                await _userBusinessRules.UserShouldBeExistWhenRequested(userId);

                User? user = await _userRepository.GetAsync(u => u.Id == userId);
                CommandUserDto mappedUserDto = _mapper.Map<CommandUserDto>(user);
                _userFunctions.SetCommandUserDtoWhenRequested(userId, ref mappedUserDto);

                return mappedUserDto;
            }
        }
    }
}
