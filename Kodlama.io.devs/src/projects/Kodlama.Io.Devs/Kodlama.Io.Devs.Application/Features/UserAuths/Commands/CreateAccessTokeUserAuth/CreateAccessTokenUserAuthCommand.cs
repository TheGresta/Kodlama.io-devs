using AutoMapper;
using Core.Security.Entities;
using Core.Security.JWT;
using Kodlama.Io.Devs.Application.Features.UserAuths.Dtos;
using MediatR;

namespace Kodlama.Io.Devs.Application.Features.UserAuths.Commands.CreateAccessTokeUserAuth
{
    public class CreateAccessTokenUserAuthCommand : IRequest<CreateAccessTokenUserAuthResultDto>
    {
        public User User { get; set; }

        public class CreateAccessTokenUserAuthCommandHandler : IRequestHandler<CreateAccessTokenUserAuthCommand, CreateAccessTokenUserAuthResultDto>
        {
            private readonly ITokenHelper _tokenHelper;
            private readonly IMapper _mapper;

            public CreateAccessTokenUserAuthCommandHandler(ITokenHelper tokenHelper, IMapper mapper)
            {
                _tokenHelper = tokenHelper;
                _mapper = mapper;
            }

            public async Task<CreateAccessTokenUserAuthResultDto> Handle(CreateAccessTokenUserAuthCommand request, CancellationToken cancellationToken)
            {
                List<OperationClaim> operationClaims = request.User.UserOperationClaims.Select(o => o.OperationClaim).ToList();
                AccessToken accessToken = _tokenHelper.CreateToken(request.User, operationClaims);
                CreateAccessTokenUserAuthResultDto createAccessTokenUserAuthResultDto = _mapper.Map<CreateAccessTokenUserAuthResultDto>(accessToken);

                return createAccessTokenUserAuthResultDto;
            }
        }
    }
}
