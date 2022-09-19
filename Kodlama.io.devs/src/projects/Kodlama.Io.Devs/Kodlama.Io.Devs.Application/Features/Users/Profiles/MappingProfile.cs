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

            CreateMap<ICollection<string>, ICollection<UserOperationClaim>>().ReverseMap();
            CreateMap<UserOperationClaim, string>()
                .ForMember(s => s, opt => opt.MapFrom(u => u.OperationClaim.Name))
                .IncludeMembers(u => u.OperationClaim).ReverseMap();

            CreateMap<IPaginate<User>, UserListModel>();
        }
    }
}
