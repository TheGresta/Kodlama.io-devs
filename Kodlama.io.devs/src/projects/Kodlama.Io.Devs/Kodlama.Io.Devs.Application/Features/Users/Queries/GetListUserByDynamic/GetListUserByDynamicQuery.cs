using AutoMapper;
using Core.Application.Requests;
using Core.Persistence.Dynamic;
using Core.Persistence.Paging;
using Core.Security.Entities;
using Kodlama.Io.Devs.Application.Features.Users.Commands;
using Kodlama.Io.Devs.Application.Features.Users.Models;
using Kodlama.Io.Devs.Application.Features.Users.Queries.GetListUser;
using Kodlama.Io.Devs.Application.Features.Users.Rules;
using Kodlama.Io.Devs.Application.Services.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Kodlama.Io.Devs.Application.Features.Users.Queries.GetListUserByDynamic
{
    public partial class GetListUserByDynamicQuery : IRequest<UserListModel>
    {
        public Dynamic Dynamic { get; set; }
        public PageRequest PageRequest { get; set; }

        public class GetListUserByDynamicQueryHandler : IRequestHandler<GetListUserByDynamicQuery, UserListModel>
        {
            private readonly IUserRepository _userRepository;
            private readonly IMapper _mapper;
            private readonly UserBusinessRules _userBusinessRules;
            private readonly UserCommandCustomFunctions _userCommandCustomFunctions;

            public GetListUserByDynamicQueryHandler(IUserRepository userRepository, IMapper mapper,
                                            UserBusinessRules userBusinessRules,
                                            UserCommandCustomFunctions userCommandCustomFunctions)
            {
                _userRepository = userRepository;
                _mapper = mapper;
                _userBusinessRules = userBusinessRules;
                _userCommandCustomFunctions = userCommandCustomFunctions;
            }

            public async Task<UserListModel> Handle(GetListUserByDynamicQuery request, CancellationToken cancellationToken)
            {
                IPaginate<User>? users = await _userRepository
                                            .GetListByDynamicAsync(request.Dynamic,                                                                     
                                                                   index: request.PageRequest.Page, 
                                                                   size: request.PageRequest.PageSize);

                await _userBusinessRules.ShouldBeSomeDataInTheUserTableWhenRequested(users);

                UserListModel mappedUserListModel = _mapper.Map<UserListModel>(users);

                _userCommandCustomFunctions.SetCommandUserDtoWhenGetListRequested(users, ref mappedUserListModel);
                
                return mappedUserListModel;
            }
        }
    }
}
