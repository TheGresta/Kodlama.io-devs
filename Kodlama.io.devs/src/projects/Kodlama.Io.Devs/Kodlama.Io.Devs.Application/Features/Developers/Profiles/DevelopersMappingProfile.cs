using AutoMapper;
using Kodlama.Io.Devs.Application.Features.Developers.Commands.UpdateDeveloperGitHub;
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

            CreateMap<Developer, DeveloperProfileDto>()
                .ForMember(d => d.Name, opt => opt.MapFrom(n => $"{n.FirstName} {n.LastName}"))
                .ForMember(d => d.GitHubLink, opt => opt.MapFrom(n => $"https://github.com/{n.GitHub}"))
                .ReverseMap();

            CreateMap<Developer, UpdateDeveloperGitHubCommand>()
                .ReverseMap();
        }
    }
}
