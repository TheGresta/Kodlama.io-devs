using AutoMapper;
using Core.Persistence.Paging;
using Core.Security.Entities;
using Kodlama.Io.Devs.Application.Features.Users.Dtos;
using Kodlama.Io.Devs.Application.Features.Users.Models;

namespace Kodlama.Io.Devs.Application.Features.Users.Profiles
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<User, GetByEmailUserDto>()
                .ForMember(d => d.Name, opt => opt.MapFrom(u => $"{u.FirstName} {u.LastName}"))
                .ReverseMap();

            CreateMap<User, GetByIdUserDto>()
                .ForMember(d => d.Name, opt => opt.MapFrom(u => $"{u.FirstName} {u.LastName}"))
                .ReverseMap();

            CreateMap<User, ListUserDto>()
                .ForMember(d => d.Name, opt => opt.MapFrom(u => $"{u.FirstName} {u.LastName}"))
                .ReverseMap();

            CreateMap<IPaginate<User>, UserListModel>()
                .ReverseMap();

            CreateMap<ICollection<UserOperationClaim>, ICollection<string>>();

            CreateMap<UserOperationClaim, string>()
                .ConvertUsing(u => u.OperationClaim.Name);
        }
    }
}
