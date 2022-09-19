using Core.Application.Requests;
using Core.Persistence.Dynamic;
using Kodlama.Io.Devs.Application.Features.UserOperationClaims.Commands.CreateUserOperationClaim;
using Kodlama.Io.Devs.Application.Features.UserOperationClaims.Commands.DeleteUserOperationClaim;
using Kodlama.Io.Devs.Application.Features.UserOperationClaims.Commands.UpdateUserOperationClaim;
using Kodlama.Io.Devs.Application.Features.UserOperationClaims.Dtos;
using Kodlama.Io.Devs.Application.Features.UserOperationClaims.Models;
using Kodlama.Io.Devs.Application.Features.UserOperationClaims.Queries.GetByIdUserOperationClaim;
using Kodlama.Io.Devs.Application.Features.UserOperationClaims.Queries.GetListUserOperationClaim;
using Kodlama.Io.Devs.Application.Features.UserOperationClaims.Queries.GetListUserOperationClaimByDynamic;
using Kodlama.Io.Devs.Application.Features.Users.Commands.CreateUser;
using Kodlama.Io.Devs.Application.Features.Users.Commands.DeleteUser;
using Kodlama.Io.Devs.Application.Features.Users.Commands.UpdateUser;
using Kodlama.Io.Devs.Application.Features.Users.Dtos;
using Kodlama.Io.Devs.Application.Features.Users.Models;
using Kodlama.Io.Devs.Application.Features.Users.Queries.GetByIdUser;
using Kodlama.Io.Devs.Application.Features.Users.Queries.GetListUser;
using Kodlama.Io.Devs.Application.Features.Users.Queries.GetListUserByDynamic;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : BaseController
    {
        [HttpPost("getlist/bydynamic")]
        public async Task<ActionResult> GetListByDynamic([FromQuery] PageRequest pageRequest, [FromBody] Dynamic dynamic)
        {
            GetListUserByDynamicQuery getListByDynamicQuery = new() { PageRequest = pageRequest, Dynamic = dynamic };
            UserListModel result = await Mediator.Send(getListByDynamicQuery);
            return Ok(result);

        }

        [HttpGet("getlist")]
        public async Task<IActionResult> GetList([FromQuery] PageRequest pageRequest)
        {
            GetListUserQuery getListUserQuery = new() { PageRequest = pageRequest };
            UserListModel result = await Mediator.Send(getListUserQuery);
            return Ok(result);
        }

        [HttpGet("getbyid/{Id}")]
        public async Task<IActionResult> GetById([FromRoute] GetByIdUserQuery getByIdUserQuery)
        {
            CommandUserDto result = await Mediator.Send(getByIdUserQuery);
            return Ok(result);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] CreateUserCommand createUserCommand)
        {
            CommandUserDto result = await Mediator.Send(createUserCommand);
            return Created("", result);
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UpdateUserCommand updateUserCommand)
        {
            CommandUserDto result = await Mediator.Send(updateUserCommand);
            return Ok(result);
        }

        [HttpDelete("delete/{Id}")]
        public async Task<IActionResult> Delete([FromRoute] DeleteUserCommand deleteUserCommand)
        {
            CommandUserDto result = await Mediator.Send(deleteUserCommand);
            return Ok(result);
        }
    }
}
