using Core.Application.Requests;
using Core.Persistence.Dynamic;
using Kodlama.Io.Devs.Application.Features.Users.Commands.UpdateUserEmail;
using Kodlama.Io.Devs.Application.Features.Users.Commands.UpdateUserName;
using Kodlama.Io.Devs.Application.Features.Users.Commands.UpdateUserPassword;
using Kodlama.Io.Devs.Application.Features.Users.Dtos;
using Kodlama.Io.Devs.Application.Features.Users.Models;
using Kodlama.Io.Devs.Application.Features.Users.Queries.GetByEmailUser;
using Kodlama.Io.Devs.Application.Features.Users.Queries.GetByIdUser;
using Kodlama.Io.Devs.Application.Features.Users.Queries.GetByIdUserSelfProfile;
using Kodlama.Io.Devs.Application.Features.Users.Queries.GetListUser;
using Kodlama.Io.Devs.Application.Features.Users.Queries.GetListUserByDynamic;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : BaseController
    {
        [HttpGet("getbyid")]
        public async Task<IActionResult> GetById([FromQuery] GetByIdUserQuery getByIdUserQuery)
        {
            GetByIdUserDto getByIdUserDto = await Mediator.Send(getByIdUserQuery);
            return Ok(getByIdUserDto);
        }

        [HttpPost("getbyemail")]
        public async Task<IActionResult> GetByEmail([FromBody] GetByEmailUserQuery getByEmailUserQuery)
        {
            GetByEmailUserDto getByEmailUserDto = await Mediator.Send(getByEmailUserQuery);
            return Ok(getByEmailUserDto);
        }

        [HttpGet("getlist")]
        public async Task<IActionResult> GetList([FromQuery] GetListUserQuery getListUserQuery)
        {
            UserListModel userListModel = await Mediator.Send(getListUserQuery);
            return Ok(userListModel);
        }

        [HttpPost("getlist/bydynamic")]
        public async Task<IActionResult> GetListByDynamic([FromQuery] PageRequest pageRequest, [FromBody] Dynamic dynamic)
        {
            GetListUserByDynamicQuery getListUserByDynamicQuery = new() { PageRequest = pageRequest, Dynamic = dynamic };
            UserListModel userListModel = await Mediator.Send(getListUserByDynamicQuery);
            return Ok(userListModel);
        }

        [HttpGet("getselfprofile")]
        public async Task<IActionResult> GetSelfProfile()
        {
            GetByIdUserDto getByIdUserDto = await Mediator.Send(new GetByIdUserSelfProfileQuery());
            return Ok(getByIdUserDto);
        }

        [HttpPost("update/selfemail")]
        public async Task<IActionResult> UpdateEmail([FromBody] UpdateUserEmailCommand updateUserEmailCommand)
        {
            UpdatedUserDto updatedUserDto = await Mediator.Send(updateUserEmailCommand);
            return Ok(updatedUserDto);
        }

        [HttpPost("update/selfname")]
        public async Task<IActionResult> UpdateName([FromBody] UpdateUserNameCommand updateUserNameCommand)
        {
            UpdatedUserDto updatedUserDto = await Mediator.Send(updateUserNameCommand);
            return Ok(updatedUserDto);
        }

        [HttpPost("update/selfpassword")]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdateUserPasswordCommand updateUserPasswordCommand)
        {
            UpdatedUserDto updatedUserDto = await Mediator.Send(updateUserPasswordCommand);
            return Ok(updatedUserDto);
        }
    }
}
