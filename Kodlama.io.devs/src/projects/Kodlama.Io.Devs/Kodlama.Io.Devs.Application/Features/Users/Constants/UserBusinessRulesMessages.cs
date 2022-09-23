namespace Kodlama.Io.Devs.Application.Features.Users.Constants
{
    public class UserBusinessRulesMessages
    {
        public string EmailDoesNotExist => "There is no any user with given email address in the system!";
        public string IdDoesNotExist => "There is no any user with given Id in the system!";
        public string ThereIsNoAnyDataInList => "There is no any user data in the system as required!";
        public string EmailAlreadyExist => "There is already an user with same email address in the system!";
        public string PasswordIsIncorrect => "Wrong password!";
    }
}
