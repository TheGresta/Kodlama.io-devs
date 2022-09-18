using FluentValidation;

namespace Kodlama.Io.Devs.Application.Features.Authorizations.Commands.Login
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(l => l.UserForLoginDto.Email).NotEmpty();
            RuleFor(l => l.UserForLoginDto.Email).NotNull();
            RuleFor(l => l.UserForLoginDto.Email).MaximumLength(50);
            RuleFor(l => l.UserForLoginDto.Email).EmailAddress();

            RuleFor(l => l.UserForLoginDto.Password).NotEmpty();
            RuleFor(l => l.UserForLoginDto.Password).NotNull();
            RuleFor(l => l.UserForLoginDto.Password).MinimumLength(5);
            RuleFor(l => l.UserForLoginDto.Password).MaximumLength(25);
        }
    }
}
