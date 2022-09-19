using AutoMapper;
using Core.Security.Entities;
using Kodlama.Io.Devs.Application.Features.Users.Commands;
using Kodlama.Io.Devs.Application.Features.Users.Dtos;
using Kodlama.Io.Devs.Application.Features.Users.Rules;
using Kodlama.Io.Devs.Application.Services.Repositories;
using MediatR;

namespace Kodlama.Io.Devs.Application.Features.Users.Queries.GetByIdUser
{
    public partial class GetByIdUserQuery : IRequest<CommandUserDto>
    {
        public int UserId { get; set; }

        public class GetByIdUserQueryHandler : IRequestHandler<GetByIdUserQuery, CommandUserDto>
        {
            private readonly IUserRepository _userRepository;
            private readonly IMapper _mapper;
            private readonly UserBusinessRules _userBusinessRules;
            private readonly UserCommandCustomFunctions _userCommandCustomFunctions;

            public GetByIdUserQueryHandler(IUserRepository userRepository, IMapper mapper, 
                                            UserBusinessRules userBusinessRules, 
                                            UserCommandCustomFunctions userCommandCustomFunctions)
            {
                _userRepository = userRepository;
                _mapper = mapper;
                _userBusinessRules = userBusinessRules;
                _userCommandCustomFunctions = userCommandCustomFunctions;
            }
            public async Task<CommandUserDto> Handle(GetByIdUserQuery request, CancellationToken cancellationToken)
            {
                await _userBusinessRules.UserShouldBeExistWhenRequested(request.UserId);

                User? user = await _userRepository.GetAsync(u => u.Id == request.UserId);
                CommandUserDto mappedUserDto = new();

                _userCommandCustomFunctions.SetCommandUserDtoWhenRequested(request.UserId, out mappedUserDto);

                mappedUserDto = _mapper.Map<CommandUserDto>(user);

                return mappedUserDto;
            }
        }
    }
}
