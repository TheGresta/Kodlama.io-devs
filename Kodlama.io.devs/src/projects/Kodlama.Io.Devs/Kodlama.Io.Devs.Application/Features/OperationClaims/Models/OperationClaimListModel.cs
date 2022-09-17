using Core.Persistence.Paging;
using Kodlama.Io.Devs.Application.Features.OperationClaims.Dtos;

namespace Kodlama.Io.Devs.Application.Features.OperationClaims.Models
{
    public class OperationClaimListModel : BasePageableModel
    {
        public IList<ListOperationClaimDto> Items { get; set; }
    }
}
