using AutoMapper;
using Core.Application.Requests;
using Core.Persistence.Paging;
using Core.Security.Entities;
using Kodlama.Io.Devs.Application.Features.OperationClaims.Models;
using Kodlama.Io.Devs.Application.Features.UserOperationClaims.Models;
using Kodlama.Io.Devs.Application.Services.Repositories;
using MediatR;

namespace Kodlama.Io.Devs.Application.Features.UserOperationClaims.Queries.GetListUserOperationClaim
{
    public partial class GetListUserOperationClaimQuery : IRequest<UserOperationClaimListModel>
    {
        public PageRequest PageRequest { get; set; }

        public class GetListUserOperationClaimQueryHandler : IRequestHandler<GetListUserOperationClaimQuery, UserOperationClaimListModel>
        {
            private readonly IUserOperationClaimRepository _userOperationClaimRepository;
            private readonly IMapper _mapper;
            private readonly UserOperationClaimBusinessRules _userOperationClaimBusinessRules;

            public GetListUserOperationClaimQueryHandler(IUserOperationClaimRepository userOperationClaimRepository, UserOperationClaimBusinessRules userOperationClaimBusinessRules, IMapper mapper)
            {
                _userOperationClaimRepository = userOperationClaimRepository;
                _mapper = mapper;
                _userOperationClaimBusinessRules = userOperationClaimBusinessRules;
            }

            public async Task<UserOperationClaimListModel> Handle(GetListUserOperationClaimQuery request, CancellationToken cancellationToken)
            {
                IPaginate<UserOperationClaim>? userOperationClaims = 
                    await _userOperationClaimRepository.GetListAsync(index: request.PageRequest.Page, size: request.PageRequest.PageSize);

                UserOperationClaimListModel mappedUserOperationClaimListModel = _mapper.Map<UserOperationClaimListModel>(userOperationClaims);
                return mappedUserOperationClaimListModel;
            }
        }
    }
}
