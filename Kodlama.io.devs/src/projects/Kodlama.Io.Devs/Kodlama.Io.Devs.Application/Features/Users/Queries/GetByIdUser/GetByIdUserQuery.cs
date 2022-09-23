using AutoMapper;
using Core.Security.Entities;
using Kodlama.Io.Devs.Application.Features.Users.Dtos;
using Kodlama.Io.Devs.Application.Features.Users.Rules;
using Kodlama.Io.Devs.Application.Services.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Kodlama.Io.Devs.Application.Features.Users.Queries.GetByIdUser
{
    public class GetByIdUserQuery : IRequest<GetByIdUserDto>
    {
        public int Id { get; set; }
        public class GetByIdUserQueryHandler : IRequestHandler<GetByIdUserQuery, GetByIdUserDto>
        {
            private readonly IUserRepository _userRepository;
            private readonly IMapper _mapper;
            private readonly UserBusinessRules _userBusinessRules;

            public GetByIdUserQueryHandler(IUserRepository userRepository, IMapper mapper, UserBusinessRules userBusinessRules)
            {
                _userRepository = userRepository;
                _mapper = mapper;
                _userBusinessRules = userBusinessRules;
            }
            public async Task<GetByIdUserDto> Handle(GetByIdUserQuery request, CancellationToken cancellationToken)
            {
                await _userBusinessRules.UserShouldBeExistWithGivenUserId(request.Id);

                User? user = await _userRepository.GetAsync(u => u.Id == request.Id,
                                                            include: x => x.Include(u => u.UserOperationClaims)
                                                                           .ThenInclude(o => o.OperationClaim),
                                                            enableTracking: true);

                GetByIdUserDto mappetUserDto = _mapper.Map<GetByIdUserDto>(user);

                return mappetUserDto;
            }
        }
    }
}
