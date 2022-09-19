using Core.Security.Dtos;
using Core.Security.JWT;
using Kodlama.Io.Devs.Application.Features.Authorizations.Commands.Login;
using Kodlama.Io.Devs.Application.Features.Authorizations.Commands.Register;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationsController : BaseController
    {
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserForLoginDto userForLoginDto)
        {
            LoginCommand loginCommand = new() { UserForLoginDto = userForLoginDto };
            AccessToken result = await Mediator.Send(loginCommand);

            return Ok(result);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Registed([FromBody] UserForRegisterDto userForRegisterDto)
        {
            RegisterCommand registerCommand = new() { UserForRegisterDto = userForRegisterDto };
            AccessToken result = await Mediator.Send(registerCommand);

            return Ok(result);
        }
    }
}
