using FluentValidation;

namespace Kodlama.Io.Devs.Application.Features.UserOperationClaims.Queries.GetByIdUserOperationClaim
{
    public class GetByIdUserOperationClaimQueryValidator : AbstractValidator<GetByIdUserOperationClaimQuery>
    {
        public GetByIdUserOperationClaimQueryValidator()
        {
            RuleFor(u => u.Id).NotEmpty();
            RuleFor(u => u.Id).NotNull();
            RuleFor(u => u.Id).GreaterThan(0);
        }
    }
}
