using AutoMapper;
using Core.Persistence.Paging;
using Kodlama.Io.Devs.Application.Features.GitHubs.Dtos;
using Kodlama.Io.Devs.Application.Features.GitHubs.Models;
using Kodlama.Io.Devs.Domain.Entities;

namespace Kodlama.Io.Devs.Application.Features.GitHubs.Profiles
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<GitHub, UpdatedGitHubDto>().ReverseMap();
            CreateMap<GitHub, CreatedGitHubDto>()
                .ForMember(g => g.UserName, opt => opt.MapFrom(src => $"{src.User.FirstName} {src.User.LastName}"))
                .ForMember(g => g.UserMail, opt => opt.MapFrom(src => $"{src.User.Email}"))
                .IncludeMembers(g => g.User).ReverseMap();

            CreateMap<GitHub, DeletedGitHubDto>()
                .ForMember(g => g.UserName, opt => opt.MapFrom(src => $"{src.User.FirstName} {src.User.LastName}"))
                .ForMember(g => g.UserMail, opt => opt.MapFrom(src => $"{src.User.Email}"))
                .IncludeMembers(g => g.User).ReverseMap();

            CreateMap<GitHub, UpdatedGitHubDto>().ReverseMap();
            CreateMap<GitHub, UpdatedGitHubDto>()
                .ForMember(g => g.UserName, opt => opt.MapFrom(src => $"{src.User.FirstName} {src.User.LastName}"))
                .ForMember(g => g.UserMail, opt => opt.MapFrom(src => $"{src.User.Email}"))
                .IncludeMembers(g => g.User).ReverseMap();

            CreateMap<GitHub, GetByIdGitHubDto>()
                .ForMember(g => g.UserName, opt => opt.MapFrom(src => $"{src.User.FirstName} {src.User.LastName}"))
                .ForMember(g => g.UserMail, opt => opt.MapFrom(src => $"{src.User.Email}"))
                .IncludeMembers(g => g.User).ReverseMap();

            CreateMap<IPaginate<GitHub>, GitHubListModel>().ReverseMap();
            CreateMap<GitHub, ListGitHubDto>()
                .ForMember(g => g.UserName, opt => opt.MapFrom(src => $"{src.User.FirstName} {src.User.LastName}"))
                .ForMember(g => g.UserMail, opt => opt.MapFrom(src => $"{src.User.Email}"))
                .IncludeMembers(g => g.User).ReverseMap();
        }
    }
}
