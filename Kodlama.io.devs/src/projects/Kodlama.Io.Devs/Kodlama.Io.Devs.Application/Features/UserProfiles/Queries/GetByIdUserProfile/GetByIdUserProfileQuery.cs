using Core.Application.Pipelines.Authorization;
using Core.Security.Extensions;
using Kodlama.Io.Devs.Application.Features.Users.Dtos;
using Kodlama.Io.Devs.Application.Features.Users.Queries.GetByIdUser;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Kodlama.Io.Devs.Application.Features.UserProfiles.Queries.GetByIdUserProfile
{
    public partial class GetByIdUserProfileQuery : IRequest<CommandUserDto>, ISecuredRequest
    {
        public string[] Roles => new[] {"User"};

        public class GetByIdUserProfileQueryHandler : IRequestHandler<GetByIdUserProfileQuery, CommandUserDto>
        {
            private readonly IMediator _mediator;
            private readonly IHttpContextAccessor _httpContextAccessor;

            public GetByIdUserProfileQueryHandler(IMediator mediator, IHttpContextAccessor httpContextAccessor)
            {
                _mediator = mediator;
                _httpContextAccessor = httpContextAccessor;
            }

            public async Task<CommandUserDto> Handle(GetByIdUserProfileQuery request, CancellationToken cancellationToken)
            {
                int userId = _httpContextAccessor.HttpContext.User.GetUserId();

                GetByIdUserQuery getByIdUserQuery = new() { Id = userId };
                CommandUserDto mappedUserDto = await _mediator.Send(getByIdUserQuery, cancellationToken);

                return mappedUserDto;
            }
        }
    }
}
