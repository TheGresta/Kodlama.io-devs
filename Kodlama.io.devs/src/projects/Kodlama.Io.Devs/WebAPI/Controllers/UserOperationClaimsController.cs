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
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserOperationClaimsController : BaseController
    {
        [HttpPost("getlist/bydynamic")]
        public async Task<IActionResult> GetListByDynamic([FromQuery] PageRequest pageRequest, [FromBody] Dynamic dynamic)
        {
            GetListUserOperationClaimByDynamicQuery getListUserOperationClaimByDynamicQuery =
                new () { PageRequest = pageRequest, Dynamic = dynamic };
            UserOperationClaimListModel result = await Mediator.Send(getListUserOperationClaimByDynamicQuery);
            return Ok(result);

        }

        [HttpGet("getlist")]
        public async Task<IActionResult> GetList([FromQuery] PageRequest pageRequest)
        {
            GetListUserOperationClaimQuery getListUserOperationClaimQuery = new() { PageRequest = pageRequest };
            UserOperationClaimListModel result = await Mediator.Send(getListUserOperationClaimQuery);
            return Ok(result);
        }

        [HttpGet("getbyid/{Id}")]
        public async Task<IActionResult> GetById([FromRoute] GetByIdUserOperationClaimQuery getByIdUserOperationClaimQuery)
        {
            GetByIdUserOperationClaimDto result = await Mediator.Send(getByIdUserOperationClaimQuery);
            return Ok(result);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] CreateUserOperationClaimCommand createUserOperationClaimCommand)
        {
            CreatedUserOperationClaimDto result = await Mediator.Send(createUserOperationClaimCommand);
            return Created("", result);
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UpdateUserOperationClaimCommand updateUserOperationClaimCommand)
        {
            UpdatedUserOperationClaimDto result = await Mediator.Send(updateUserOperationClaimCommand);
            return Ok(result);
        }

        [HttpDelete("delete/{Id}")]
        public async Task<IActionResult> Delete([FromRoute] DeleteUserOperationClaimCommand deleteUserOperationClaimCommand)
        {
            DeletedUserOperationClaimDto result = await Mediator.Send(deleteUserOperationClaimCommand);
            return Ok(result);
        }
    }
}
