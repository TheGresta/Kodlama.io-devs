﻿namespace Kodlama.Io.Devs.Application.Features.Users.Dtos
{
    public class GetByEmailUserDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        ICollection<string> UserOperationClaims { get; set; }
    }
}
