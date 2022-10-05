using Core.Security.Entities;
using Core.Security.JWT;

namespace Kodlama.Io.Devs.Application.Features.DeveloperAuths.Dtos
{
    public class RegisterRefreshedTokenDto
    {
        public AccessToken AccessToken { get; set; }
        public RefreshToken RefreshToken { get; set; }
    }
}
