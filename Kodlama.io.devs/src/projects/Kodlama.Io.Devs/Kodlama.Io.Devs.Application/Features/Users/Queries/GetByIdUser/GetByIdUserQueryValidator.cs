using FluentValidation;

namespace Kodlama.Io.Devs.Application.Features.Users.Queries.GetByIdUser
{
    public class GetByIdUserQueryValidator : AbstractValidator<GetByIdUserQuery>
    {
        public GetByIdUserQueryValidator()
        {
            RuleFor(r => r.Id).NotNull();
            RuleFor(r => r.Id).NotEmpty();
            RuleFor(r => r.Id).GreaterThan(0);
        }
    }
}
