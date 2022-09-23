using AutoMapper;
using Core.Application.Requests;
using Core.Persistence.Paging;
using Core.Security.Entities;
using Kodlama.Io.Devs.Application.Features.Users.Models;
using Kodlama.Io.Devs.Application.Features.Users.Rules;
using Kodlama.Io.Devs.Application.Services.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Kodlama.Io.Devs.Application.Features.Users.Queries.GetListUser
{
    public partial class GetListUserQuery : IRequest<UserListModel>
    {
        public PageRequest PageRequest { get; set; }

        public class GetListUserQueryHandler : IRequestHandler<GetListUserQuery, UserListModel>
        {
            private readonly IUserRepository _userRepository;
            private readonly IMapper _mapper;
            private readonly UserBusinessRules _userBusinessRules;

            public GetListUserQueryHandler(IUserRepository userRepository, IMapper mapper, UserBusinessRules userBusinessRules)
            {
                _userRepository = userRepository;
                _mapper = mapper;
                _userBusinessRules = userBusinessRules;
            }

            public async Task<UserListModel> Handle(GetListUserQuery request, CancellationToken cancellationToken)
            {
                IPaginate<User>? users = await _userRepository.GetListAsync(include: x => x.Include(u => u.UserOperationClaims)
                                                                                           .ThenInclude(o => o.OperationClaim),
                                                                            index: request.PageRequest.Page,
                                                                            size: request.PageRequest.PageSize,
                                                                            enableTracking: true);

                await _userBusinessRules.ThereShouldBeSomeDataInUserListAsRequired(users);

                UserListModel mappedUserDto = _mapper.Map<UserListModel>(users);

                return mappedUserDto;
            }
        }
    }
}
