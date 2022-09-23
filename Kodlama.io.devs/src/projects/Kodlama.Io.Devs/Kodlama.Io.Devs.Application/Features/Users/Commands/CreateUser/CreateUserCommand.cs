using AutoMapper;
using Core.Security.Dtos;
using Core.Security.Entities;
using Core.Security.Enums;
using Core.Security.Hashing;
using Kodlama.Io.Devs.Application.Features.Users.Dtos;
using Kodlama.Io.Devs.Application.Features.Users.Rules;
using Kodlama.Io.Devs.Application.Services.Repositories;
using MediatR;

namespace Kodlama.Io.Devs.Application.Features.Users.Commands.CreateUser
{
    public partial class CreateUserCommand : IRequest<CreatedUserDto>
    {
        public UserForRegisterDto UserForRegisterDto { get; set; }

        public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, CreatedUserDto>
        {
            private readonly IUserRepository _userRepository;
            private readonly IMapper _mapper;            
            private readonly UserBusinessRules _userBusinessRules;

            public CreateUserCommandHandler(IUserRepository userRepository, 
                                            IMapper mapper,
                                            UserBusinessRules userBusinessRules)
            {
                _userRepository = userRepository;
                _mapper = mapper;
                _userBusinessRules = userBusinessRules;
            }

            public async Task<CreatedUserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
            {
                await _userBusinessRules.UserEmailCanNotBeDuplicatedWhenInserted(request.UserForRegisterDto.Email);

                User user = new();
                SetUserPasswordWhenUserCreated(request.UserForRegisterDto, out user);

                User addedUser = await _userRepository.AddAsync(user);
                CreatedUserDto mappedUserDto = _mapper.Map<CreatedUserDto>(addedUser);

                return mappedUserDto;
            }

            private void SetUserPasswordWhenUserCreated(UserForRegisterDto userForRegisterDto, out User user)
            {
                byte[] passwordHash, passwordSalt;
                HashingHelper.CreatePasswordHash(userForRegisterDto.Password, out passwordHash, out passwordSalt);

                user = _mapper.Map<User>(userForRegisterDto);
                user.Status = true;
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
                user.AuthenticatorType = AuthenticatorType.Email;
            }
        }
    }
}
