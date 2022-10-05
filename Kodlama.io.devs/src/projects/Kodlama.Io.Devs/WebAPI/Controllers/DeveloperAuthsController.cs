using Core.Security.Dtos;
using Core.Security.Entities;
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
        public async Task<IActionResult> Register([FromBody] UserForRegisterDto userForRegisterDto)
        {
            RegisterDeveloperAuthCommand registerDeveloperAuthCommand = new() 
            { 
                UserForRegisterDto = userForRegisterDto,
                IpAddress = GetIpAddress()
            };

            RegisterDeveloperAuthResultDto registerDeveloperAuthResultDto = await Mediator.Send(registerDeveloperAuthCommand);
            SetRefreshTokenToCookie(registerDeveloperAuthResultDto.RefreshToken);
            return Ok(registerDeveloperAuthResultDto.AccessToken);
        }

        private void SetRefreshTokenToCookie(RefreshToken refreshToken)
        {
            CookieOptions cookieOptions = new() { HttpOnly = true, Expires = DateTime.Now.AddDays(7) };
            Response.Cookies.Append("refreshToken", refreshToken.Token, cookieOptions);
        }
    }
}
