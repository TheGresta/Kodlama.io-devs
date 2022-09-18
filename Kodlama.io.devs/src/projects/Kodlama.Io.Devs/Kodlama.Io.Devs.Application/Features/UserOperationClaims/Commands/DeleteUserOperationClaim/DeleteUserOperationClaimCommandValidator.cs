using FluentValidation;

namespace Kodlama.Io.Devs.Application.Features.UserOperationClaims.Commands.DeleteUserOperationClaim
{
    public class DeleteUserOperationClaimCommandValidator : AbstractValidator<DeleteUserOperationClaimCommand>
    {
        public DeleteUserOperationClaimCommandValidator()
        {
            RuleFor(u => u.Id).NotEmpty();
            RuleFor(u => u.Id).NotNull();
            RuleFor(u => u.Id).GreaterThan(0);
        }
    }
}
