using AutoMapper;
using Core.Application.Pipelines.Authorization;
using Core.Security.Entities;
using Core.Security.Extensions;
using Kodlama.Io.Devs.Application.Features.Users.Dtos;
using Kodlama.Io.Devs.Application.Features.Users.Rules;
using Kodlama.Io.Devs.Application.Services.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Kodlama.Io.Devs.Application.Features.Users.Commands.UpdateUserEmail
{
    public partial class UpdateUserEmailCommand : IRequest<UpdatedUserDto>, ISecuredRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public string[] Roles => new[] { "Admin", "User" };

        public class UpdateUserEmailCommandHandler : IRequestHandler<UpdateUserEmailCommand, UpdatedUserDto>
        {
            private readonly IUserRepository _userRepository;
            private readonly UserBusinessRules _userBusinessRules;
            private readonly IMapper _mapper;
            private readonly IHttpContextAccessor _httpContextAccessor;

            public UpdateUserEmailCommandHandler(IUserRepository userRepository, 
                                                 UserBusinessRules userBusinessRules, 
                                                 IMapper mapper, 
                                                 IHttpContextAccessor httpContextAccessor)
            {
                _userRepository = userRepository;
                _userBusinessRules = userBusinessRules;
                _mapper = mapper;
                _httpContextAccessor = httpContextAccessor;
            }

            public async Task<UpdatedUserDto> Handle(UpdateUserEmailCommand request, CancellationToken cancellationToken)
            {
                int userId = _httpContextAccessor.HttpContext.User.GetUserId();

                await _userBusinessRules.UserShouldBeExistWithGivenUserId(userId);

                User? user = await _userRepository.GetAsync(u => u.Id == userId);

                await _userBusinessRules.ThereShouldBeNoOtherUserWithGivenEmailWhenUserUpdated(userId, request.Email);
                await _userBusinessRules.PasswordShouldBeValidWhenUserTryingToUpdateProfile(request.Password, user.PasswordHash, user.PasswordSalt);

                _mapper.Map(request, user);

                User updatedUser = await _userRepository.UpdateAsync(user);
                UpdatedUserDto mappedUserDto = _mapper.Map<UpdatedUserDto>(updatedUser);

                return mappedUserDto;
            }
        }
    }
}
