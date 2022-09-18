using Core.Persistence.Paging;
using Kodlama.Io.Devs.Application.Features.GitHubs.Dtos;

namespace Kodlama.Io.Devs.Application.Features.GitHubs.Models
{
    public class GitHubListModel : BasePageableModel
    {
        public IList<ListGitHubDto> Items { get; set; }
    }
}
