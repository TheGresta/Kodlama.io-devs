using Core.Persistence.Repositories;
using Kodlama.Io.Devs.Application.Services.Repositories;
using Kodlama.Io.Devs.Domain.Entities;
using Kodlama.Io.Devs.Persistence.Contexts;

namespace Kodlama.Io.Devs.Persistence.Repositories
{
    public class LanguageTechnologyRepository : EfRepositoryBase<LanguageTechnology, BaseDbContext>, ILanguageTechnologyRepository
    {
        public LanguageTechnologyRepository(BaseDbContext context) : base(context)
        {
        }
    }
}
