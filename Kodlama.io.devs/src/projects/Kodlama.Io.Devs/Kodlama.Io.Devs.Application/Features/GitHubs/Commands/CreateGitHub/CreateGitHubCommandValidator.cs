using FluentValidation;

namespace Kodlama.Io.Devs.Application.Features.GitHubs.Commands.CreateGitHub
{
    public class CreateGitHubCommandValidator : AbstractValidator<CreateGitHubCommand>
    {
        public CreateGitHubCommandValidator()
        {
            RuleFor(g => g.Name).NotEmpty();
            RuleFor(g => g.Name).NotNull();
            RuleFor(g => g.Name).MinimumLength(5);
            RuleFor(g => g.Name).MaximumLength(20);

            RuleFor(g => g.UserId).NotEmpty();
            RuleFor(g => g.UserId).NotNull();
            RuleFor(g => g.UserId).GreaterThan(0);
        }
    }
}
