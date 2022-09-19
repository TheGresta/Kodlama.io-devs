using FluentValidation;

namespace Kodlama.Io.Devs.Application.Features.Users.Commands.UpdateUser
{
    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator()
        {
            RuleFor(r => r.UserId).NotEmpty();
            RuleFor(r => r.UserId).NotEmpty();
            RuleFor(r => r.UserId).GreaterThan(0);

            RuleFor(r => r.UserForRegisterDto.FirstName).NotEmpty();
            RuleFor(r => r.UserForRegisterDto.FirstName).NotNull();
            RuleFor(r => r.UserForRegisterDto.FirstName).MaximumLength(50);

            RuleFor(r => r.UserForRegisterDto.LastName).NotEmpty();
            RuleFor(r => r.UserForRegisterDto.LastName).NotNull();
            RuleFor(r => r.UserForRegisterDto.LastName).MaximumLength(50);

            RuleFor(r => r.UserForRegisterDto.Email).NotEmpty();
            RuleFor(r => r.UserForRegisterDto.Email).NotNull();
            RuleFor(r => r.UserForRegisterDto.Email).MaximumLength(50);
            RuleFor(r => r.UserForRegisterDto.Email).EmailAddress();

            RuleFor(r => r.UserForRegisterDto.Password).NotEmpty();
            RuleFor(r => r.UserForRegisterDto.Password).NotNull();
            RuleFor(r => r.UserForRegisterDto.Password).MinimumLength(5);
            RuleFor(r => r.UserForRegisterDto.Password).MaximumLength(25);

            RuleFor(r => r.OperationClaimIdList).Must(list => list.Count > 0);
            RuleFor(r => r.OperationClaimIdList).ForEach(i => i.GreaterThan(0));

            RuleFor(r => r.GitHubName).Must(g => CheckGitHubName(g)).WithMessage("GitHub name should be empty or between 5 - 20 size.");
        }

        private bool CheckGitHubName(string gitHubName)
        {
            if (gitHubName == null || gitHubName == "")
                return true;

            if (gitHubName.Length >= 5 && gitHubName.Length <= 20)
                return true;

            return false;
        }
    }
}
