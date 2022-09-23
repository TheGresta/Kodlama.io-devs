using FluentValidation;

namespace Kodlama.Io.Devs.Application.Features.DeveloperAuths.Commands.RegisterDeveloperAuth
{
    public class RegisterDeveloperAuthCommandValidator : AbstractValidator<RegisterDeveloperAuthCommand>
    {
        public RegisterDeveloperAuthCommandValidator()
        {
            RuleFor(r => r.UserForRegisterDto.Email).NotEmpty();
            RuleFor(r => r.UserForRegisterDto.Email).NotNull();
            RuleFor(r => r.UserForRegisterDto.Email).MinimumLength(8);
            RuleFor(r => r.UserForRegisterDto.Email).MaximumLength(50);
            RuleFor(r => r.UserForRegisterDto.Email).EmailAddress();

            RuleFor(r => r.UserForRegisterDto.Password).NotEmpty();
            RuleFor(r => r.UserForRegisterDto.Password).NotNull();
            RuleFor(r => r.UserForRegisterDto.Password).MinimumLength(5);
            RuleFor(r => r.UserForRegisterDto.Password).MaximumLength(30);

            RuleFor(r => r.UserForRegisterDto.FirstName).NotEmpty();
            RuleFor(r => r.UserForRegisterDto.FirstName).NotNull();
            RuleFor(r => r.UserForRegisterDto.FirstName).MinimumLength(2);
            RuleFor(r => r.UserForRegisterDto.FirstName).MaximumLength(20);

            RuleFor(r => r.UserForRegisterDto.LastName).NotEmpty();
            RuleFor(r => r.UserForRegisterDto.LastName).NotNull();
            RuleFor(r => r.UserForRegisterDto.LastName).MinimumLength(2);
            RuleFor(r => r.UserForRegisterDto.LastName).MaximumLength(50);
        }
    }
}
