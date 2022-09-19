using AutoMapper;
using Core.Security.Dtos;
using Core.Security.Entities;

namespace Kodlama.Io.Devs.Application.Features.Authorizations.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserForLoginDto, UserForRegisterDto>().ReverseMap();
        }
    }
}
