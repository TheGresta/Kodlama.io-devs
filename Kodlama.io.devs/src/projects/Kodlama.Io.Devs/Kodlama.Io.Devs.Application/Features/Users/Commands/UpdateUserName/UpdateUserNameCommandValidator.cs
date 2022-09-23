using FluentValidation;

namespace Kodlama.Io.Devs.Application.Features.Users.Commands.UpdateUserName
{
    public class UpdateUserNameCommandValidator : AbstractValidator<UpdateUserNameCommand>
    {
        public UpdateUserNameCommandValidator()
        {
            RuleFor(r => r.FirstName).NotEmpty();
            RuleFor(r => r.FirstName).NotNull();
            RuleFor(r => r.FirstName).MinimumLength(2);
            RuleFor(r => r.FirstName).MaximumLength(20);

            RuleFor(r => r.LastName).NotEmpty();
            RuleFor(r => r.LastName).NotNull();
            RuleFor(r => r.LastName).MinimumLength(2);
            RuleFor(r => r.LastName).MaximumLength(50);

            RuleFor(r => r.Password).NotEmpty();
            RuleFor(r => r.Password).NotNull();
            RuleFor(r => r.Password).MinimumLength(5);
            RuleFor(r => r.Password).MaximumLength(30);
        }
    }
}
