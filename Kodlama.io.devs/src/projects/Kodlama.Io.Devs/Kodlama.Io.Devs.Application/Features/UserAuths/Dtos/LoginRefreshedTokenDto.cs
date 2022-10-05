using Core.Security.Entities;
using Core.Security.JWT;

namespace Kodlama.Io.Devs.Application.Features.UserAuths.Dtos
{
    public class LoginRefreshedTokenDto
    {
        public AccessToken AccessToken { get; set; }
        public RefreshToken RefreshToken { get; set; }
    }
}
