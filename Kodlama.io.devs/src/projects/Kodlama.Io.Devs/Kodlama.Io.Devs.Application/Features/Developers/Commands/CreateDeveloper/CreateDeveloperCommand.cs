using AutoMapper;
using Kodlama.Io.Devs.Application.Features.Developers.Dtos;
using Kodlama.Io.Devs.Application.Services.Repositories;
using Kodlama.Io.Devs.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Kodlama.Io.Devs.Application.Features.Developers.Commands.CreateDeveloper
{
    public partial class CreateDeveloperCommand : IRequest<CreateDeveloperResultDto>
    {
        public Developer Developer { get; set; }

        public class CreateDeveloperCommandHandler : IRequestHandler<CreateDeveloperCommand, CreateDeveloperResultDto>
        {
            private readonly IDeveloperRepository _developerRepository;
            private readonly IMapper _mapper;

            public CreateDeveloperCommandHandler(IDeveloperRepository developerRepository, IMapper mapper)
            {
                _developerRepository = developerRepository;
                _mapper = mapper;
            }

            public async Task<CreateDeveloperResultDto> Handle(CreateDeveloperCommand request, CancellationToken cancellationToken)
            {
                Developer addedDeveloper = await _developerRepository.AddAsync(request.Developer, 
                                                                               include: x => x.Include(d => d.UserOperationClaims)
                                                                                              .ThenInclude(o => o.OperationClaim));

                CreateDeveloperResultDto createDeveloperResultDto = _mapper.Map<CreateDeveloperResultDto>(addedDeveloper);

                return createDeveloperResultDto;
            }
        }
    }
}
