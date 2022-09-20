using AutoMapper;
using Core.Persistence.Paging;
using Core.Security.Entities;
using Kodlama.Io.Devs.Application.Features.Users.Dtos;
using Kodlama.Io.Devs.Application.Features.Users.Models;

namespace Kodlama.Io.Devs.Application.Features.Users.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, CommandUserDto>()
                .ForMember(u => u.Email, opt => opt.MapFrom(e => e.Email))
                .ForMember(u => u.Name, opt => opt.MapFrom(e => $"{e.FirstName} {e.LastName}"))
                .ReverseMap();

            CreateMap<UserOperationClaim, string>()
                .ConvertUsing(l => l.OperationClaim.Name);

            CreateMap<IPaginate<User>, UserListModel>();            
        }
    }
}
