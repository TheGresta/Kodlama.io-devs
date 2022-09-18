using AutoMapper;
using Core.Persistence.Paging;
using Core.Security.Entities;
using Kodlama.Io.Devs.Application.Features.UserOperationClaims.Commands.CreateUserOperationClaim;
using Kodlama.Io.Devs.Application.Features.UserOperationClaims.Commands.UpdateUserOperationClaim;
using Kodlama.Io.Devs.Application.Features.UserOperationClaims.Dtos;
using Kodlama.Io.Devs.Application.Features.UserOperationClaims.Models;

namespace Kodlama.Io.Devs.Application.Features.UserOperationClaims.Profiles
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<UserOperationClaim, CreateUserOperationClaimCommand>().ReverseMap();
            CreateMap<UserOperationClaim, CreatedUserOperationClaimDto>()
                .ForMember(u => u.UserName, opt => opt.MapFrom(src => $"{ src.User.FirstName } {src.User.LastName}"))
                .ForMember(u => u.OperationClaimName, opt => opt.MapFrom(src => $"{src.OperationClaim.Name}"))
                .IncludeMembers(u => u.User, u => u.OperationClaim).ReverseMap();

            CreateMap<UserOperationClaim, DeletedUserOperationClaimDto>()
                .ForMember(u => u.UserName, opt => opt.MapFrom(src => $"{src.User.FirstName} {src.User.LastName}"))
                .ForMember(u => u.OperationClaimName, opt => opt.MapFrom(src => $"{src.OperationClaim.Name}"))
                .IncludeMembers(u => u.User, u => u.OperationClaim).ReverseMap();

            CreateMap<UserOperationClaim, UpdateUserOperationClaimCommand>().ReverseMap();
            CreateMap<UserOperationClaim, UpdatedUserOperationClaimDto>()
                .ForMember(u => u.UserName, opt => opt.MapFrom(src => $"{src.User.FirstName} {src.User.LastName}"))
                .ForMember(u => u.OperationClaimName, opt => opt.MapFrom(src => $"{src.OperationClaim.Name}"))
                .IncludeMembers(u => u.User, u => u.OperationClaim).ReverseMap();

            CreateMap<UserOperationClaim, GetByIdUserOperationClaimDto>()
                .ForMember(u => u.UserName, opt => opt.MapFrom(src => $"{src.User.FirstName} {src.User.LastName}"))
                .ForMember(u => u.OperationClaimName, opt => opt.MapFrom(src => $"{src.OperationClaim.Name}"))
                .IncludeMembers(u => u.User, u => u.OperationClaim).ReverseMap();

            CreateMap<IPaginate<UserOperationClaim>, UserOperationClaimListModel>().ReverseMap();
            CreateMap<UserOperationClaim, ListUserOperationClaimDto>()
                .ForMember(u => u.UserName, opt => opt.MapFrom(src => $"{src.User.FirstName} {src.User.LastName}"))
                .ForMember(u => u.OperationClaimName, opt => opt.MapFrom(src => $"{src.OperationClaim.Name}"))
                .IncludeMembers(u => u.User, u => u.OperationClaim).ReverseMap();            
        }
    }
}
