using FluentValidation;

namespace Kodlama.Io.Devs.Application.Features.Languages.Queries.GetListLanguage
{
    public class GetListLanguageQueryValidator : AbstractValidator<GetListLanguageQuery>
    {
        public GetListLanguageQueryValidator()
        {
            RuleFor(l => l.PageRequest.Page).NotEmpty();
            RuleFor(l => l.PageRequest.Page).GreaterThanOrEqualTo(0);

            RuleFor(l => l.PageRequest.PageSize).NotEmpty();
            RuleFor(l => l.PageRequest.PageSize).GreaterThan(0);
        }
    }
}
