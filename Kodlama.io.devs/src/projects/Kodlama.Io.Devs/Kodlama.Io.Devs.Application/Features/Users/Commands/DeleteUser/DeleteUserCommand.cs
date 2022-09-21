using AutoMapper;
using Core.Security.Entities;
using Kodlama.Io.Devs.Application.Features.Users.Dtos;
using Kodlama.Io.Devs.Application.Features.Users.Rules;
using Kodlama.Io.Devs.Application.Services.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Kodlama.Io.Devs.Application.Features.Users.Commands.DeleteUser
{
    public partial class DeleteUserCommand : IRequest<CommandUserDto>
    {
        public int Id { get; set; }
        public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, CommandUserDto>
        {
            private readonly IUserRepository _userRepository;
            private readonly IMapper _mapper;
            private readonly UserBusinessRules _userBusinessRules;
            private readonly UserCommandCustomFunctions _userCommandCustomFunctions;

            public DeleteUserCommandHandler(IUserRepository userRepository, IMapper mapper, UserBusinessRules userBusinessRules, UserCommandCustomFunctions userCommandCustomFunctions)
            {
                _userRepository = userRepository;
                _mapper = mapper;
                _userBusinessRules = userBusinessRules;
                _userCommandCustomFunctions = userCommandCustomFunctions;
            }

            public async Task<CommandUserDto> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
            {
                await _userBusinessRules.UserShouldBeExistWhenRequested(request.Id);

                User? user = await _userRepository.GetAsync(u => u.Id == request.Id);
                User deletedUser = await _userRepository.DeleteAsync(user);
                CommandUserDto mappedUserDto = new();

                await _userCommandCustomFunctions.DeleteGitHubAddressWhenUserDeleted(deletedUser.Id);
                await _userCommandCustomFunctions.DeleteAllUserOperationClaimsWhenUserDeleted(deletedUser.Id);

                mappedUserDto = _mapper.Map<CommandUserDto>(deletedUser);
                return mappedUserDto;
            }
        }
    }
}
