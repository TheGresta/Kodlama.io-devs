using AutoMapper;
using Core.Persistence.Paging;
using Kodlama.Io.Devs.Application.Features.Languages.Commands.CreateLanguage;
using Kodlama.Io.Devs.Application.Features.Languages.Commands.DeleteLanguage;
using Kodlama.Io.Devs.Application.Features.Languages.Commands.UpdateLanguage;
using Kodlama.Io.Devs.Application.Features.Languages.Dtos;
using Kodlama.Io.Devs.Application.Features.Languages.Models;
using Kodlama.Io.Devs.Domain.Entities;

namespace Kodlama.Io.Devs.Application.Features.Languages.Profiles
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Language, CreateLanguageCommand>().ReverseMap();
            CreateMap<Language, CreatedLanguageDto>().ReverseMap();

            CreateMap<Language, DeletedLanguageDto>().ReverseMap();

            CreateMap<Language, UpdatedLanguageDto>().ReverseMap();
            CreateMap<Language, UpdateLanguageCommand>().ReverseMap();

            CreateMap<Language, GetByIdLanguageDto>().ReverseMap();

            CreateMap<Language, ListLanguageDto>().ReverseMap();
            CreateMap<IPaginate<Language>, LanguageListModel>().ReverseMap();            
        }
    }
}
