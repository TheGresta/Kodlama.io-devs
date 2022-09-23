using FluentValidation;

namespace Kodlama.Io.Devs.Application.Features.Developers.Commands.CreateDeveloper
{
    public class CreateDeveloperCommandValidator : AbstractValidator<CreateDeveloperCommand>
    {
        public CreateDeveloperCommandValidator()
        {
            RuleFor(r => r.Developer.Email).NotEmpty();
            RuleFor(r => r.Developer.Email).NotNull();
            RuleFor(r => r.Developer.Email).MinimumLength(8);
            RuleFor(r => r.Developer.Email).MaximumLength(50);
            RuleFor(r => r.Developer.Email).EmailAddress();

            RuleFor(r => r.Developer.FirstName).NotEmpty();
            RuleFor(r => r.Developer.FirstName).NotNull();
            RuleFor(r => r.Developer.FirstName).MinimumLength(2);
            RuleFor(r => r.Developer.FirstName).MaximumLength(20);

            RuleFor(r => r.Developer.LastName).NotEmpty();
            RuleFor(r => r.Developer.LastName).NotNull();
            RuleFor(r => r.Developer.LastName).MinimumLength(2);
            RuleFor(r => r.Developer.LastName).MaximumLength(50);

            RuleFor(r => r.Developer.PasswordHash).NotEmpty();
            RuleFor(r => r.Developer.PasswordHash).NotNull();

            RuleFor(r => r.Developer.PasswordSalt).NotEmpty();
            RuleFor(r => r.Developer.PasswordSalt).NotNull();
        }
    }
}
