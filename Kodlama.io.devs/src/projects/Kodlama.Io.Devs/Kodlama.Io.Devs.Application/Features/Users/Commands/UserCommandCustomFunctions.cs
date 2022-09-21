using AutoMapper;
using Core.Persistence.Paging;
using Core.Security.Dtos;
using Core.Security.Entities;
using Core.Security.Enums;
using Core.Security.Hashing;
using Kodlama.Io.Devs.Application.Features.GitHubs.Commands.CreateGitHub;
using Kodlama.Io.Devs.Application.Features.GitHubs.Commands.UpdateGitHub;
using Kodlama.Io.Devs.Application.Features.UserOperationClaims.Commands.CreateUserOperationClaim;
using Kodlama.Io.Devs.Application.Features.UserOperationClaims.Models;
using Kodlama.Io.Devs.Application.Features.Users.Dtos;
using Kodlama.Io.Devs.Application.Features.Users.Models;
using Kodlama.Io.Devs.Application.Services.Repositories;
using Kodlama.Io.Devs.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Kodlama.Io.Devs.Application.Features.Users.Commands
{
    public class UserCommandCustomFunctions
    {
        private readonly IMediator _mediator;
        private readonly IGitHubRepository _gitHubRepository;
        private readonly IUserOperationClaimRepository _userOperationClaimRepository;
        private readonly IMapper _mapper;

        public UserCommandCustomFunctions(IMediator mediator, IGitHubRepository gitHubRepository,
                                IUserOperationClaimRepository userOperationClaimRepository, IMapper mapper)
        {
            _mediator = mediator;
            _gitHubRepository = gitHubRepository;
            _userOperationClaimRepository = userOperationClaimRepository;
            _mapper = mapper;
        }

        public async Task CreateOrUpdateGitHubAsync(int userId, string name, CancellationToken cancellationToken)
        {
            GitHub? gitHub = _gitHubRepository.Get(g => g.UserId == userId);

            if (name == null | name == "")
            {
                await _gitHubRepository.DeleteAsync(gitHub);
            }
            else if (gitHub == null)
            {
                CreateGitHubCommand createGitHubCommand = new() { Name = name, UserId = userId };
                await _mediator.Send(createGitHubCommand, cancellationToken);
            }            
            else if (gitHub.Name != name)
            {
                UpdateGitHubCommand updateGitHubCommand = new() { Id = gitHub.Id, Name = name, UserId = userId };
                await _mediator.Send(updateGitHubCommand, cancellationToken);
            }
        }

        public async Task CreateOrUpdateOperationClaimsAsync(IList<int> operationClaimIdList, int userId, CancellationToken cancellationToken)
        {
            IPaginate<UserOperationClaim> userOperationClaims = _userOperationClaimRepository.GetList(u => u.UserId == userId);

            foreach (UserOperationClaim userOperationClaim in userOperationClaims.Items)
            {
                await _userOperationClaimRepository.DeleteAsync(userOperationClaim);
            }

            foreach (int operationClaimId in operationClaimIdList)
            {
                CreateUserOperationClaimCommand createUserOperationClaimCommand = new() { UserId = userId, OperationClaimId = operationClaimId };
                await _mediator.Send(createUserOperationClaimCommand, cancellationToken);
            }
        }

        public async Task DeleteGitHubAddressWhenUserDeleted(int userId)
        {
            GitHub? gitHub = _gitHubRepository.Get(g => g.UserId == userId);

            if (gitHub != null) await _gitHubRepository.DeleteAsync(gitHub);
        }

        public async Task DeleteAllUserOperationClaimsWhenUserDeleted(int userId)
        {
            IPaginate<UserOperationClaim> userOperationClaims = _userOperationClaimRepository.GetList(u => u.UserId == userId);

            foreach (UserOperationClaim userOperationClaim in userOperationClaims.Items)
            {
                await _userOperationClaimRepository.DeleteAsync(userOperationClaim);
            }
        }

        public void SetUserPasswordWhenUserCreated(UserForRegisterDto userForRegisterDto, out User user)
        {
            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(userForRegisterDto.Password, out passwordHash, out passwordSalt);

            user = _mapper.Map<User>(userForRegisterDto);
            user.Status = true;
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.AuthenticatorType = AuthenticatorType.Email;
        }

        public void SetUserPasswordWhenUserUpdated(UserForRegisterDto userForRegisterDto, ref User user)
        {
            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(userForRegisterDto.Password, out passwordHash, out passwordSalt);

            _mapper.Map(userForRegisterDto, user);
            user.Status = true;
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.AuthenticatorType = AuthenticatorType.Email;
        }

        public void SetCommandUserDtoWhenRequested(int userId, ref CommandUserDto commadUserDto)
        {
            GitHub? gitHub = _gitHubRepository.Get(g => g.UserId == userId);
            commadUserDto.GitHubLink = $"github.com/{gitHub.Name}";

            IPaginate<UserOperationClaim> userOperationClaims = 
                        _userOperationClaimRepository.GetList(u => u.UserId == userId, include: x => x.Include(g => g.OperationClaim));

            commadUserDto.UserOperationClaims.Clear();

            foreach (UserOperationClaim uop in userOperationClaims.Items)
            {
                commadUserDto.UserOperationClaims.Add(uop.OperationClaim.Name);
            }
        }

        public void SetCommandUserDtoWhenGetListRequested(IPaginate<User> userList, ref UserListModel userListModel)
        {
            for (int i = 0; i < userList.Items.Count; i++)
            {
                GitHub? gitHub = _gitHubRepository.Get(g => g.UserId == userList.Items[i].Id);
                string GitHubLink = $"github.com/{gitHub.Name}";
                userListModel.Items.ElementAt(i).GitHubLink = GitHubLink;

                IPaginate<UserOperationClaim> userOperationClaims = _userOperationClaimRepository
                    .GetList(u => u.UserId == userList.Items[i].Id, include: x => x.Include(g => g.OperationClaim));

                foreach (UserOperationClaim uop in userOperationClaims.Items)
                {
                    userListModel.Items.ElementAt(i).UserOperationClaims.Add(uop.OperationClaim.Name);
                }
            }
        }


    }
}
