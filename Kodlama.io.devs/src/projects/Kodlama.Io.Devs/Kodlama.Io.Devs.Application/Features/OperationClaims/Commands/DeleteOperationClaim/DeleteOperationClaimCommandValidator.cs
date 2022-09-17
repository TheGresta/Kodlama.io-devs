using FluentValidation;

namespace Kodlama.Io.Devs.Application.Features.OperationClaims.Commands.DeleteOperationClaim
{
    public class DeleteOperationClaimCommandValidator : AbstractValidator<DeleteOperationClaimCommand>
    {
        public DeleteOperationClaimCommandValidator()
        {
            RuleFor(o => o.Id).NotEmpty();
            RuleFor(o => o.Id).NotNull();
            RuleFor(o => o.Id).GreaterThan(0);
        }
    }
}
