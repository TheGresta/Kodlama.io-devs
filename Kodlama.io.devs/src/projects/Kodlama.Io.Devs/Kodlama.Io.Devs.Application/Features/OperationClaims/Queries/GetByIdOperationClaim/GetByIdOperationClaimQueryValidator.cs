using FluentValidation;

namespace Kodlama.Io.Devs.Application.Features.OperationClaims.Queries.GetByIdOperationClaim
{
    public class GetByIdOperationClaimQueryValidator : AbstractValidator<GetByIdOperationClaimQuery>
    {
        public GetByIdOperationClaimQueryValidator()
        {
            RuleFor(o => o.Id).NotEmpty();
            RuleFor(o => o.Id).NotNull();
            RuleFor(o => o.Id).GreaterThan(0);
        }
    }
}
