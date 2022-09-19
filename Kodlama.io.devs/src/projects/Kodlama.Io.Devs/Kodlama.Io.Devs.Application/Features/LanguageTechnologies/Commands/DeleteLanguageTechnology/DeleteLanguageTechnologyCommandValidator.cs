using FluentValidation;

namespace Kodlama.Io.Devs.Application.Features.LanguageTechnologies.Commands.DeleteLanguageTechnology
{
    public class DeleteLanguageTechnologyCommandValidator : AbstractValidator<DeleteLanguageTechnologyCommand>
    {
        public DeleteLanguageTechnologyCommandValidator()
        {
            RuleFor(l => l.Id).NotEmpty();
            RuleFor(l => l.Id).NotNull();
            RuleFor(l => l.Id).GreaterThan(0);
        }
    }
}
