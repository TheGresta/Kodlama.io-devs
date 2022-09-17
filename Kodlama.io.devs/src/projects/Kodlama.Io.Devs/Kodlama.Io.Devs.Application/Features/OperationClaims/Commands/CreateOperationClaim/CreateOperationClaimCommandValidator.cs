using FluentValidation;

namespace Kodlama.Io.Devs.Application.Features.OperationClaims.Commands.CreateOperationClaim
{
    public class CreateOperationClaimCommandValidator : AbstractValidator<CreateOperationClaimCommand>
    {
        public CreateOperationClaimCommandValidator()
        {
            RuleFor(o => o.Name).NotEmpty();
            RuleFor(o => o.Name).NotNull();
            RuleFor(o => o.Name).MinimumLength(1);
            RuleFor(o => o.Name).MaximumLength(30);
        }
    }
}
