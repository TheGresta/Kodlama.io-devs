using FluentValidation;

namespace Kodlama.Io.Devs.Application.Features.LanguageTechnologies.Commands.UpdateLanguageTechnology
{
    public class UpdateLanguageTechnologyCommandValidator : AbstractValidator<UpdateLanguageTechnologyCommand>
    {
        public UpdateLanguageTechnologyCommandValidator()
        {
            RuleFor(l => l.Id).NotEmpty();
            RuleFor(l => l.Id).NotNull();
            RuleFor(l => l.Id).GreaterThan(0);

            RuleFor(l => l.LanguageId).NotEmpty();
            RuleFor(l => l.LanguageId).NotNull();
            RuleFor(l => l.LanguageId).GreaterThan(0);

            RuleFor(l => l.Name).NotEmpty();
            RuleFor(l => l.Name).NotNull();
            RuleFor(l => l.Name).MaximumLength(20);
        }
    }
}
