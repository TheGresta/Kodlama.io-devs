namespace Kodlama.Io.Devs.Application.Features.UserAuths.Dtos
{
    public class CreateAccessTokenUserAuthResultDto
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
