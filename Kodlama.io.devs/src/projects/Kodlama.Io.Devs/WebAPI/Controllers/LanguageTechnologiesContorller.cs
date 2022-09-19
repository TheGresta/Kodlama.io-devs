using Core.Application.Requests;
using Core.Persistence.Dynamic;
using Kodlama.Io.Devs.Application.Features.LanguageTechnologies.Commands.CreateLanguageTechnology;
using Kodlama.Io.Devs.Application.Features.LanguageTechnologies.Commands.DeleteLanguageTechnology;
using Kodlama.Io.Devs.Application.Features.LanguageTechnologies.Commands.UpdateLanguageTechnology;
using Kodlama.Io.Devs.Application.Features.LanguageTechnologies.Dtos;
using Kodlama.Io.Devs.Application.Features.LanguageTechnologies.Models;
using Kodlama.Io.Devs.Application.Features.LanguageTechnologies.Queries.GetByIdLanguageTechnology;
using Kodlama.Io.Devs.Application.Features.LanguageTechnologies.Queries.GetListLanguageTechnology;
using Kodlama.Io.Devs.Application.Features.LanguageTechnologies.Queries.GetListLanguageTechnologyByDynamic;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LanguageTechnologiesContorller : BaseController
    {
        [HttpPost("getlist/bydynamic")]
        public async Task<ActionResult> GetListByDynamic([FromQuery] PageRequest pageRequest, [FromBody] Dynamic dynamic)
        {
            GetListLanguageTechnologyByDynamicQuery getListLanguageTechnologyByDynamicQuery = new() { PageRequest = pageRequest, Dynamic = dynamic };
            LanguageTechnologyListModel result = await Mediator.Send(getListLanguageTechnologyByDynamicQuery);
            return Ok(result);

        }

        [HttpGet("getlist")]
        public async Task<IActionResult> GetList([FromQuery] PageRequest pageRequest)
        {
            GetListLanguageTechnologyQuery getListLanguageTechnologyQuery = new() { PageRequest = pageRequest };
            LanguageTechnologyListModel result = await Mediator.Send(getListLanguageTechnologyQuery);
            return Ok(result);
        }

        [HttpGet("getbyid/{Id}")]
        public async Task<IActionResult> GetById([FromRoute] GetByIdLanguageTechnologyQuery getByIdLanguageTechnologyQuery)
        {
            GetByIdLanguageTechnologyDto result = await Mediator.Send(getByIdLanguageTechnologyQuery);
            return Ok(result);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] CreateLanguageTechnologyCommand createLanguageTechnologyCommand)
        {
            CreatedLanguageTechnologyDto result = await Mediator.Send(createLanguageTechnologyCommand);
            return Created("", result);
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UpdateLanguageTechnologyCommand updateLanguageTechnologyCommand)
        {
            UpdatedLanguageTechnologyDto result = await Mediator.Send(updateLanguageTechnologyCommand);
            return Ok(result);
        }

        [HttpDelete("delete/{Id}")]
        public async Task<IActionResult> Delete([FromRoute] DeleteLanguageTechnologyCommand deleteLanguageTechnologyCommand)
        {
            DeletedLanguageTechnologyDto result = await Mediator.Send(deleteLanguageTechnologyCommand);
            return Ok(result);
        }
    }
}
