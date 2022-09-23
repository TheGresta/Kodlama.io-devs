using Kodlama.Io.Devs.Application.Features.DeveloperAuths.Commands.RegisterDeveloperAuth;
using Kodlama.Io.Devs.Application.Features.DeveloperAuths.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeveloperAuthsController : BaseController
    {
        [HttpPost("register/developer")]
        public async Task<IActionResult> Register([FromBody] RegisterDeveloperAuthCommand registerDeveloperAuthCommand)
        {
            RegisterDeveloperAuthResultDto registerDeveloperAuthResultDto = await Mediator.Send(registerDeveloperAuthCommand);
            return Ok(registerDeveloperAuthResultDto);
        }
    }
}
