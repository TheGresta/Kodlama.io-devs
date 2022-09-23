using AutoMapper;
using Kodlama.Io.Devs.Application.Features.OperationClaims.Dtos;
using Kodlama.Io.Devs.Application.Services.Repositories;
using MediatR;
using Core.Security.Entities;
using Kodlama.Io.Devs.Application.Features.OperationClaims.Rules;
using Core.Application.Pipelines.Authorization;

namespace Kodlama.Io.Devs.Application.Features.OperationClaims.Commands.CreateOperationClaim
{
    public partial class CreateOperationClaimCommand : IRequest<CreatedOperationClaimDto>, ISecuredRequest
    {
        public string Name { get; set; }

        public string[] Roles => new[] { "Admin" };

        public class CreateOperationClaimCommandHandler : IRequestHandler<CreateOperationClaimCommand, CreatedOperationClaimDto>
        {
            private readonly IOperationClaimRepository _operationClaimRepository;
            private readonly IMapper _mapper;
            private readonly OperationClaimBusinessRules _operationClaimBusinessRules;

            public CreateOperationClaimCommandHandler(IOperationClaimRepository operationClaimRepository, IMapper mapper, OperationClaimBusinessRules operationClaimBusinessRules)
            {
                _operationClaimRepository = operationClaimRepository;
                _mapper = mapper;
                _operationClaimBusinessRules = operationClaimBusinessRules;
            }

            public async Task<CreatedOperationClaimDto> Handle(CreateOperationClaimCommand request, CancellationToken cancellationToken)
            {
                await _operationClaimBusinessRules.OperationClaimNameCanNotBeDuplicatedWhenInsertedOrUpdated(request.Name);

               OperationClaim mappedOperationClaim = _mapper.Map<OperationClaim>(request);
               OperationClaim createdOperationClaim = await _operationClaimRepository.AddAsync(mappedOperationClaim);
               CreatedOperationClaimDto createdOperationClaimDto = _mapper.Map<CreatedOperationClaimDto>(createdOperationClaim);

               return createdOperationClaimDto;
            }
        }
    }
}
