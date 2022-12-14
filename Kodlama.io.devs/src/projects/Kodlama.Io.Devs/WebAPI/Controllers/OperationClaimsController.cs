using Core.Application.Requests;
using Core.Persistence.Dynamic;
using Kodlama.Io.Devs.Application.Features.OperationClaims.Commands.CreateOperationClaim;
using Kodlama.Io.Devs.Application.Features.OperationClaims.Commands.DeleteOperationClaim;
using Kodlama.Io.Devs.Application.Features.OperationClaims.Commands.UpdateOperationClaim;
using Kodlama.Io.Devs.Application.Features.OperationClaims.Dtos;
using Kodlama.Io.Devs.Application.Features.OperationClaims.Models;
using Kodlama.Io.Devs.Application.Features.OperationClaims.Queries.GetByIdOperationClaim;
using Kodlama.Io.Devs.Application.Features.OperationClaims.Queries.GetListOperationClaim;
using Kodlama.Io.Devs.Application.Features.OperationClaims.Queries.GetListOperationClaimByDynamic;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OperationClaimsController : BaseController
    {
        [HttpPost("getlist/bydynamic")]
        public async Task<IActionResult> GetListByDynamic([FromQuery] PageRequest pageRequest, [FromBody] Dynamic dynamic)
        {
            GetListOperationClaimByDynamicQuery getListOperationClaimByDynamicQuery = 
                new () { PageRequest = pageRequest, Dynamic = dynamic };
            OperationClaimListModel result = await Mediator.Send(getListOperationClaimByDynamicQuery);
            return Ok(result);

        }

        [HttpGet("getlist")]
        public async Task<IActionResult> GetList([FromQuery] PageRequest pageRequest)
        {
            GetListOperationClaimQuery getListOperationClaimQuery = new() { PageRequest = pageRequest };
            OperationClaimListModel result = await Mediator.Send(getListOperationClaimQuery);
            return Ok(result);
        }

        [HttpGet("getbyid")]
        public async Task<IActionResult> GetById([FromQuery] GetByIdOperationClaimQuery getByIdOperationClaimQuery)
        {
            GetByIdOperationClaimDto result = await Mediator.Send(getByIdOperationClaimQuery);
            return Ok(result);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] CreateOperationClaimCommand createOperationClaimCommand)
        {
            CreatedOperationClaimDto result = await Mediator.Send(createOperationClaimCommand);
            return Created("", result);
        }

        [HttpPost("update")]
        public async Task<IActionResult> Update([FromBody] UpdateOperationClaimCommand updateOperationClaimCommand)
        {
            UpdatedOperationClaimDto result = await Mediator.Send(updateOperationClaimCommand);
            return Ok(result);
        }

        [HttpGet("delete")]
        public async Task<IActionResult> Delete([FromQuery] DeleteOperationClaimCommand deleteOperationClaimCommand)
        {
            DeletedOperationClaimDto result = await Mediator.Send(deleteOperationClaimCommand);
            return Ok(result);
        }
    }
}
