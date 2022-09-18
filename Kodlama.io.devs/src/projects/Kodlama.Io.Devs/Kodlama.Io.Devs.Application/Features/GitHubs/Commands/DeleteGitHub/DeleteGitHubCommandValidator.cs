using FluentValidation;

namespace Kodlama.Io.Devs.Application.Features.GitHubs.Commands.DeleteGitHub
{
    public class DeleteGitHubCommandValidator : AbstractValidator<DeleteGitHubCommand>
    {
        public DeleteGitHubCommandValidator()
        {
            RuleFor(g => g.Id).NotEmpty();
            RuleFor(g => g.Id).NotNull();
            RuleFor(g => g.Id).GreaterThan(0);
        }
    }
}
