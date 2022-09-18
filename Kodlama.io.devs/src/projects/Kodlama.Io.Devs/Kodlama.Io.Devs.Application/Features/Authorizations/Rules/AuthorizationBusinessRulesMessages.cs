namespace Kodlama.Io.Devs.Application.Features.Authorizations.Rules
{
    public class AuthorizationBusinessRulesMessages
    {
        public string UserDoesNotExist => "Wrong mail address!";
        public string WrongPassword => "Wrong password!";
        public string UserAlreadyExists => "There is already an User in the system with given info!";
    }
}
