using FluentValidation;

namespace Kodlama.Io.Devs.Application.Features.UserAuths.Commands.LoginUserAuth
{
    public class LoginUserAuthCommandValidator : AbstractValidator<LoginUserAuthCommand>
    {
        public LoginUserAuthCommandValidator()
        {
            RuleFor(r => r.UserForLoginDto.Email).NotEmpty();
            RuleFor(r => r.UserForLoginDto.Email).NotNull();
            RuleFor(r => r.UserForLoginDto.Email).MinimumLength(8);
            RuleFor(r => r.UserForLoginDto.Email).MaximumLength(50);
            RuleFor(r => r.UserForLoginDto.Email).EmailAddress();

            RuleFor(r => r.UserForLoginDto.Password).NotEmpty();
            RuleFor(r => r.UserForLoginDto.Password).NotNull();
            RuleFor(r => r.UserForLoginDto.Password).MinimumLength(5);
            RuleFor(r => r.UserForLoginDto.Password).MaximumLength(30);
        }
    }
}
