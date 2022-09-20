using AutoMapper;
using Kodlama.Io.Devs.Application.Features.LanguageTechnologies.Dtos;
using Kodlama.Io.Devs.Application.Features.LanguageTechnologies.Rules;
using Kodlama.Io.Devs.Application.Services.Repositories;
using Kodlama.Io.Devs.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Kodlama.Io.Devs.Application.Features.LanguageTechnologies.Commands.DeleteLanguageTechnology
{
    public partial class DeleteLanguageTechnologyCommand : IRequest<DeletedLanguageTechnologyDto>
    {
        public int Id { get; set; }
        public class DeleteLanguageTechnologyCommandHandler : IRequestHandler<DeleteLanguageTechnologyCommand, DeletedLanguageTechnologyDto>
        {
            private readonly ILanguageTechnologyRepository _languageTechnologyRepository;
            private readonly IMapper _mapper;
            private readonly LanguageTechnologyBusinessRules _languageTechnologyBusinessRules;

            public DeleteLanguageTechnologyCommandHandler(ILanguageTechnologyRepository languageTechnologyRepository,
                                                            IMapper mapper,
                                                            LanguageTechnologyBusinessRules languageTechnologyBusinessRules)
            {
                _languageTechnologyRepository = languageTechnologyRepository;
                _mapper = mapper;
                _languageTechnologyBusinessRules = languageTechnologyBusinessRules;
            }

            public async Task<DeletedLanguageTechnologyDto> Handle(DeleteLanguageTechnologyCommand request, CancellationToken cancellationToken)
            {
                await _languageTechnologyBusinessRules.LanguageTechnologyShouldBeExistWhenRequested(request.Id);

                LanguageTechnology? languageTechnology = await _languageTechnologyRepository.GetAsync(l => l.Id == request.Id);
                LanguageTechnology? deletedLanguageTechnology = await _languageTechnologyRepository.DeleteAsync(languageTechnology, include: x => x.Include(l => l.Language));
                DeletedLanguageTechnologyDto mappedLanguageTechnology = _mapper.Map<DeletedLanguageTechnologyDto>(deletedLanguageTechnology);

                return mappedLanguageTechnology;
            }
        }
    }
}
