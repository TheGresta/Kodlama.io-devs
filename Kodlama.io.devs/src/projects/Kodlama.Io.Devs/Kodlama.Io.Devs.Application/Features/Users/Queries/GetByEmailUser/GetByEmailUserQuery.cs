using AutoMapper;
using Core.Security.Entities;
using Kodlama.Io.Devs.Application.Features.Users.Dtos;
using Kodlama.Io.Devs.Application.Features.Users.Rules;
using Kodlama.Io.Devs.Application.Services.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Kodlama.Io.Devs.Application.Features.Users.Queries.GetByEmailUser
{
    public partial class GetByEmailUserQuery : IRequest<GetByEmailUserDto>
    {
        public string Email { get; set; }
        public class GetByEmailUserQueryHandler : IRequestHandler<GetByEmailUserQuery, GetByEmailUserDto>
        {
            private readonly IUserRepository _userRepository;
            private readonly IMapper _mapper;
            private readonly UserBusinessRules _userBusinessRules;

            public GetByEmailUserQueryHandler(IUserRepository userRepository, IMapper mapper, UserBusinessRules userBusinessRules)
            {
                _userRepository = userRepository;
                _mapper = mapper;
                _userBusinessRules = userBusinessRules;
            }
            public async Task<GetByEmailUserDto> Handle(GetByEmailUserQuery request, CancellationToken cancellationToken)
            {
                await _userBusinessRules.UserShouldBeExistWithGivenEmailAddress(request.Email);

                User? user = await _userRepository.GetAsync(u => u.Email.ToLower() == request.Email.ToLower(),
                                                            include: x => x.Include(u => u.UserOperationClaims)
                                                                           .ThenInclude(o => o.OperationClaim),
                                                            enableTracking: true);

                GetByEmailUserDto mappetUserDto = _mapper.Map<GetByEmailUserDto>(user);

                return mappetUserDto;
            }
        }
    }
}
