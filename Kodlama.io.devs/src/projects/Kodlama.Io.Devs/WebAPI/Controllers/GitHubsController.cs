using Core.Application.Requests;
using Core.Persistence.Dynamic;
using Kodlama.Io.Devs.Application.Features.GitHubs.Commands.CreateGitHub;
using Kodlama.Io.Devs.Application.Features.GitHubs.Commands.DeleteGitHub;
using Kodlama.Io.Devs.Application.Features.GitHubs.Commands.UpdateGitHub;
using Kodlama.Io.Devs.Application.Features.GitHubs.Dtos;
using Kodlama.Io.Devs.Application.Features.GitHubs.Models;
using Kodlama.Io.Devs.Application.Features.GitHubs.Queries.GetByIdGitHub;
using Kodlama.Io.Devs.Application.Features.GitHubs.Queries.GetListGitHub;
using Kodlama.Io.Devs.Application.Features.GitHubs.Queries.GetListGitHubByDynamic;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GitHubsController : BaseController
    {
        [HttpPost("getlist/bydynamic")]
        public async Task<IActionResult> GetListByDynamic([FromQuery] PageRequest pageRequest, [FromBody] Dynamic dynamic)
        {
            GetListGitHubByDynamicQuery getListGitHubByDynamicQuery = new () { PageRequest = pageRequest, Dynamic = dynamic };
            GitHubListModel result = await Mediator.Send(getListGitHubByDynamicQuery);
            return Ok(result);

        }

        [HttpGet("getlist")]
        public async Task<IActionResult> GetList([FromQuery] PageRequest pageRequest)
        {
            GetListGitHubQuery getListGitHubQuery = new() { PageRequest = pageRequest };
            GitHubListModel result = await Mediator.Send(getListGitHubQuery);
            return Ok(result);
        }

        [HttpGet("getbyid/{Id}")]
        public async Task<IActionResult> GetById([FromRoute] GetByIdGitHubQuery getByIdGitHubQuery)
        {
            GetByIdGitHubDto result = await Mediator.Send(getByIdGitHubQuery);
            return Ok(result);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] CreateGitHubCommand createGitHubCommand)
        {
            CreatedGitHubDto result = await Mediator.Send(createGitHubCommand);
            return Created("", result);
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UpdateGitHubCommand updateGitHubCommand)
        {
            UpdatedGitHubDto result = await Mediator.Send(updateGitHubCommand);
            return Ok(result);
        }

        [HttpDelete("delete/{Id}")]
        public async Task<IActionResult> Delete([FromRoute] DeleteGitHubCommand deleteGitHubCommand)
        {
            DeletedGitHubDto result = await Mediator.Send(deleteGitHubCommand);
            return Ok(result);
        }
    }
}
