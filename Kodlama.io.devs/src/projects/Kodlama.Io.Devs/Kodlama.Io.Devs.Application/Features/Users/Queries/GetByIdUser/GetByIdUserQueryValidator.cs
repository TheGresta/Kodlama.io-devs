using FluentValidation;

namespace Kodlama.Io.Devs.Application.Features.Users.Queries.GetByIdUser
{
    public class GetByIdUserQueryValidator : AbstractValidator<GetByIdUserQuery>
    {
        public GetByIdUserQueryValidator()
        {
            RuleFor(u => u.Id).NotEmpty();
            RuleFor(u => u.Id).NotNull();
            RuleFor(u => u.Id).GreaterThan(0);
        }
    }
}
