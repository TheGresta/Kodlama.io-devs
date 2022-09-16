﻿using Core.Application.Requests;
using Kodlama.Io.Devs.Application.Features.Languages.Commands.CreateLanguage;
using Kodlama.Io.Devs.Application.Features.Languages.Commands.DeleteLanguage;
using Kodlama.Io.Devs.Application.Features.Languages.Commands.UpdateLanguage;
using Kodlama.Io.Devs.Application.Features.Languages.Dtos;
using Kodlama.Io.Devs.Application.Features.Languages.Models;
using Kodlama.Io.Devs.Application.Features.Languages.Queries.GetByIdLanguage;
using Kodlama.Io.Devs.Application.Features.Languages.Queries.GetListLanguage;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LanguagesContorller : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] PageRequest pageRequest)
        {
            GetListLanguageQuery getListLanguageQuery = new() { PageRequest =  pageRequest};
            LanguageListModel result = await Mediator.Send(getListLanguageQuery);
            return Ok(result);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById([FromRoute] GetByIdLanguageQuery getByIdLanguageQuery)
        {
            GetByIdLanguageDto result = await Mediator.Send(getByIdLanguageQuery);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateLanguageCommand createLanguageCommand)
        {
            CreatedLanguageDto result = await Mediator.Send(createLanguageCommand);
            return Created("", result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateLanguageCommand updateLanguageCommand)
        {
            UpdatedLanguageDto result = await Mediator.Send(updateLanguageCommand);
            return Ok(result);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete([FromRoute] DeleteLanguageCommand deleteLanguageCommand)
        {
            DeletedLanguageDto result = await Mediator.Send(deleteLanguageCommand);
            return Ok(result);
        }
    }
}