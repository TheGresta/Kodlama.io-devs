using Kodlama.Io.Devs.Application.Features.UserAuths.Commands.LoginUserAuth;
using Kodlama.Io.Devs.Application.Features.UserAuths.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAuthsController : BaseController
    {
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserAuthCommand loginUserAuthCommand)
        {
            LoginUserAuthResultDto loginUserAuthResultDto = await Mediator.Send(loginUserAuthCommand);
            return Ok(loginUserAuthResultDto);
        }
    }
}
