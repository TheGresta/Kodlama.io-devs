using Core.Persistence.Repositories;
using Core.Security.Entities;
using Kodlama.Io.Devs.Application.Services.Repositories;
using Kodlama.Io.Devs.Persistence.Contexts;

namespace Kodlama.Io.Devs.Persistence.Repositories
{
    public class OperationClaimRepository : EfRepositoryBase<OperationClaim, BaseDbContext>, IOperationClaimRepository
    {
        public OperationClaimRepository(BaseDbContext context) : base(context)
        {
        }
    }
}
