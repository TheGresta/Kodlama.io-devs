
using FluentValidation;

namespace Kodlama.Io.Devs.Application.Features.UserOperationClaims.Commands.CreateMultipleUserOperationClaim
{
    public class CreateMultipleUserOperationClaimCommandValidator : AbstractValidator<CreateMultipleUserOperationClaimCommand>
    {
        public CreateMultipleUserOperationClaimCommandValidator()
        {
            RuleFor(r => r.UserId).NotEmpty();
            RuleFor(r => r.UserId).NotNull();
            RuleFor(r => r.UserId).GreaterThan(0);

            RuleFor(r => r.RoleNames).NotNull();
            RuleFor(r => r.RoleNames).NotEmpty();
            RuleFor(r => r.RoleNames).Must(r => r.Length > 0);
        }
    }
}
