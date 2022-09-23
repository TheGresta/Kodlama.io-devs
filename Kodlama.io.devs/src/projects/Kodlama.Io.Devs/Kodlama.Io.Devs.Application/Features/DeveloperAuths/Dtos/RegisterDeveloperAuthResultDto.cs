namespace Kodlama.Io.Devs.Application.Features.DeveloperAuths.Dtos
{
    public class RegisterDeveloperAuthResultDto
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
