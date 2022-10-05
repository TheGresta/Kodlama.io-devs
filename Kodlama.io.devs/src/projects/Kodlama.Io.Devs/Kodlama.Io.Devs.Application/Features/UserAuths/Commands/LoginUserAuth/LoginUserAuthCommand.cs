using AutoMapper;
using Core.Security.Dtos;
using Core.Security.Entities;
using Core.Security.JWT;
using Kodlama.Io.Devs.Application.Features.DeveloperAuths.Dtos;
using Kodlama.Io.Devs.Application.Features.UserAuths.Dtos;
using Kodlama.Io.Devs.Application.Features.UserAuths.Rules;
using Kodlama.Io.Devs.Application.Services.Repositories;
using Kodlama.Io.Devs.Application.Services.UserAuthService;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Kodlama.Io.Devs.Application.Features.UserAuths.Commands.LoginUserAuth
{
    public class LoginUserAuthCommand : IRequest<LoginUserAuthResultDto>
    {
        public UserForLoginDto UserForLoginDto { get; set; }
        public string IpAddress { get; set; }

        public class LoginUserAuthCommandHandler : IRequestHandler<LoginUserAuthCommand, LoginUserAuthResultDto>
        {
            private readonly UserAuthBusinessRules _userAuthBusinessRules;
            private readonly IUserRepository _userRepository;
            private readonly IMediator _mediator;
            private readonly IUserAuthService _userAuthService;

            public LoginUserAuthCommandHandler(UserAuthBusinessRules userAuthBusinessRules,
                                               IUserRepository userRepository,
                                               IMediator mediator,
                                               IUserAuthService userAuthService)
            {
                _userAuthBusinessRules = userAuthBusinessRules;
                _userRepository = userRepository;
                _mediator = mediator;
                _userAuthService = userAuthService;
            }

            public async Task<LoginUserAuthResultDto> Handle(LoginUserAuthCommand request, CancellationToken cancellationToken)
            {
                await _userAuthBusinessRules.EmailShouldBeExistInTheUserTableWhenUserTryingToLogin(request.UserForLoginDto.Email);

                User? user = await _userRepository.GetAsync(u => u.Email.ToLower() == request.UserForLoginDto.Email.ToLower(), 
                                                            include: x => x.Include(u => u.UserOperationClaims).ThenInclude(o => o.OperationClaim));

                await _userAuthBusinessRules.PasswordShouldBeValidWhenUserTryingToLogin(request.UserForLoginDto.Password, user.PasswordHash, user.PasswordSalt);

                AccessToken createdAccessToken = await _userAuthService.CreateAccessToken(user);
                RefreshToken createdRefreshToken = await _userAuthService.CreateRefreshToken(user, request.IpAddress);
                RefreshToken addedRefreshToken = await _userAuthService.AddRefreshToken(createdRefreshToken);

                LoginUserAuthResultDto loginUserAuthResultDto = new()
                {
                    RefreshToken = addedRefreshToken,
                    AccessToken = createdAccessToken
                };

                return loginUserAuthResultDto;
            }
        }
    }
}
