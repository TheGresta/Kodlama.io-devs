using AutoMapper;
using Core.Application.Requests;
using Core.Persistence.Paging;
using Core.Security.Entities;
using Kodlama.Io.Devs.Application.Features.Users.Commands;
using Kodlama.Io.Devs.Application.Features.Users.Models;
using Kodlama.Io.Devs.Application.Features.Users.Rules;
using Kodlama.Io.Devs.Application.Services.Repositories;
using MediatR;

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
            private readonly UserCommandCustomFunctions _userCommandCustomFunctions;

            public GetListUserQueryHandler(IUserRepository userRepository, IMapper mapper, 
                                            UserBusinessRules userBusinessRules, 
                                            UserCommandCustomFunctions userCommandCustomFunctions)
            {
                _userRepository = userRepository;
                _mapper = mapper;
                _userBusinessRules = userBusinessRules;
                _userCommandCustomFunctions = userCommandCustomFunctions;
            }

            public async Task<UserListModel> Handle(GetListUserQuery request, CancellationToken cancellationToken)
            {
                IPaginate<User>? users = await _userRepository.GetListAsync(index: request.PageRequest.Page, size: request.PageRequest.PageSize);

                await _userBusinessRules.ShouldBeSomeDataInTheUserTableWhenRequested(users);

                UserListModel mappedUserListModel = new();

                _userCommandCustomFunctions.SetCommandUserDtoWhenGetListRequested(users, ref mappedUserListModel);

                mappedUserListModel = _mapper.Map<UserListModel>(users);
                return mappedUserListModel;
            }
        }
    }
}
