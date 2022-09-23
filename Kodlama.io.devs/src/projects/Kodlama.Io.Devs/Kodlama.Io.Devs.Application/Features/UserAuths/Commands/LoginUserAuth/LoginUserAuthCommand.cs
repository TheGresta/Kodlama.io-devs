using AutoMapper;
using Core.Security.Dtos;
using Core.Security.Entities;
using Kodlama.Io.Devs.Application.Features.UserAuths.Commands.CreateAccessTokeUserAuth;
using Kodlama.Io.Devs.Application.Features.UserAuths.Dtos;
using Kodlama.Io.Devs.Application.Features.UserAuths.Rules;
using Kodlama.Io.Devs.Application.Services.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Kodlama.Io.Devs.Application.Features.UserAuths.Commands.LoginUserAuth
{
    public class LoginUserAuthCommand : IRequest<LoginUserAuthResultDto>
    {
        public UserForLoginDto UserForLoginDto { get; set; }

        public class LoginUserAuthCommandHandler : IRequestHandler<LoginUserAuthCommand, LoginUserAuthResultDto>
        {
            private readonly IMapper _mapper;
            private readonly UserAuthBusinessRules _userAuthBusinessRules;
            private readonly IUserRepository _userRepository;
            private readonly IMediator _mediator;

            public LoginUserAuthCommandHandler(IMapper mapper, UserAuthBusinessRules userAuthBusinessRules, IUserRepository userRepository, IMediator mediator)
            {
                _mapper = mapper;
                _userAuthBusinessRules = userAuthBusinessRules;
                _userRepository = userRepository;
                _mediator = mediator;
            }

            public async Task<LoginUserAuthResultDto> Handle(LoginUserAuthCommand request, CancellationToken cancellationToken)
            {
                await _userAuthBusinessRules.EmailShouldBeExistInTheUserTableWhenUserTryingToLogin(request.UserForLoginDto.Email);

                User? user = await _userRepository.GetAsync(u => u.Email.ToLower() == request.UserForLoginDto.Email.ToLower(), 
                                                            include: x => x.Include(u => u.UserOperationClaims).ThenInclude(o => o.OperationClaim));

                await _userAuthBusinessRules.PasswordShouldBeValidWhenUserTryingToLogin(request.UserForLoginDto.Password, user.PasswordHash, user.PasswordSalt);

                CreateAccessTokenUserAuthCommand createAccessTokenUserAuthCommand = new() { User = user };
                CreateAccessTokenUserAuthResultDto createAccessTokenUserAuthResultDto = await _mediator.Send(createAccessTokenUserAuthCommand);
                LoginUserAuthResultDto loginUserAuthResultDto = _mapper.Map<LoginUserAuthResultDto>(createAccessTokenUserAuthResultDto);

                return loginUserAuthResultDto;
            }
        }
    }
}
