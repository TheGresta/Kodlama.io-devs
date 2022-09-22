using Core.Application.Pipelines.Authorization;
using Core.Security.Dtos;
using Core.Security.Extensions;
using Kodlama.Io.Devs.Application.Features.Users.Commands.UpdateUser;
using Kodlama.Io.Devs.Application.Features.Users.Dtos;
using Kodlama.Io.Devs.Application.Services.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Kodlama.Io.Devs.Application.Features.UserProfiles.Commands.UpdateUserProfile
{
    public partial class UpdateUserProfileCommand : IRequest<CommandUserDto>, ISecuredRequest
    {
        public UserForRegisterDto UserForRegister { get; set; }
        public string GitHubLink { get; set; }
        public string[] Roles => new[] { "User" };
        public class UpdateUserProfileCommandHandler : IRequestHandler<UpdateUserProfileCommand, CommandUserDto>
        {
            private readonly IMediator _mediator;
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly IOperationClaimRepository _operationClaimRepository;

            public UpdateUserProfileCommandHandler(IMediator mediator, 
                                                  IHttpContextAccessor httpContextAccessor, 
                                                  IOperationClaimRepository operationClaimRepository)
            {
                _mediator = mediator;
                _httpContextAccessor = httpContextAccessor;
                _operationClaimRepository = operationClaimRepository;                
            }

            public async Task<CommandUserDto> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
            {
                int userId = _httpContextAccessor.HttpContext.User.GetUserId();
                IList<int> OperationClaimIdList = new List<int>() { _operationClaimRepository.Get(o => o.Name == "User").Id };

                UpdateUserCommand updateUserCommand = new() { Id = userId, 
                                                              UserForRegisterDto = request.UserForRegister, 
                                                              GitHubName = request.GitHubLink, 
                                                              OperationClaimIdList = OperationClaimIdList };

                CommandUserDto mappedUserDto = await _mediator.Send(updateUserCommand, cancellationToken);

                return mappedUserDto;
            }
        }
    }
}
