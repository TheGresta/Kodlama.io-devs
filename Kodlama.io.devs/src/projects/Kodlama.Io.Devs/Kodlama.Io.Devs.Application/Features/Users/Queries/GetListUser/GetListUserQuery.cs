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
            private readonly UserCustomFunctions _userCustomFunctions;

            public GetListUserQueryHandler(IUserRepository userRepository, IMapper mapper, 
                                            UserBusinessRules userBusinessRules, 
                                            UserCustomFunctions userCommandCustomFunctions)
            {
                _userRepository = userRepository;
                _mapper = mapper;
                _userBusinessRules = userBusinessRules;
                _userCustomFunctions = userCommandCustomFunctions;
            }

            public async Task<UserListModel> Handle(GetListUserQuery request, CancellationToken cancellationToken)
            {
                IPaginate<User>? users = await _userRepository.GetListAsync(index: request.PageRequest.Page, 
                                                                            size: request.PageRequest.PageSize);

                await _userBusinessRules.ShouldBeSomeDataInTheUserTableWhenRequested(users);

                UserListModel mappedUserListModel = _mapper.Map<UserListModel>(users);

                _userCustomFunctions.SetCommandUserDtoWhenGetListRequested(users, ref mappedUserListModel);

                return mappedUserListModel;
            }
        }
    }
}
