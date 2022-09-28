using AutoMapper;
using Core.Security.Entities;
using Kodlama.Io.Devs.Application.Features.UserOperationClaims.Dtos;
using Kodlama.Io.Devs.Application.Features.UserOperationClaims.Rules;
using Kodlama.Io.Devs.Application.Services.Repositories;
using MediatR;

namespace Kodlama.Io.Devs.Application.Features.UserOperationClaims.Commands.CreateMultipleUserOperationClaim
{
    public partial class CreateMultipleUserOperationClaimCommand : IRequest<CreatedMultipleUserOperationClaimDto>
    {
        public int UserId { get; set; }
        public string[] RoleNames { get; set; }

        public class CreateMultipleUserOperationClaimCommandHandler : IRequestHandler<CreateMultipleUserOperationClaimCommand,
                                                                                      CreatedMultipleUserOperationClaimDto>
        {
            private readonly IUserOperationClaimRepository _userOperationClaimRepository;
            private readonly IMapper _mapper;
            private readonly UserOperationClaimBusinessRules _userOperationClaimBusinessRules;
            private readonly IOperationClaimRepository _operationClaimRepository;

            public CreateMultipleUserOperationClaimCommandHandler(IUserOperationClaimRepository userOperationClaimRepository, 
                                                                  IMapper mapper, 
                                                                  UserOperationClaimBusinessRules userOperationClaimBusinessRules, 
                                                                  IOperationClaimRepository operationClaimRepository)
            {
                _userOperationClaimRepository = userOperationClaimRepository;
                _mapper = mapper;
                _userOperationClaimBusinessRules = userOperationClaimBusinessRules;
                _operationClaimRepository = operationClaimRepository;
            }

            public async Task<CreatedMultipleUserOperationClaimDto> Handle(CreateMultipleUserOperationClaimCommand request, CancellationToken cancellationToken)
            {
                await _userOperationClaimBusinessRules.UserShouldBeExistWhenRequested(request.UserId);

                CreatedMultipleUserOperationClaimDto createdMultipleUserOperationClaimDto = new();

                foreach(string role in request.RoleNames)
                {
                    OperationClaim? operationClaim = await _operationClaimRepository.GetAsync(o => o.Name == role);
                    await _userOperationClaimBusinessRules.OperationClaimCanNotBeNullWhenRequested(operationClaim);

                    UserOperationClaim userOperationClaim = new() { UserId = request.UserId, OperationClaimId = operationClaim.Id };
                    UserOperationClaim createdUserOperationClaim = await _userOperationClaimRepository.AddAsync(userOperationClaim);
                    CreatedUserOperationClaimDto createdUserOperationClaimDto = _mapper.Map<CreatedUserOperationClaimDto>(createdUserOperationClaim);

                    createdMultipleUserOperationClaimDto.CreatedMultipleUserOperationClaimDtos.Add(createdUserOperationClaimDto);
                }

                return createdMultipleUserOperationClaimDto;
            }
        }
    }
}
