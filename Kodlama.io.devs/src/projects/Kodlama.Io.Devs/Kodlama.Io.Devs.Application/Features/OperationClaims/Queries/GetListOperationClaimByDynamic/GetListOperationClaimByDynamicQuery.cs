using AutoMapper;
using Core.Application.Requests;
using Core.Persistence.Dynamic;
using Core.Persistence.Paging;
using Core.Security.Entities;
using Kodlama.Io.Devs.Application.Features.OperationClaims.Models;
using Kodlama.Io.Devs.Application.Features.OperationClaims.Rules;
using Kodlama.Io.Devs.Application.Services.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Kodlama.Io.Devs.Application.Features.OperationClaims.Queries.GetListOperationClaimByDynamic
{
    public class GetListOperationClaimByDynamicQuery : IRequest<OperationClaimListModel>
    {
        public Dynamic Dynamic { get; set; }
        public PageRequest PageRequest { get; set; }

        public  class GetListOperationClaimByDynamicQueryHandler : IRequestHandler<GetListOperationClaimByDynamicQuery, OperationClaimListModel>
        {
            private readonly IMapper _mapper;
            private readonly IOperationClaimRepository _operationClaimRepository;
            private readonly OperationClaimBusinessRules _operationClaimBusinessRules;

            public GetListOperationClaimByDynamicQueryHandler(IMapper mapper, IOperationClaimRepository operationClaimRepository, OperationClaimBusinessRules operationClaimBusinessRules)
            {
                _mapper = mapper;
                _operationClaimRepository = operationClaimRepository;
                _operationClaimBusinessRules = operationClaimBusinessRules;
            }

            public async Task<OperationClaimListModel> Handle(GetListOperationClaimByDynamicQuery request, CancellationToken cancellationToken)
            {
                IPaginate<OperationClaim> operationClaims = await _operationClaimRepository.GetListByDynamicAsync(
                    request.Dynamic, index: request.PageRequest.Page, size: request.PageRequest.PageSize);

                await _operationClaimBusinessRules.ShouldBeSomeDataInTheOperationClaimTableWhenRequested(operationClaims);

                OperationClaimListModel mappedOperationClaimModels = _mapper.Map<OperationClaimListModel>(operationClaims);
                return mappedOperationClaimModels;
            }
        }
    }
}
