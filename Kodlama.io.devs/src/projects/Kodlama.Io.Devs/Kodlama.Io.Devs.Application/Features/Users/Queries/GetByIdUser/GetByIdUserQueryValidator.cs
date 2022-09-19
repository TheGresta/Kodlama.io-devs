using FluentValidation;

namespace Kodlama.Io.Devs.Application.Features.Users.Queries.GetByIdUser
{
    public class GetByIdUserQueryValidator : AbstractValidator<GetByIdUserQuery>
    {
        public GetByIdUserQueryValidator()
        {
            RuleFor(u => u.UserId).NotEmpty();
            RuleFor(u => u.UserId).NotNull();
            RuleFor(u => u.UserId).GreaterThan(0);
        }
    }
}
