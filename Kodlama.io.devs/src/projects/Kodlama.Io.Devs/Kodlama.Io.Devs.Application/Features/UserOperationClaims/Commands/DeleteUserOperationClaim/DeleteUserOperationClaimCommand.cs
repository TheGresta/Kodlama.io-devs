using AutoMapper;
using Core.Application.Pipelines.Authorization;
using Core.Security.Entities;
using Kodlama.Io.Devs.Application.Features.UserOperationClaims.Dtos;
using Kodlama.Io.Devs.Application.Features.UserOperationClaims.Rules;
using Kodlama.Io.Devs.Application.Services.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Kodlama.Io.Devs.Application.Features.UserOperationClaims.Commands.DeleteUserOperationClaim
{
    public partial class DeleteUserOperationClaimCommand : IRequest<DeletedUserOperationClaimDto>, ISecuredRequest
    {
        public int Id { get; set; }

        public string[] Roles => new[] { "Admin" };

        public class DeleteUserOperationClaimCommandHandler : IRequestHandler<DeleteUserOperationClaimCommand, DeletedUserOperationClaimDto>
        {
            private readonly IUserOperationClaimRepository _userOperationClaimRepository;
            private readonly IMapper _mapper;
            private readonly UserOperationClaimBusinessRules _userOperationClaimBusinessRules;

            public DeleteUserOperationClaimCommandHandler(IUserOperationClaimRepository userOperationClaimRepository, IMapper mapper, UserOperationClaimBusinessRules userOperationClaimBusinessRules)
            {
                _userOperationClaimRepository = userOperationClaimRepository;
                _mapper = mapper;
                _userOperationClaimBusinessRules = userOperationClaimBusinessRules;
            }

            public async Task<DeletedUserOperationClaimDto> Handle(DeleteUserOperationClaimCommand request, CancellationToken cancellationToken)
            {
                await _userOperationClaimBusinessRules.UserOperationClaimShouldBeExistWhenRequested(request.Id);

                UserOperationClaim? userOperationClaim = await _userOperationClaimRepository.GetAsync(u => u.Id == request.Id);
                UserOperationClaim deletedUserOperationClaim = await _userOperationClaimRepository.DeleteAsync(userOperationClaim, include: x => x.Include(g => g.User).Include(g => g.OperationClaim));
                DeletedUserOperationClaimDto deletedUserOperationClaimDto = _mapper.Map<DeletedUserOperationClaimDto>(deletedUserOperationClaim);

                return deletedUserOperationClaimDto;
            }
        }
    }
}
