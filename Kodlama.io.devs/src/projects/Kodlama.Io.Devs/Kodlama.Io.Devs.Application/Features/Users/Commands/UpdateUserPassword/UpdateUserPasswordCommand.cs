using AutoMapper;
using Core.Security.Entities;
using Core.Security.Extensions;
using Core.Security.Hashing;
using Kodlama.Io.Devs.Application.Features.Users.Dtos;
using Kodlama.Io.Devs.Application.Features.Users.Rules;
using Kodlama.Io.Devs.Application.Services.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Kodlama.Io.Devs.Application.Features.Users.Commands.UpdateUserPassword
{
    public partial class UpdateUserPasswordCommand : IRequest<UpdatedUserDto>
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }

        public class UpdateUserPasswordCommandHandler : IRequestHandler<UpdateUserPasswordCommand, UpdatedUserDto>
        {
            private readonly IUserRepository _userRepository;
            private readonly UserBusinessRules _userBusinessRules;
            private readonly IMapper _mapper;
            private readonly IHttpContextAccessor _httpContextAccessor;

            public UpdateUserPasswordCommandHandler(IUserRepository userRepository,
                                                    UserBusinessRules userBusinessRules,
                                                     IMapper mapper,
                                                     IHttpContextAccessor httpContextAccessor)
            {
                _userRepository = userRepository;
                _userBusinessRules = userBusinessRules;
                _mapper = mapper;
                _httpContextAccessor = httpContextAccessor;
            }

            public async Task<UpdatedUserDto> Handle(UpdateUserPasswordCommand request, CancellationToken cancellationToken)
            {
                int userId = _httpContextAccessor.HttpContext.User.GetUserId();

                await _userBusinessRules.UserShouldBeExistWithGivenUserId(userId);

                User? user = await _userRepository.GetAsync(u => u.Id == userId);

                await _userBusinessRules.PasswordShouldBeValidWhenUserTryingToUpdateProfile(request.OldPassword, user.PasswordHash, user.PasswordSalt);

                byte[] passwordHash, passwordSald;
                HashingHelper.CreatePasswordHash(request.NewPassword, out passwordHash, out passwordSald);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSald;

                User updatedUser = await _userRepository.UpdateAsync(user);
                UpdatedUserDto mappedUserDto = _mapper.Map<UpdatedUserDto>(updatedUser);

                return mappedUserDto;
            }
        }
    }
}
