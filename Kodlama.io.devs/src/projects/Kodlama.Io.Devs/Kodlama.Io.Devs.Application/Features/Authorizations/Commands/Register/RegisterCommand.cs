using AutoMapper;
using Core.Security.Dtos;
using Core.Security.Entities;
using Core.Security.JWT;
using Kodlama.Io.Devs.Application.Features.Authorizations.Rules;
using Kodlama.Io.Devs.Application.Features.Users.Commands.CreateUser;
using Kodlama.Io.Devs.Application.Services.Repositories;
using MediatR;

namespace Kodlama.Io.Devs.Application.Features.Authorizations.Commands.Register
{
    public partial class RegisterCommand : IRequest<AccessToken>
    {
        public UserForRegisterDto UserForRegisterDto { get; set; }
        public string? GitHubName { get; set; }

        public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AccessToken>
        {
            private readonly AuthorizationBusinessRules _authorizationBusinessRules;
            private readonly IMediator _mediator;
            private readonly IOperationClaimRepository _operationClaimRepository;

            public RegisterCommandHandler(AuthorizationBusinessRules authorizationBusinessRules, IMediator mediator, IOperationClaimRepository operationClaimRepository)
            {
                _authorizationBusinessRules = authorizationBusinessRules;
                _mediator = mediator;
                _operationClaimRepository = operationClaimRepository;
            }
            public async Task<AccessToken> Handle(RegisterCommand request, CancellationToken cancellationToken)
            {
                await _authorizationBusinessRules.UserShouldNotExistsWhenRegister(request.UserForRegisterDto.Email);

                IList<int> operationClaimIdList = new List<int>() { _operationClaimRepository.Get(o => o.Name == "User").Id };
                CreateUserCommand createUserCommand = new() { UserForRegisterDto = request.UserForRegisterDto, 
                                                              GitHubName = request.GitHubName, 
                                                              OperationClaimIdList = operationClaimIdList };

                AccessToken accessToken = await _mediator.Send(createUserCommand, cancellationToken);
                return accessToken;
            }
        }
    }
}
