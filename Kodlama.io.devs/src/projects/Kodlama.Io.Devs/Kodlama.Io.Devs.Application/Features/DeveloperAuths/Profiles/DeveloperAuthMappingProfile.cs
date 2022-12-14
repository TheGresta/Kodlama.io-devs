using AutoMapper;
using Core.Security.Dtos;
using Core.Security.Entities;
using Kodlama.Io.Devs.Domain.Entities;

namespace Kodlama.Io.Devs.Application.Features.DeveloperAuths.Profiles
{
    public class DeveloperAuthMappingProfile : Profile
    {
        public DeveloperAuthMappingProfile()
        {
            CreateMap<User, Developer>()
                .ReverseMap();

            CreateMap<Developer, UserForRegisterDto>()
                .ReverseMap();
        }
    }
}
