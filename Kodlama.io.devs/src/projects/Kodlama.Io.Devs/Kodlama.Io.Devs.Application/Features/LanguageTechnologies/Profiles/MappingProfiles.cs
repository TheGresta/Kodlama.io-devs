using AutoMapper;
using Core.Persistence.Paging;
using Kodlama.Io.Devs.Application.Features.LanguageTechnologies.Commands.CreateLanguageTechnology;
using Kodlama.Io.Devs.Application.Features.LanguageTechnologies.Commands.UpdateLanguageTechnology;
using Kodlama.Io.Devs.Application.Features.LanguageTechnologies.Dtos;
using Kodlama.Io.Devs.Application.Features.LanguageTechnologies.Models;
using Kodlama.Io.Devs.Domain.Entities;

namespace Kodlama.Io.Devs.Application.Features.LanguageTechnologies.Profiles
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<LanguageTechnology, CreateLanguageTechnologyCommand>().ReverseMap();
            CreateMap<LanguageTechnology, CreatedLanguageTechnologyDto>()
                .ForMember(l => l.LanguageName, opt => opt.MapFrom(src => $"{src.Language.Name}"))
                .IncludeMembers(l => l.Language).ReverseMap();

            CreateMap<LanguageTechnology, DeletedLanguageTechnologyDto>()
                .ForMember(l => l.LanguageName, opt => opt.MapFrom(src => $"{src.Language.Name}"))
                .IncludeMembers(l => l.Language).ReverseMap();

            CreateMap<LanguageTechnology, UpdateLanguageTechnologyCommand>().ReverseMap();
            CreateMap<LanguageTechnology, UpdatedLanguageTechnologyDto>()
                .ForMember(l => l.LanguageName, opt => opt.MapFrom(src => $"{src.Language.Name}"))
                .IncludeMembers(l => l.Language).ReverseMap();

            CreateMap<LanguageTechnology, GetByIdLanguageTechnologyDto>()
                .ForMember(l => l.LanguageName, opt => opt.MapFrom(src => $"{src.Language.Name}"))
                .IncludeMembers(l => l.Language).ReverseMap();

            CreateMap<IPaginate<LanguageTechnology>, LanguageTechnologyListModel>().ReverseMap();
            CreateMap<LanguageTechnology, ListLanguageTechnologyDto>()
                .ForMember(l => l.LanguageName, opt => opt.MapFrom(src => $"{src.Language.Name}"))
                .IncludeMembers(l => l.Language).ReverseMap();
        }
    }
}
