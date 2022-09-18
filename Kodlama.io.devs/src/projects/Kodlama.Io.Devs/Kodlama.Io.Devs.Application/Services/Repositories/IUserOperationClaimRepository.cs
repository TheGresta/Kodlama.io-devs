using Core.Persistence.Repositories;
using Core.Security.Entities;

namespace Kodlama.Io.Devs.Application.Services.Repositories
{
    public interface IUserOperationClaimRepository : IAsyncRepository<UserOperationClaim>, IRepository<UserOperationClaim>
    {
    }
}
