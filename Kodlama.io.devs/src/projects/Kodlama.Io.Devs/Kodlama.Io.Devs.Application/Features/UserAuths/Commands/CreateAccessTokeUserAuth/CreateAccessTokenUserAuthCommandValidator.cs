using FluentValidation;

namespace Kodlama.Io.Devs.Application.Features.UserAuths.Commands.CreateAccessTokeUserAuth
{
    public class CreateAccessTokenUserAuthCommandValidator : AbstractValidator<CreateAccessTokenUserAuthCommand>
    {
        public CreateAccessTokenUserAuthCommandValidator()
        {
            RuleFor(r => r.User.Email).NotEmpty();
            RuleFor(r => r.User.Email).NotNull();
            RuleFor(r => r.User.Email).MinimumLength(8);
            RuleFor(r => r.User.Email).MaximumLength(50);
            RuleFor(r => r.User.Email).EmailAddress();

            RuleFor(r => r.User.FirstName).NotEmpty();
            RuleFor(r => r.User.FirstName).NotNull();
            RuleFor(r => r.User.FirstName).MinimumLength(2);
            RuleFor(r => r.User.FirstName).MaximumLength(20);

            RuleFor(r => r.User.LastName).NotEmpty();
            RuleFor(r => r.User.LastName).NotNull();
            RuleFor(r => r.User.LastName).MinimumLength(2);
            RuleFor(r => r.User.LastName).MaximumLength(50);

            RuleFor(r => r.User.UserOperationClaims).NotEmpty();
            RuleFor(r => r.User.UserOperationClaims).NotNull();
            RuleFor(r => r.User.UserOperationClaims).Must(u => u.Any());
        }
    }
}
