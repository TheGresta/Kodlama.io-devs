using AutoMapper;
using Core.Security.Entities;
using Core.Security.Extensions;
using Kodlama.Io.Devs.Application.Features.Users.Dtos;
using Kodlama.Io.Devs.Application.Features.Users.Rules;
using Kodlama.Io.Devs.Application.Services.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Kodlama.Io.Devs.Application.Features.Users.Commands.UpdateUserName
{
    public partial class UpdateUserNameCommand : IRequest<UpdatedUserDto>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }

        public class UpdateUserEmailCommandHandler : IRequestHandler<UpdateUserNameCommand, UpdatedUserDto>
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

            public async Task<UpdatedUserDto> Handle(UpdateUserNameCommand request, CancellationToken cancellationToken)
            {
                int userId = _httpContextAccessor.HttpContext.User.GetUserId();

                await _userBusinessRules.UserShouldBeExistWithGivenUserId(userId);

                User? user = await _userRepository.GetAsync(u => u.Id == userId);

                await _userBusinessRules.PasswordShouldBeValidWhenUserTryingToUpdateProfile(request.Password, user.PasswordHash, user.PasswordSalt);

                user.FirstName = request.FirstName;
                user.LastName = request.LastName;

                User updatedUser = await _userRepository.UpdateAsync(user);
                UpdatedUserDto mappedUserDto = _mapper.Map<UpdatedUserDto>(updatedUser);

                return mappedUserDto;
            }
        }
    }
}
