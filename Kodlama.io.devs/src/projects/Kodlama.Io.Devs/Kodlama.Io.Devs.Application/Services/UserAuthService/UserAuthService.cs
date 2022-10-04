using Core.Persistence.Paging;
using Core.Security.Entities;
using Core.Security.JWT;
using Kodlama.Io.Devs.Application.Features.UserOperationClaims.Commands.CreateMultipleUserOperationClaim;
using Kodlama.Io.Devs.Application.Services.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Kodlama.Io.Devs.Application.Services.UserAuthService
{
    public class UserAuthService : IUserAuthService
    {
        private readonly IUserOperationClaimRepository _userOperationClaimRepository;
        private readonly ITokenHelper _tokenHelper;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IMediator _mediator;

        public UserAuthService(IUserOperationClaimRepository userOperationClaimRepository, 
                               ITokenHelper tokenHelper, 
                               IRefreshTokenRepository refreshTokenRepository, 
                               IMediator mediator)
        {
            _userOperationClaimRepository = userOperationClaimRepository;
            _tokenHelper = tokenHelper;
            _refreshTokenRepository = refreshTokenRepository;
            _mediator = mediator;
        }

        public async Task<RefreshToken> AddRefreshToken(RefreshToken refreshToken)
        {
            RefreshToken addedRefreshToken = await _refreshTokenRepository.AddAsync(refreshToken);
            return addedRefreshToken;
        }

        public async Task AddUserOperationClaimsForCreatedUser(int userId, string[] roleNames)
        {
            CreateMultipleUserOperationClaimCommand createMultipleUserOperationClaimCommand = new()
            {
                UserId = userId,
                RoleNames = roleNames
            };

            await _mediator.Send(createMultipleUserOperationClaimCommand);
        }

        public async Task<AccessToken> CreateAccessToken(User user)
        {
            ​IPaginate<UserOperationClaim> userOperationClaims = await _userOperationClaimRepository
                                                                        .GetListAsync(u => u.UserId == user.Id, include:
                                                                                      u => u.Include(u => u.OperationClaim));

            IList<OperationClaim> operationClaims =
            userOperationClaims.Items.Select(u => new OperationClaim
            { Id = u.OperationClaim.Id, Name = u.OperationClaim.Name }).ToList();

            AccessToken accessToken = _tokenHelper.CreateToken(user, operationClaims);
            return accessToken;
        }

        public async Task<RefreshToken> CreateRefreshToken(User user, string ipAddress)
        {
            RefreshToken refreshToken = _tokenHelper.CreateRefreshToken(user, ipAddress);
            return await Task.FromResult(refreshToken);
        }
    }
}
