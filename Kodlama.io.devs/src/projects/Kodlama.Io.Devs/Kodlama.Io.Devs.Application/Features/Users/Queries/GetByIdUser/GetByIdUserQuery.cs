using AutoMapper;
using Core.Application.Pipelines.Authorization;
using Core.Security.Entities;
using Kodlama.Io.Devs.Application.Features.Users.Dtos;
using Kodlama.Io.Devs.Application.Features.Users.Rules;
using Kodlama.Io.Devs.Application.Services.Repositories;
using MediatR;

namespace Kodlama.Io.Devs.Application.Features.Users.Queries.GetByIdUser
{
    public partial class GetByIdUserQuery : IRequest<CommandUserDto>, ISecuredRequest
    {
        public int Id { get; set; }

        public string[] Roles => new[] { "Admin" };

        public class GetByIdUserQueryHandler : IRequestHandler<GetByIdUserQuery, CommandUserDto>
        {
            private readonly IUserRepository _userRepository;
            private readonly IMapper _mapper;
            private readonly UserBusinessRules _userBusinessRules;
            private readonly UserCustomFunctions _userCustomFunctions;

            public GetByIdUserQueryHandler(IUserRepository userRepository, IMapper mapper, 
                                            UserBusinessRules userBusinessRules, 
                                            UserCustomFunctions userCommandCustomFunctions)
            {
                _userRepository = userRepository;
                _mapper = mapper;
                _userBusinessRules = userBusinessRules;
                _userCustomFunctions = userCommandCustomFunctions;
            }
            public async Task<CommandUserDto> Handle(GetByIdUserQuery request, CancellationToken cancellationToken)
            {
                await _userBusinessRules.UserShouldBeExistWhenRequested(request.Id);

                User? user = await _userRepository.GetAsync(u => u.Id == request.Id);
                CommandUserDto mappedUserDto = _mapper.Map<CommandUserDto>(user);
                _userCustomFunctions.SetCommandUserDtoWhenRequested(request.Id, ref mappedUserDto);                

                return mappedUserDto;
            }
        }
    }
}
