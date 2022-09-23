using FluentValidation;

namespace Kodlama.Io.Devs.Application.Features.Users.Commands.UpdateUserEmail
{
    public class UpdateUserEmailCommandValidator : AbstractValidator<UpdateUserEmailCommand>
    {
        public UpdateUserEmailCommandValidator()
        {
            RuleFor(r => r.Email).NotEmpty();
            RuleFor(r => r.Email).NotNull();
            RuleFor(r => r.Email).MinimumLength(8);
            RuleFor(r => r.Email).MaximumLength(50);
            RuleFor(r => r.Email).EmailAddress();

            RuleFor(r => r.Password).NotEmpty();
            RuleFor(r => r.Password).NotNull();
            RuleFor(r => r.Password).MinimumLength(5);
            RuleFor(r => r.Password).MaximumLength(30);
        }
    }
}
