﻿namespace Kodlama.Io.Devs.Application.Features.Users.Dtos
{
    public class GetByIdUserDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public ICollection<string> UserOperationClaims { get; set; }
    }
}
