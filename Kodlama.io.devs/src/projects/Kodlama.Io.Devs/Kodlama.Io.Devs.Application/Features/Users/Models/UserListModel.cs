using Core.Persistence.Paging;
using Kodlama.Io.Devs.Application.Features.Users.Dtos;

namespace Kodlama.Io.Devs.Application.Features.Users.Models
{
    public class UserListModel : BasePageableModel
    {
        public IList<CommandUserDto> Items { get; set; }
    }
}
