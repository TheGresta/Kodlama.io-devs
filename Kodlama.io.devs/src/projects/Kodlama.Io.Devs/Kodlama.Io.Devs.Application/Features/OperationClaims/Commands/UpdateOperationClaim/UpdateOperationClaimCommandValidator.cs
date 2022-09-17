using FluentValidation;

namespace Kodlama.Io.Devs.Application.Features.OperationClaims.Commands.UpdateOperationClaim
{
    public class UpdateOperationClaimCommandValidator : AbstractValidator<UpdateOperationClaimCommand>
    {
        public UpdateOperationClaimCommandValidator()
        {
            RuleFor(o => o.Id).NotEmpty();
            RuleFor(o => o.Id).NotNull();
            RuleFor(o => o.Id).GreaterThan(0);

            RuleFor(o => o.Name).NotEmpty();
            RuleFor(o => o.Name).NotNull();
            RuleFor(o => o.Name).MinimumLength(1);
            RuleFor(o => o.Name).MaximumLength(30);
        }
    }
}
