using FluentValidation;

namespace Kodlama.Io.Devs.Application.Features.LanguageTechnologies.Queries.GetByIdLanguageTechnology
{
    public class GetByIdLanguageTechnologyQueryValidator : AbstractValidator<GetByIdLanguageTechnologyQuery>
    {
        public GetByIdLanguageTechnologyQueryValidator()
        {
            RuleFor(l => l.Id).NotEmpty();
            RuleFor(l => l.Id).NotNull();
            RuleFor(l => l.Id).GreaterThan(0);
        }
    }
}
