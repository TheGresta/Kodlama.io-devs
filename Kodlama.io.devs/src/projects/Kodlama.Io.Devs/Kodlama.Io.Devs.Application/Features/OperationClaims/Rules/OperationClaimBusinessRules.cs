using Core.CrossCuttingConcerns.Exceptions;
using Core.Persistence.Paging;
using Core.Security.Entities;
using Kodlama.Io.Devs.Application.Services.Repositories;

namespace Kodlama.Io.Devs.Application.Features.OperationClaims.Rules
{
    public class OperationClaimBusinessRules
    {
        private readonly IOperationClaimRepository _operationClaimRepository;
        private readonly OperationClaimBusinessRulesMessages _messages;

        public OperationClaimBusinessRules(IOperationClaimRepository operationClaimRepository, OperationClaimBusinessRulesMessages messages)
        {
            _operationClaimRepository = operationClaimRepository;
            _messages = messages;
        }

        public async Task OperationClaimNameCanNotBeDuplicatedWhenInsertedOrUpdated(string name)
        {
            IPaginate<OperationClaim> result = await _operationClaimRepository.GetListAsync(o => o.Name == name, enableTracking: false);
            if (result.Items.Any()) throw new BusinessException(_messages.OperationClaimNameAlreadyExist);
        }

        public async Task OperationClaimShouldBeExistWhenRequested(int id)
        {
            OperationClaim? operationClaim = await _operationClaimRepository.GetAsync(o => o.Id == id);
            if (operationClaim == null) throw new BusinessException(_messages.OperationClaimDoesNotExist);
        }

        public async Task ShouldBeSomeDataInTheOperationClaimTableWhenRequested(IPaginate<OperationClaim> operationClaims)
        {
            if (!operationClaims.Items.Any()) throw new BusinessException(_messages.OperationClaimDataNotExist);
        }
    }
}
