using FluentValidation;

namespace Kodlama.Io.Devs.Application.Features.LanguageTechnologies.Commands.CreateLanguageTechnology
{
    public class CreateLanguageTechnologyCommandValidator : AbstractValidator<CreateLanguageTechnologyCommand>
    {
        public CreateLanguageTechnologyCommandValidator()
        {
            RuleFor(l => l.LanguageId).NotEmpty();
            RuleFor(l => l.LanguageId).NotNull();
            RuleFor(l => l.LanguageId).GreaterThan(0);

            RuleFor(l => l.Name).NotEmpty();
            RuleFor(l => l.Name).NotNull();
            RuleFor(l => l.Name).MaximumLength(20);
        }
    }
}
