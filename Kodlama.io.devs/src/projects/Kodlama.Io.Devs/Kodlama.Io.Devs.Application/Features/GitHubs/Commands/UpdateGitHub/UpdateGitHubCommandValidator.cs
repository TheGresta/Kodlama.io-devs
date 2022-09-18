using FluentValidation;

namespace Kodlama.Io.Devs.Application.Features.GitHubs.Commands.UpdateGitHub
{
    public class UpdateGitHubCommandValidator : AbstractValidator<UpdateGitHubCommand>
    {
        public UpdateGitHubCommandValidator()
        {
            RuleFor(g => g.Name).NotEmpty();
            RuleFor(g => g.Name).NotNull();
            RuleFor(g => g.Name).MinimumLength(5);
            RuleFor(g => g.Name).MaximumLength(20);

            RuleFor(g => g.UserId).NotEmpty();
            RuleFor(g => g.UserId).NotNull();
            RuleFor(g => g.UserId).GreaterThan(0);

            RuleFor(g => g.Id).NotEmpty();
            RuleFor(g => g.Id).NotNull();
            RuleFor(g => g.Id).GreaterThan(0);
        }
    }
}
