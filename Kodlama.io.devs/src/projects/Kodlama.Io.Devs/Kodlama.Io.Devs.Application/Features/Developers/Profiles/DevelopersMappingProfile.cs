using AutoMapper;
using Kodlama.Io.Devs.Application.Features.Developers.Dtos;
using Kodlama.Io.Devs.Domain.Entities;

namespace Kodlama.Io.Devs.Application.Features.Developers.Profiles
{
    public class DevelopersMappingProfile : Profile
    {
        public DevelopersMappingProfile()
        {
            CreateMap<Developer, CreateDeveloperResultDto>()
                .ForMember(c => c.Name, opt => opt.MapFrom(d => $"{d.FirstName} {d.LastName}"))
                .ReverseMap();
        }
    }
}
