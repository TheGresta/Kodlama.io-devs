using FluentValidation;

namespace Kodlama.Io.Devs.Application.Features.GitHubs.Queries.GetByIdGitHub
{
    public class GetByIdGitHubQueryValidator : AbstractValidator<GetByIdGitHubQuery>
    {
        public GetByIdGitHubQueryValidator()
        {
            RuleFor(g => g.Id).NotEmpty();
            RuleFor(g => g.Id).NotNull();
            RuleFor(g => g.Id).GreaterThan(0);
        }
    }
}
