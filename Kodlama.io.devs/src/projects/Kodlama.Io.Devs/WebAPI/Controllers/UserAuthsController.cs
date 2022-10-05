using Core.Security.Dtos;
using Core.Security.Entities;
using Kodlama.Io.Devs.Application.Features.DeveloperAuths.Dtos;
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
        public async Task<IActionResult> Login([FromBody] UserForLoginDto userForLoginDto)
        {
            LoginUserAuthCommand loginUserAuthCommand = new()
            {
                UserForLoginDto = userForLoginDto,
                IpAddress = GetIpAddress()
            };

            LoginUserAuthResultDto loginUserAuthResultDto = await Mediator.Send(loginUserAuthCommand);
            SetRefreshTokenToCookie(loginUserAuthResultDto.RefreshToken);
            return Ok(loginUserAuthResultDto.AccessToken);
        }

        private void SetRefreshTokenToCookie(RefreshToken refreshToken)
        {
            CookieOptions cookieOptions = new() { HttpOnly = true, Expires = DateTime.Now.AddDays(7) };
            Response.Cookies.Append("refreshToken", refreshToken.Token, cookieOptions);
        }
    }
}
