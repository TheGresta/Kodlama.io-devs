using Kodlama.Io.Devs.Application.Features.UserProfiles.Commands.UpdateUserProfile;
using Kodlama.Io.Devs.Application.Features.UserProfiles.Queries.GetByIdUserProfile;
using Kodlama.Io.Devs.Application.Features.Users.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfilesController : BaseController
    {
        [HttpGet("getbyid")]
        public async Task<IActionResult> GetById()
        {
            GetByIdUserProfileQuery request = new();
            CommandUserDto result = await Mediator.Send(request);

            return Ok(result);
        }

        [HttpPost("update")]
        public async Task<IActionResult> Update([FromBody] UpdateUserProfileCommand updateUserProfileCommand)
        {
            CommandUserDto result = await Mediator.Send(updateUserProfileCommand);
            return Ok(result);
        }
    }
}
