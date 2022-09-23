using AutoMapper;
using Core.Security.Entities;
using Core.Security.JWT;
using Kodlama.Io.Devs.Application.Features.UserAuths.Commands.CreateAccessTokeUserAuth;
using Kodlama.Io.Devs.Application.Features.UserAuths.Commands.LoginUserAuth;
using Kodlama.Io.Devs.Application.Features.UserAuths.Dtos;

namespace Kodlama.Io.Devs.Application.Features.UserAuths.Profiles
{
    public class UserAuthMappingProfile : Profile
    {
        public UserAuthMappingProfile()
        {
            CreateMap<LoginUserAuthResultDto, CreateAccessTokenUserAuthResultDto>().ReverseMap();

            CreateMap<CreateAccessTokenUserAuthResultDto, AccessToken>().ReverseMap();
        }
    }
}
