using Core.Persistence.Repositories;
using Kodlama.Io.Devs.Domain.Entities;

namespace Kodlama.Io.Devs.Application.Services.Repositories
{
    public interface ILanguageTechnologyRepository : IAsyncRepository<LanguageTechnology>, IRepository<LanguageTechnology>
    {
    }
}
