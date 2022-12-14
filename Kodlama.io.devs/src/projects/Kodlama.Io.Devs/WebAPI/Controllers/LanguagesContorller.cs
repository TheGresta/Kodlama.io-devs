using Core.Application.Requests;
using Core.Persistence.Dynamic;
using Kodlama.Io.Devs.Application.Features.Languages.Commands.CreateLanguage;
using Kodlama.Io.Devs.Application.Features.Languages.Commands.DeleteLanguage;
using Kodlama.Io.Devs.Application.Features.Languages.Commands.UpdateLanguage;
using Kodlama.Io.Devs.Application.Features.Languages.Dtos;
using Kodlama.Io.Devs.Application.Features.Languages.Models;
using Kodlama.Io.Devs.Application.Features.Languages.Queries.GetByIdLanguage;
using Kodlama.Io.Devs.Application.Features.Languages.Queries.GetListLanguage;
using Kodlama.Io.Devs.Application.Features.Languages.Queries.GetListLanguageByDynamic;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LanguagesContorller : BaseController
    {
        [HttpPost("getlist/bydynamic")]
        public async Task<IActionResult> GetListByDynamic([FromQuery] PageRequest pageRequest, [FromBody] Dynamic dynamic)
        {
            GetListLanguageByDynamicQuery getListLanguageByDynamicQuery = new () { PageRequest = pageRequest, Dynamic = dynamic };
            LanguageListModel result = await Mediator.Send(getListLanguageByDynamicQuery);
            return Ok(result);

        }

        [HttpGet("getlist")]
        public async Task<IActionResult> GetList([FromQuery] PageRequest pageRequest)
        {
            GetListLanguageQuery getListLanguageQuery = new() { PageRequest =  pageRequest};
            LanguageListModel result = await Mediator.Send(getListLanguageQuery);
            return Ok(result);
        }

        [HttpGet("getbyid")]
        public async Task<IActionResult> GetById([FromQuery] GetByIdLanguageQuery getByIdLanguageQuery)
        {
            GetByIdLanguageDto result = await Mediator.Send(getByIdLanguageQuery);
            return Ok(result);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] CreateLanguageCommand createLanguageCommand)
        {
            CreatedLanguageDto result = await Mediator.Send(createLanguageCommand);
            return Created("", result);
        }

        [HttpPost("update")]
        public async Task<IActionResult> Update([FromBody] UpdateLanguageCommand updateLanguageCommand)
        {
            UpdatedLanguageDto result = await Mediator.Send(updateLanguageCommand);
            return Ok(result);
        }

        [HttpGet("delete")]
        public async Task<IActionResult> Delete([FromQuery] DeleteLanguageCommand deleteLanguageCommand)
        {
            DeletedLanguageDto result = await Mediator.Send(deleteLanguageCommand);
            return Ok(result);
        }
    }
}
