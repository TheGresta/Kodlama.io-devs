using FluentValidation;

namespace Kodlama.Io.Devs.Application.Features.Users.Commands.UpdateUserPassword
{
    public class UpdateUserPasswordCommandValidator : AbstractValidator<UpdateUserPasswordCommand>
    {
        public UpdateUserPasswordCommandValidator()
        {
            RuleFor(r => r.OldPassword).NotEmpty();
            RuleFor(r => r.OldPassword).NotNull();
            RuleFor(r => r.OldPassword).MinimumLength(5);
            RuleFor(r => r.OldPassword).MaximumLength(30);

            RuleFor(r => r.NewPassword).NotEmpty();
            RuleFor(r => r.NewPassword).NotNull();
            RuleFor(r => r.NewPassword).MinimumLength(5);
            RuleFor(r => r.NewPassword).MaximumLength(30);
        }
    }
}
