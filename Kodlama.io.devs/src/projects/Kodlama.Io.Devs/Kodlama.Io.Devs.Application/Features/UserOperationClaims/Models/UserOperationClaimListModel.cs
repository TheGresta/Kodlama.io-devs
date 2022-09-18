using Core.Persistence.Paging;
using Kodlama.Io.Devs.Application.Features.UserOperationClaims.Dtos;

namespace Kodlama.Io.Devs.Application.Features.UserOperationClaims.Models
{
    public class UserOperationClaimListModel : BasePageableModel
    {
        public IList<ListUserOperationClaimDto> Items { get; set; }
    }
}
