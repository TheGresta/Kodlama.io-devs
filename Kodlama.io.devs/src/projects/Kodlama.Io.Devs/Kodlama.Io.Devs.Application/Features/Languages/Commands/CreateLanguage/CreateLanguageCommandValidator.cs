using FluentValidation;

namespace Kodlama.Io.Devs.Application.Features.Languages.Commands.CreateLanguage
{
    public class CreateLanguageCommandValidator : AbstractValidator<CreateLanguageCommand>
    {
        public CreateLanguageCommandValidator()
        {
            RuleFor(l => l.Name).NotEmpty();
            RuleFor(l => l.Name).NotNull();
            RuleFor(l => l.Name).MinimumLength(1);
            RuleFor(l => l.Name).MaximumLength(30);
        }
    }
}
