using FluentValidation;

namespace Kodlama.Io.Devs.Application.Features.Languages.Queries.GetByIdLanguage
{
    public class GetByIdLanguageQueryValidator : AbstractValidator<GetByIdLanguageQuery>
    {
        public GetByIdLanguageQueryValidator()
        {
            RuleFor(l => l.Id).NotEmpty();
            RuleFor(l => l.Id).GreaterThan(0);
        }
    }
}
