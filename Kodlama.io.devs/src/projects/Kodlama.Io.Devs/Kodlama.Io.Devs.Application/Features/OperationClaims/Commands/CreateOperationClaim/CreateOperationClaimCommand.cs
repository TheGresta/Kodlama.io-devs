using AutoMapper;
using Kodlama.Io.Devs.Application.Features.OperationClaims.Dtos;
using Kodlama.Io.Devs.Application.Services.Repositories;
using MediatR;
using Core.Security.Entities;

namespace Kodlama.Io.Devs.Application.Features.OperationClaims.Commands.CreateOperationClaim
{
    public partial class CreateOperationClaimCommand : IRequest<CreatedOperationClaimDto>
    {
        public string Name { get; set; }

        public class CreateOperationClaimCommandHandler : IRequestHandler<CreateOperationClaimCommand, CreatedOperationClaimDto>
        {
            private readonly IOperationClaimRepository _operationClaimRepository;
            private readonly IMapper _mapper;

            public CreateOperationClaimCommandHandler(IOperationClaimRepository operationClaimRepository, IMapper mapper)
            {
                _operationClaimRepository = operationClaimRepository;
                _mapper = mapper;
            }

            public async Task<CreatedOperationClaimDto> Handle(CreateOperationClaimCommand request, CancellationToken cancellationToken)
            {

               OperationClaim mappedOperationClaim = _mapper.Map<OperationClaim>(request);
               OperationClaim createdOperationClaim = await _operationClaimRepository.AddAsync(mappedOperationClaim);
               CreatedOperationClaimDto createdOperationClaimDto = _mapper.Map<CreatedOperationClaimDto>(createdOperationClaim);

               return createdOperationClaimDto;
            }
        }
    }
}
