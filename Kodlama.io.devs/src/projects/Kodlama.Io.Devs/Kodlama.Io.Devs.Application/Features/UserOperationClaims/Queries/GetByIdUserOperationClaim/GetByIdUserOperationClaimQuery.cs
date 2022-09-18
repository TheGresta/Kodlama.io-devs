using AutoMapper;
using Core.Security.Entities;
using Kodlama.Io.Devs.Application.Features.UserOperationClaims.Dtos;
using Kodlama.Io.Devs.Application.Services.Repositories;
using MediatR;


namespace Kodlama.Io.Devs.Application.Features.UserOperationClaims.Queries.GetByIdUserOperationClaim
{
    public partial class GetByIdUserOperationClaimQuery : IRequest<GetByIdUserOperationClaimDto>
    {
        public int Id { get; set; }

        public class GetByIdUserOperationClaimQueryHandler : IRequestHandler<GetByIdOperationClaimQuery, GetByIdOperationClaimDto>
        {
            private readonly IUserOperationClaimRepository _userOperationClaimRepository;
            private readonly IMapper _mapper;
            private readonly UserOperationClaimBusinessRules _userOperationClaimBusinessRules;

            public GetByIdUserOperationClaimQueryHandler(IUserOperationClaimRepository userOperationClaimRepository, UserOperationClaimBusinessRules userOperationClaimBusinessRules, IMapper mapper)
            {
                _userOperationClaimRepository = userOperationClaimRepository;
                _mapper = mapper;
                _userOperationClaimBusinessRules = userOperationClaimBusinessRules;                
            }

            public async Task<GetByIdUserOperationClaimDto> Handle(GetByIdUserOperationClaimQuery request, CancellationToken cancellationToken)
            {
                UserOperationClaim? userOperationClaim = await _userOperationClaimRepository.GetAsync(u => u.Id == request.Id);
                GetByIdUserOperationClaimDto getByIdUserOperationClaimDto = _mapper.Map<GetByIdUserOperationClaimDto>(userOperationClaim);

                return getByIdUserOperationClaimDto;
            }
        }
    }
}
