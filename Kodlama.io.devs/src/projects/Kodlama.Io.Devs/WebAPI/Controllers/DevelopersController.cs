using Kodlama.Io.Devs.Application.Features.Developers.Commands.UpdateDeveloperGitHub;
using Kodlama.Io.Devs.Application.Features.Developers.Dtos;
using Kodlama.Io.Devs.Application.Features.Developers.Queries.GetByIdDeveloperSelfProfile;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DevelopersController : BaseController
    {
        [HttpGet("getselfprofile")]
        public async Task<IActionResult> GetSelfProfile()
        {
            DeveloperProfileDto developerProfileDto = await Mediator.Send(new GetByIdDeveloperSelfProfileQuery());
            return Ok(developerProfileDto);
        }

        [HttpGet("delete/github")]
        public async Task<IActionResult> DeleteGitHub()
        {
            DeveloperProfileDto developerProfileDto = await Mediator.Send(new UpdateDeveloperGitHubCommand() { GitHub = "" });
            return Ok(developerProfileDto);
        }

        [HttpPost("addorupdate/github")]
        public async Task<IActionResult> AddOrUpdateGitHub([FromBody] UpdateDeveloperGitHubCommand updateDeveloperGitHubCommand)
        {
            DeveloperProfileDto developerProfileDto = await Mediator.Send(updateDeveloperGitHubCommand);
            return Ok(developerProfileDto);
        }
    }
}
