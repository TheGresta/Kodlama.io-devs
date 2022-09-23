using FluentValidation;

namespace Kodlama.Io.Devs.Application.Features.Users.Queries.GetByEmailUser
{
    public class GetByEmailUserQueryValidator : AbstractValidator<GetByEmailUserQuery>
    {
        public GetByEmailUserQueryValidator()
        {
            RuleFor(r => r.Email).NotEmpty();
            RuleFor(r => r.Email).NotNull();
            RuleFor(r => r.Email).MinimumLength(8);
            RuleFor(r => r.Email).MaximumLength(50);
            RuleFor(r => r.Email).EmailAddress();
        }
    }
}
