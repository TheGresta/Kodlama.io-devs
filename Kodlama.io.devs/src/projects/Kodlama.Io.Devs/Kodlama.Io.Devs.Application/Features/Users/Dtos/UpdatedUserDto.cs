namespace Kodlama.Io.Devs.Application.Features.Users.Dtos
{
    public class UpdatedUserDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public ICollection<string> UserOperationClaims { get; set; }
        public string GitHubLink { get; set; }
    }
}
