using Core.Persistence.Paging;
using Kodlama.Io.Devs.Application.Features.LanguageTechnologies.Dtos;

namespace Kodlama.Io.Devs.Application.Features.LanguageTechnologies.Models
{
    public class LanguageTechnologyListModel : BasePageableModel
    {
        public IList<ListLanguageTechnologyDto> Items { get; set; }
    }
}
