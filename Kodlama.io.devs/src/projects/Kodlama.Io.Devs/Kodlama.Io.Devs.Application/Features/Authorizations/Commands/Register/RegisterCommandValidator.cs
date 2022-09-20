using FluentValidation;

namespace Kodlama.Io.Devs.Application.Features.Authorizations.Commands.Register
{
    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(r => r.UserForRegisterDto.FirstName).NotEmpty();
            RuleFor(r => r.UserForRegisterDto.FirstName).NotNull();
            RuleFor(r => r.UserForRegisterDto.FirstName).MaximumLength(50);

            RuleFor(r => r.UserForRegisterDto.LastName).NotEmpty();
            RuleFor(r => r.UserForRegisterDto.LastName).NotNull();
            RuleFor(r => r.UserForRegisterDto.LastName).MaximumLength(50);

            RuleFor(r => r.UserForRegisterDto.Email).NotEmpty();
            RuleFor(r => r.UserForRegisterDto.Email).NotNull();
            RuleFor(r => r.UserForRegisterDto.Email).MaximumLength(50);
            RuleFor(r => r.UserForRegisterDto.Email).EmailAddress();

            RuleFor(r => r.UserForRegisterDto.Password).NotEmpty();
            RuleFor(r => r.UserForRegisterDto.Password).NotNull();
            RuleFor(r => r.UserForRegisterDto.Password).MinimumLength(5);
            RuleFor(r => r.UserForRegisterDto.Password).MaximumLength(25);
        }
    }
}
