using FluentValidation;

namespace Kodlama.Io.Devs.Application.Features.Developers.Commands.UpdateDeveloperGitHub
{
    public class UpdateDeveloperGitHubCommandValidator : AbstractValidator<UpdateDeveloperGitHubCommand>
    {
        public UpdateDeveloperGitHubCommandValidator()
        {
            RuleFor(r => r.GitHub).MaximumLength(30);
        }
    }
}
