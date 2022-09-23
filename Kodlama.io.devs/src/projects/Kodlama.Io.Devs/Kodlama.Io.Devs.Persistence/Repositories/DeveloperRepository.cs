using Core.Persistence.Repositories;
using Kodlama.Io.Devs.Application.Services.Repositories;
using Kodlama.Io.Devs.Domain.Entities;
using Kodlama.Io.Devs.Persistence.Contexts;

namespace Kodlama.Io.Devs.Persistence.Repositories
{
    public class DeveloperRepository : EfRepositoryBase<Developer, BaseDbContext>, IDeveloperRepository
    {
        public DeveloperRepository(BaseDbContext context) : base(context)
        {
        }
    }
}
